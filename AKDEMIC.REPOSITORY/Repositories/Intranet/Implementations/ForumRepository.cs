using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        public ForumRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetForums()
        {
            var result = await _context.Forums
                .Where(x => x.System == 2)
                .Select(s => new
                {
                    state = s.Active,
                    description = s.Description,
                    name = s.Name,
                    id = s.Id
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetoForumById(Guid id, int careerCount)
        {
            var forum = _context.Forums.Where(x => x.Id == id)
                .Include(x=>x.ForumCareers)
            .AsEnumerable()
            .Select(x => new
            {
                x.Active,
                x.Description,
                x.Id,
                x.Name,
                Careers = x.ForumCareers.Count == careerCount ? new Guid[] { Guid.Empty } : x.ForumCareers.Select(y => y.CareerId).ToArray()
            })
            .FirstOrDefault();

            return forum;
        }

        public async Task UpdateForumCompleted(ForumTemplate model)
        {
            var forumCareers = new List<ForumCareer>();

            if (model.Careers.Any(x => x == Guid.Empty))
            {
                forumCareers = await _context.Careers
                    .Select(x => new ForumCareer
                    {
                        ForumId = model.Id,
                        CareerId = x.Id
                    }).ToListAsync();
            }
            else
            {
                forumCareers = model.Careers
                    .Select(x => new ForumCareer
                    {
                        ForumId = model.Id,
                        CareerId = x
                    }).ToList();
            }

            var forum = await _context.Forums.FindAsync(model.Id);

            var oldForumCareers = await _context.ForumCareers.Where(x => x.ForumId == forum.Id).ToListAsync();
            _context.ForumCareers.RemoveRange(oldForumCareers);
            
            forum.Name = model.Name;
            forum.Description = model.Description;
            forum.Active = model.Active;
            forum.ForumCareers = forumCareers;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteForum(Guid id)
        {
            var forum = await _context.Forums.FindAsync(id);

            var forumcareers = await _context.ForumCareers.Where(x => x.ForumId == forum.Id).ToListAsync();

            _context.ForumCareers.RemoveRange(forumcareers);
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Forum>> GetAllBySystem(int system)
        {
            var query = _context.Forums
                .Where(x => x.System == system);

            return await query.ToListAsync();
        }

        public async Task<Forum> GetWithIncludes(Guid id)
        {
            var query = _context.Forums
                .Include(x => x.ForumCareers)
                .Include(x => x.Topics)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async override Task DeleteById(Guid id)
        {
            var forum = await _context.Forums
                .Include(x => x.ForumCareers)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            _context.ForumCareers.RemoveRange(forum.ForumCareers);
            await _context.SaveChangesAsync();
            await base.DeleteById(id);
        }

        public async Task<object> GetCustomForumByCareer(Guid careerId, int system)
        {
            var query = await _context.Forums
                                .Where(x =>
                                x.Active == true &&
                                x.System == system &&
                                x.ForumCareers.Any(y => y.CareerId == careerId))
                                .OrderBy(x => x.Name)
                                .ToListAsync();
            return query;
        }

        public async Task<List<Forum>> IndexForumJobExchange(string userId, int system, string search = null)
        {
            var forums = new List<Forum>();
            var user = await _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            var query = _context.Forums.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var trimSearch = search.Trim().ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(trimSearch));
            }

            if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.STUDENTS))
            {
                var careerId = await _context.Students
                    .Where(x => x.UserId == userId)
                    .Select(x => x.CareerId)
                    .FirstOrDefaultAsync();

                forums = await query
                                    .Where(x =>
                                    x.Active == true &&
                                    x.System == system &&
                                    x.ForumCareers.Any(y => y.CareerId == careerId))
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();
            }
            else if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.CAREER_DIRECTOR || x.Role.Name == ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var careerId = _context.Careers
                    .Where(x => x.AcademicCoordinatorId == userId || x.AcademicSecretaryId == userId)
                    .Select(x => x.Id).FirstOrDefault();

                forums = await query
                                    .Where(x =>
                                    x.Active == true &&
                                    x.System == system &&
                                    x.ForumCareers.Any(y => y.CareerId == careerId))
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();

                if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.TUTORING_COORDINATOR))
                {
                    var careerIdT = await _context.TutoringCoordinators.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();

                    forums = await query
                                        .Where(x =>
                                        x.Active == true &&
                                        x.System == system &&
                                        x.ForumCareers.Any(y => y.CareerId == careerIdT))
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
                }
                if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.TUTOR))
                {
                    var careerIdT = await _context.Tutors.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();

                    forums = await query
                                        .Where(x =>
                                        x.Active == true &&
                                        x.System == system &&
                                        x.ForumCareers.Any(y => y.CareerId == careerIdT))
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
                }
            }
            else if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
            {
                var careerList = await _context.CoordinatorCareers.Where(x => x.UserId == userId).Select(x => x.CareerId).ToListAsync();

                forums = await query
                                    .Where(x =>
                                    x.Active == true &&
                                    x.System == system &&
                                    x.ForumCareers.Any(y => careerList.Contains(y.CareerId)))
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();

            }
            else if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.TEACHERS))
            {
                var teacher = await _context.Teachers
                    .Where(x => x.UserId == userId)
                    .Select(x => new 
                    {
                        x.AcademicDepartmentId,
                        AcademicDepartmentCareerId = x.AcademicDepartment.CareerId,
                        x.CareerId,
                        AcademicDepartmentFacultyId = x.AcademicDepartmentId == null ? null : (Guid?)x.AcademicDepartment.FacultyId,
                    })
                    .FirstOrDefaultAsync();

                forums = await query
                    .Where(x => x.Active && x.System == system)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                if (teacher != null && teacher.AcademicDepartmentId != null)
                {
                    forums = await query
                        .Where(x => x.Active && x.System == system &&
                            x.ForumCareers.Any(y => y.Career.FacultyId == teacher.AcademicDepartmentFacultyId))
                        .OrderBy(x => x.Name)
                        .ToListAsync();
                }


                if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.TUTORING_COORDINATOR))
                {
                    var careerId = await _context.TutoringCoordinators.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();

                    forums = await query
                                        .Where(x =>
                                        x.Active == true &&
                                        x.System == system &&
                                        x.ForumCareers.Any(y => y.CareerId == careerId))
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
                }
                if (user.UserRoles.Any(x => x.Role.Name == ConstantHelpers.ROLES.TUTOR))
                {
                    var careerId = await _context.Tutors.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();

                    forums = await query
                                        .Where(x =>
                                        x.Active == true &&
                                        x.System == system &&
                                        x.ForumCareers.Any(y => y.CareerId == careerId))
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
                }

            }
            else
            {
                forums = await query
                                    .Where(x => x.Active == true && x.System == system)
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();
            }

            return forums;
        }

        public async Task DeteleInformationForum(Guid ForumId)
        {
            var forum = await _context.Forums.Include(x => x.Topics).Where(x => x.Id == ForumId).FirstOrDefaultAsync();
            if (forum.Topics != null)
            {
                foreach (var topic in forum.Topics)
                {
                    var posts = await _context.Posts.Where(x => x.TopicId == topic.Id).ToListAsync();
                    if (posts.Count > 0)
                    {
                        _context.Posts.RemoveRange(posts);
                        await _context.SaveChangesAsync();
                    }
                }
                _context.Topics.RemoveRange(forum.Topics);
            }
            await _context.SaveChangesAsync();
            var forumCareers = await _context.ForumCareers.Where(x => x.ForumId == forum.Id).ToListAsync();
            if (forumCareers.Count > 0)
            {
                _context.ForumCareers.RemoveRange(forumCareers);
                await _context.SaveChangesAsync();
            }
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasTopics(Guid forumId)
            => await _context.Topics.AnyAsync(x => x.ForumId == forumId);

        public async Task<List<Forum>> GetForumBySystem(int system)
            => await _context.Forums.Where(x => x.System == system).ToListAsync();
        public async Task<List<Forum>> GetForumBySystemToTutoring()
            => await _context.Forums.Where(x => x.System == ConstantHelpers.Solution.Forum_Tutoring_Coordinator || x.System == ConstantHelpers.Solution.Forum_Tutoring_Tutor).ToListAsync();
    }
}
