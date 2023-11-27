using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringSessionRepository : Repository<TutoringSession>, ITutoringSessionRepository
    {
        public TutoringSessionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutorId(DateTime start, DateTime end, string tutorId, bool? isMultiple = null, bool? isDictated = null)
        {
            var query = _context.TutoringSessions
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus)
                .Where(x => x.StartTime >= start && x.EndTime <= end)
                .Where(x => x.TutorId == tutorId)
                .AsQueryable();

            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);

            if (isDictated.HasValue)
                query = query.Where(x => x.IsDictated == isDictated.Value);

            return await query.ToListAsync();
        }
            
        public async Task<IEnumerable<TutoringSession>> GetAllByDateRangeAndTutoringStudentId(DateTime start, DateTime end, Guid tutoringStudentId, bool? isMultiple = null, bool? isDictated = null)
        {
            var query = _context.TutoringSessions
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus)
                .Where(x => x.StartTime >= start && x.EndTime <= end)
                .Where(x => x.TutoringSessionStudents.Any(ts => ts.TutoringStudentStudentId == tutoringStudentId))
                .AsQueryable();

            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);

            if (isDictated.HasValue)
                query = query.Where(x => x.IsDictated == isDictated.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TutoringSession>> GetAllByType(bool isMultiple)
            => await _context.TutoringSessions
                .Where(x => isMultiple ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1)
                .ToListAsync();

        public override async Task DeleteById(Guid tutoringSessionId)
        {
            var tutoringSession = await _context.TutoringSessions
                .Include(x => x.TutoringSessionStudents)
                .FirstOrDefaultAsync(x => x.Id == tutoringSessionId);
            if (tutoringSession.TutoringSessionStudents.Any())
            {
                _context.TutoringSessionStudents.RemoveRange(tutoringSession.TutoringSessionStudents);
                await _context.SaveChangesAsync();
            }
            await base.Delete(tutoringSession);
        }

        public override async Task Delete(TutoringSession tutoringSession)
        {
            if (tutoringSession.TutoringSessionStudents.Any())
            {
                _context.TutoringSessionStudents.RemoveRange(tutoringSession.TutoringSessionStudents);
                await _context.SaveChangesAsync();
            }
            await base.Delete(tutoringSession);
        }

        public async Task<bool> AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId = null)
        {
            var query = _context.TutoringSessions.AsQueryable();

            if (exceptionId.HasValue)
                query = query.Where(c => c.Id != exceptionId.Value);

            return await query.AnyAsync(c => c.ClassroomId == classroomId && (c.StartTime < end && start < c.EndTime));
        }

        public async Task<TutoringSession> GetByTutorIdAndDateRange(string tutorId, DateTime start, DateTime end, Guid? exceptionId = null)
        {
            var query = _context.TutoringSessions.AsQueryable();

            if (exceptionId.HasValue)
                query = query.Where(c => c.Id != exceptionId.Value);

            return await query.FirstOrDefaultAsync(c => c.TutorId == tutorId && (c.StartTime < end && start < c.EndTime));
        }

        public async Task<TutoringSession> GetByTutorIdAndDate(string tutorId, DateTime time, bool? isMultiple = null)
        {
            var query = _context.TutoringSessions
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus)
                .Include(x => x.Classroom)
                .ThenInclude(x => x.ClassroomType)
                .AsQueryable();
            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);
            return await query.FirstOrDefaultAsync(x => x.TutorId == tutorId && x.StartTime <= time && x.EndTime >= time);
        }
        
        public async Task<TutoringSession> Get(Guid id, string tutorId = null, bool? isMultiple = null)
        {
            var query = _context.TutoringSessions
                .Include(x => x.Tutor).ThenInclude(x => x.User)
                .Include(x => x.TutoringSessionStudents)
                .Include(x => x.TutoringSessionProblems)
                .ThenInclude(x => x.TutoringProblem)
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus)
                .Include(x => x.Classroom)
                .ThenInclude(x => x.ClassroomType)
                .AsQueryable();
            if(isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);
            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorId == tutorId);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TutoringSession> GetNearestSessionByTutorId(string tutorId, DateTime? time = null, bool? isMultiple = null)
        {
            time = time ?? DateTime.UtcNow;
            var query = _context.TutoringSessions.Where(x => x.TutorId == tutorId && x.StartTime > time)
                        .OrderBy(x => x.StartTime).AsQueryable();
            if(isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> CountByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, bool? isMultiple = null, bool? isDictated = null)
        {
            var query = _context.TutoringSessions.AsQueryable();

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorId == tutorId);
            if(termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);
            if (careerId.HasValue)
                query = query.Where(x => x.Tutor.CareerId == careerId.Value);
            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringSessionStudents.Any(tss => tss.TutoringStudentStudentId == tutoringStudentId.Value));
            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);
            if (isDictated.HasValue)
                query = query.Where(x => x.EndTime < DateTime.UtcNow && x.IsDictated == isDictated);

            return await query.CountAsync();
        }

        public async Task<int> CountReferredByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null)
        {
            var query = _context.TutoringSessionStudents
                .Where(x => x.SupportOfficeId.HasValue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutoringSession.TutorId == tutorId);
            if (termId.HasValue)
                query = query.Where(x => x.TutoringSession.TermId == termId.Value);
            if (careerId.HasValue)
                query = query.Where(x => x.TutoringSession.Tutor.CareerId == careerId.Value);
            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringStudentStudentId == tutoringStudentId.Value);
            if (supportOfficeId.HasValue)
                query = query.Where(x => x.SupportOfficeId == supportOfficeId.Value);

            return await query.CountAsync();
        }

        public async Task<int> CountAttendedByTutorIdAndCareerIdAndTutoringStudentId(string tutorId = null, Guid? termId = null, Guid? careerId = null, Guid? tutoringStudentId = null, Guid? supportOfficeId = null)
        {
            var query = _context.TutoringSessionStudents
               .Where(x => x.Attended)
               .AsQueryable();

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutoringSession.TutorId == tutorId);
            if (termId.HasValue)
                query = query.Where(x => x.TutoringSession.TermId == termId.Value);
            if (careerId.HasValue)
                query = query.Where(x => x.TutoringSession.Tutor.CareerId == careerId.Value);
            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringStudentStudentId == tutoringStudentId.Value);
            if (supportOfficeId.HasValue)
                query = query.Where(x => x.SupportOfficeId == supportOfficeId.Value);

            return await query.CountAsync();
        }

        public async Task<int> CountByDateRangeAndCareerId(DateTime startTime, DateTime endTime, Guid? careerId = null, Guid? termId = null, bool? isMultiple = null, bool? isDictated = null, bool? isAttended = null)
        {
            var query = _context.TutoringSessions
                .Where(x => x.StartTime > startTime && x.EndTime < endTime)
                .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.Tutor.CareerId == careerId.Value);
            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);
            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);
            if (isDictated.HasValue)
                query = query.Where(x => x.IsDictated == isDictated.Value);
            if (isAttended.HasValue)
                query = query.Where(x => x.TutoringSessionStudents.All(y => y.Attended == isAttended.Value));

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TutoringSession>> GetAllWithInclude()
        {
            var query = await _context.TutoringSessions.Include(x => x.Tutor).Include(x => x.TutoringSessionStudents).ToListAsync();

            return query;
        }

        public async Task<TutoringSession> GetAllWithData(Guid tutoringSessionId)
            => await _context.TutoringSessions
            .Include(x => x.TutoringSessionProblems).ThenInclude(x => x.TutoringProblem)
            .Include(x => x.Tutor).ThenInclude(x => x.User)
            .Where(x => x.Id == tutoringSessionId).FirstOrDefaultAsync();
        public async Task<DataTablesStructs.ReturnedData<TutoringSession>> GetTutoringSessionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string tutorId = null, bool? isMultiple = null, bool? isPast = null, bool? isDictated = null, bool? individually = null, int? typeSection = null, Guid? term = null)
        {

            Expression<Func<TutoringSession, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.StartTime);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.StartTime);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Classroom.Building.Campus.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Classroom.Building.Name);

                    break;
                case "4":
                    orderByPredicate = ((x) => new { x.Classroom.Description, x.Classroom.ClassroomType.Name });

                    break;
                default:
                    orderByPredicate = ((x) => x.StartTime);

                    break;
            }


            var query = _context.TutoringSessions
                .Where(x => x.TutorId == tutorId)
                .AsNoTracking();

            if (isMultiple.HasValue)
                query = query.Where(x => isMultiple.Value ? x.TutoringSessionStudents.Count() > 1 : x.TutoringSessionStudents.Count() == 1);

            if (typeSection.HasValue)
            {
                if (typeSection == 1)
                    query = query.Where(x => x.TutoringSessionStudents.Count() == 1);
                if(typeSection == 2)
                    query = query.Where(x => x.TutoringSessionStudents.Count() > 1);
            }

            if (term.HasValue)
                query = query.Where(x => x.TermId == term);
            if (isDictated.HasValue)
            {
                if (isDictated.Value)
                {
                    if (individually.HasValue && individually.Value) // dictadas para individuales, solo se asegura que hayan pasado ya que estas no tienen la toma de asistencia convencional que les cambia el estado hasta que registren problemas o refieran
                        query = query.Where(x => x.EndTime < DateTime.UtcNow);
                    else
                        query = query.Where(x => x.IsDictated && x.EndTime < DateTime.UtcNow); //dictadas para grupales y por defecto, asegura que hayan pasado y tengan el estado en true ya que en la toma de asistencia se cambia dicho estado
                }
                else
                    query = query.Where(x => !x.IsDictated); //una no dictada puede o no ser pasado
            }

            //if (isPast.HasValue)
            //{
            //    if (isPast.Value)
            //        query = query.Where(x => x.EndTime < DateTime.UtcNow);
            //    else
            //        query = query.Where(x => x.EndTime > DateTime.UtcNow);
            //}

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Classroom.Description.ToUpper().Contains(searchValue)
                            || x.Classroom.Building.Name.ToUpper().Contains(searchValue)
                            || x.Classroom.Building.Campus.Name.ToUpper().Contains(searchValue));

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == "DESC", orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringSession
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    ClassroomId = x.ClassroomId,
                    TutorId = x.TutorId,
                    Classroom = new ENTITIES.Models.Enrollment.Classroom
                    {
                        Description = x.Classroom.Description,
                        ClassroomTypeId = x.Classroom.ClassroomTypeId,
                        ClassroomType = new ENTITIES.Models.Enrollment.ClassroomType
                        {
                            Name = x.Classroom.ClassroomType.Name
                        },
                        BuildingId = x.Classroom.BuildingId,
                        Building = new ENTITIES.Models.Enrollment.Building
                        {
                            Name = x.Classroom.Building.Name,
                            CampusId = x.Classroom.Building.CampusId,
                            Campus = new ENTITIES.Models.Enrollment.Campus
                            {
                                Name = x.Classroom.Building.Campus.Name
                            }
                        }
                    },
                    TutoringSessionStudents = x.TutoringSessionStudents
                })
                .ToListAsync();

            var recordsTotal =  data.Count;

            return new DataTablesStructs.ReturnedData<TutoringSession>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        
        public async Task<object> GetAllTutorsChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TutoringSessions.AsQueryable();

            if (termId != null)
                query = query.Where(x => x.TermId == termId);



            var tutors = await query
                .Select(x => new
                {
                    x.TutorId,
                    x.Tutor.CareerId
                })
                .Distinct()
                .ToListAsync();

            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var careers = await careersQuery
                .Select(x => new
                {
                    x.Name,
                    x.Id
                }).ToListAsync();

            var data = careers
                .Select(x => new
                {
                    x.Name,
                    Count = tutors.Where(y => y.CareerId == x.Id).Count()
                }).ToList();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetAllTutoringsMadeByTutorByFacultyIdAsData(int tutoringSessionType, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TutoringSessions
               .Where(x => x.IsDictated)
               .AsQueryable();

            var carersQuery = _context.Careers.AsNoTracking();


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                    carersQuery = carersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
            {
                query = query.Where(x => x.Tutor.CareerId == careerId);
                carersQuery = carersQuery.Where(x => x.Id == careerId);
            }


            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            var tutoringData = new List<TutoringTypeViewModel>();

            if (tutoringSessionType == 1) //Individual
            {
                tutoringData = await query
                    .Where(x => x.TutoringSessionStudents.Count() == 1)
                    .Select(x => new TutoringTypeViewModel
                    {
                        CareerId = x.Tutor.CareerId
                    })
                    .ToListAsync();

            }
            else if (tutoringSessionType == 2) //Grupal
            {
                tutoringData = await query
                    .Where(x => x.TutoringSessionStudents.Count() > 1)
                    .Select(x => new TutoringTypeViewModel
                    {
                        CareerId = x.Tutor.CareerId
                    })
                    .ToListAsync();
            }

            var careers = await carersQuery
                .Select(x => new
                {
                    x.Name,
                    x.Id
                }).ToListAsync();

            var data = careers
                .Select(x => new
                {
                    x.Name,
                    Count = tutoringData.Where(y => y.CareerId == x.Id).Count()
                }).ToList();


            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }
    
        public async Task<int> GetTutoringCount(string teacherId = null)
        {
            var tutorSessions = _context.TutoringSessions
                .Where(x => x.IsDictated)
                .AsQueryable();

            int result = 0;

            if (!string.IsNullOrEmpty(teacherId))
            {
                result = await tutorSessions.Where(x => x.TutorId == teacherId).CountAsync();
            }

            return result;
        }

        public async Task<object> GetTutorsTeacherSelect2(ClaimsPrincipal user = null)
        {
            var tutors = _context.TutoringSessions.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    tutors = tutors.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                }
            }

            var result = await tutors
                    .Select(x => new
                    {
                        id = x.Tutor.UserId,
                        text = x.Tutor.User.FullName
                    })
                    .Distinct()
                    .ToListAsync();

            return result;
        }

        public async Task<List<TutoringSession>> GetAllByTutor(string tutorId)
        {
            var result = await _context.TutoringSessions
                .Where(x => x.TutorId == tutorId)
                .ToListAsync();

            return result;
        }

        public async Task<bool> AnyByTutor(string tutorId)
        {
            var result = await _context.TutoringSessions
                .AnyAsync(x => x.TutorId == tutorId);

            return result;
        }
    }
}
