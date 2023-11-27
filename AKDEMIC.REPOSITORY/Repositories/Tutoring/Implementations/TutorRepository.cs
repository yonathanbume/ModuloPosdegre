using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
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
    public class TutorRepository : Repository<Tutor>, ITutorRepository
    {
        public TutorRepository(AkdemicContext context) : base(context) { }

        public override async Task<IEnumerable<Tutor>> GetAll()
        {
            return await _context.Tutors
                           .Include(x => x.User)
                           .ToListAsync();
        }

        public override async Task<Tutor> Get(string tutorId)
        {
            return await _context.Tutors
                           .Where(t => t.UserId == tutorId)
                           .Include(t => t.User)
                           .Include(c => c.Career)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tutor>> GetAllByCareerIdAndTermId(Guid careerId, Guid? termId = null, string search = null)
        {
            var query = _context.Tutors
                            .Where(q => q.CareerId == careerId)
                           .Include(x => x.User)
                           .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(q => q.TutorTutoringStudents.Any(ts => ts.TutoringStudentTermId == termId));

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(search)
                    || x.User.UserName.ToUpper().Contains(search));

            return await query
                           .ToListAsync();
        }

        public async Task<int> CountByCareerId(Guid? careerId = null)
        {
            var query = _context.Tutors.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            return await query.CountAsync();
        }
        public async Task<int> CountByTutorsectionId(Guid careerId, Guid termId)
        {
            var query = _context.Tutors.Where(x => x.CareerId == careerId && x.TutorTutoringStudents.Any(y => y.TutorId == x.UserId && y.TutoringStudentTermId == termId)).AsQueryable();



            return await query.CountAsync();
        }

        public async Task<int> SumTimesUpdatedByCareerId(Guid? careerId = null)
        {
            var query = _context.Tutors.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            return await query.SumAsync(x => x.TimesUpdated);
        }

        public async Task<IEnumerable<Tutor>> GetAllByTutoringStudentId(Guid tutoringStudentId)
        {
            return await _context.Tutors
                           .Include(x => x.User)
                           .Where(x => x.TutorTutoringStudents.Any(tss => tss.TutoringStudentStudentId == tutoringStudentId))
                           .ToListAsync();
        }

        public async Task<int> CountDictatedTutoringSession(int times, Guid? termId = null, Guid? careerId = null)
        {
            return await _context.Tutors
                           .Where(x => x.TutoringSessions.Where(ts => (termId.HasValue ? ts.TermId == termId.Value : true)
                                                                        && (careerId.HasValue ? ts.Tutor.CareerId == careerId.Value : true)
                                                                        && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == times)
                           .CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null, int? type = null)
        {

            Expression<Func<Tutor, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Type);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.Tutors
                .AsNoTracking();

            if (careerId.HasValue)
                query = query.Where(q => q.CareerId == careerId);

            if (type.HasValue && type != 0)
                query = query.Where(q => q.Type == type);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                                || x.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                                || x.User.Name.ToUpper().Contains(searchValue.ToUpper())
                                || x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
                                || x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    userId = x.UserId,
                    career = x.Career.Name,
                    name = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    dni = x.User.Dni,
                    userName = x.User.UserName,
                    picture = x.User.Picture,
                    fullName = x.User.FullName,
                    faculty = x.Career.Faculty.Name,
                    type = ConstantHelpers.TUTORING.TYPETUTOR.VALUES.ContainsKey(x.Type) ? ConstantHelpers.TUTORING.TYPETUTOR.VALUES[x.Type] : ""
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<Teacher>> GetTutorsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? careerId = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Faculty.Name);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
            }

            var query = _context.Teachers.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                var test = await _context.TeacherSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId).Select(x => x.TeacherId).ToListAsync();

                query = query.Where(x => test.Any(y => y == x.UserId));
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.User.Name.ToUpper().Contains(searchValue)
                            || x.User.MaternalSurname.ToUpper().Contains(searchValue)
                            || x.User.PaternalSurname.ToUpper().Contains(searchValue)
                            || x.User.UserName.ToUpper().Contains(searchValue));

            query = query
                .AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Teacher
                {
                    UserId = x.UserId,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        Picture = x.User.Picture,
                        FullName = x.User.FullName
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name,
                        Faculty = new ENTITIES.Models.Enrollment.Faculty
                        {
                            Name = x.Career.Faculty.Name
                        },
                    },
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Teacher>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<SupportOfficeUser> GetTutorSupportOffice(string tutorId)
        {
            return await _context.SupportOfficeUsers.Where(x => x.UserId == tutorId).FirstOrDefaultAsync();
        }

        public async Task<Tutor> GetWithData(string tutorId)
            => await _context.Tutors.Include(x => x.Career).Include(x => x.User).Where(x => x.UserId == tutorId).FirstOrDefaultAsync();
        public async Task<Tutor> GetByUser(string userId)
            => await _context.Tutors.Include(x => x.Career).Where(x => x.UserId == userId).FirstOrDefaultAsync();

        public async Task<IEnumerable<Tutor>> GetAllByCareerIdAndHasTutoringStudent(Guid careerId, Guid termId, string search = null)
        {
            var query = _context.Tutors.Include(x => x.User)
                .Where(x => x.CareerId == careerId && x.TutorTutoringStudents.Any(y => y.TutorId == x.UserId && y.TutoringStudentTermId == termId)).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(search)
                    || x.User.UserName.ToUpper().Contains(search));

            return await query
                           .ToListAsync();

        }

        public async Task<object> GetReportCountByCareer(Guid termId, string searchValue)
        {
  
            var query = _context.Careers.Where(x => x.Tutors.Count > 0).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper())
                            || x.Name.ToUpper().Contains(searchValue.ToUpper()));


            var recordsFiltered = await query.CountAsync();

            query = query
                .AsQueryable();


            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    tutors = x.Tutors.Where(x => x.TutorTutoringStudents.Any(tts => tts.TutorId == x.UserId && tts.TutoringStudentTermId == termId)).Count(),
                    dataTutors = x.Tutors.Where(x => x.TutorTutoringStudents.Any(tts => tts.TutorId == x.UserId && tts.TutoringStudentTermId == termId)).Sum(y => y.TimesUpdated),
                    tutoringStudents = x.Students.Where(x => x.TutoringStudents.Any(ts => ts.StudentId == x.Id && ts.TutorTutoringStudents.Any(tts => tts.TutoringStudentStudentId == ts.StudentId && tts.TutoringStudentTermId == termId))).Count(),
                    dataTutoringStudents = _context.TutoringStudents.Where(y => y.Student.CareerId == x.Id).Sum(y => y.TimesUpdated),

                    singleSessions = _context.TutoringSessions.Where(y => y.TermId == termId && y.Tutor.CareerId == x.Id && y.EndTime < DateTime.UtcNow && y.IsDictated && y.TutoringSessionStudents.Count() == 1).Count(),

                    groupSessions1 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 1).Count(),
                    groupSessions2 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 2).Count(),
                    groupSessions3 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 3).Count(),
                    groupSessions4 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 4).Count(),
                    groupSessions5 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 5).Count(),
                    groupSessions6 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 6).Count(),
                    groupSessions7 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 7).Count(),

                    groupSessions8 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 8).Count(),
                    groupSessions9 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 9).Count(),
                    groupSessions10 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 10).Count(),
                    groupSessions11 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 11).Count(),
                    groupSessions12 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 12).Count(),
                    groupSessions13 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 13).Count(),

                    referred = _context.TutoringSessionStudents.Where(y => y.SupportOfficeId.HasValue && y.TutoringSession.TermId == termId && y.TutoringSession.Tutor.CareerId == x.Id).Count(),
                    attended = _context.TutoringSessionStudents.Where(y => y.Attended && y.TutoringSession.TermId == termId && y.TutoringSession.Tutor.CareerId == x.Id).Count()
                }).Where(x => x.tutors > 0).ToListAsync();

            //var recordsTotal = data.Count;

            //return new DataTablesStructs.ReturnedData<object>
            //{
            //    Data = data,
            //    DrawCounter = sentParameters.DrawCounter,
            //    RecordsFiltered = recordsFiltered,
            //    RecordsTotal = recordsTotal
            //};

            return data.OrderBy(x => x.code);

        }
        public async Task<List<ReportCountByCareerTemplate>> GetReportCountByCareerTemplate(Guid termId)
        {
            var query = _context.Careers.Where(x => x.Tutors.Count > 0).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new ReportCountByCareerTemplate
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Tutors = x.Tutors.Where(x => x.TutorTutoringStudents.Any(tts => tts.TutorId == x.UserId && tts.TutoringStudentTermId == termId)).Count(),
                    DataTutors = x.Tutors.Where(x => x.TutorTutoringStudents.Any(tts => tts.TutorId == x.UserId && tts.TutoringStudentTermId == termId)).Sum(y => y.TimesUpdated),
                    TutoringStudents = x.Students.Where(x => x.TutoringStudents.Any(ts => ts.StudentId == x.Id && ts.TutorTutoringStudents.Any(tts => tts.TutoringStudentStudentId == ts.StudentId && tts.TutoringStudentTermId == termId))).Count(),
                    DataTutoringStudents = _context.TutoringStudents.Where(y => y.Student.CareerId == x.Id).Sum(y => y.TimesUpdated),

                    SingleSessions = _context.TutoringSessions.Where(y => y.TermId == termId && y.Tutor.CareerId == x.Id && y.EndTime < DateTime.UtcNow && y.IsDictated && y.TutoringSessionStudents.Count() == 1).Count(),

                    GroupSessions1 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 1).Count(),
                    GroupSessions2 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 2).Count(),
                    GroupSessions3 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 3).Count(),
                    GroupSessions4 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 4).Count(),
                    GroupSessions5 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 5).Count(),
                    GroupSessions6 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 6).Count(),
                    GroupSessions7 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 7).Count(),

                    GroupSessions8 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 8).Count(),
                    GroupSessions9 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 9).Count(),
                    GroupSessions10 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 10).Count(),
                    GroupSessions11 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 11).Count(),
                    GroupSessions12 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 12).Count(),
                    GroupSessions13 = x.Tutors.Where(x => x.TutoringSessions.Where(ts => ts.TutorId == x.UserId && ts.TermId == termId && ts.IsDictated && ts.TutoringSessionStudents.Count() > 1).Count() == 13).Count(),

                    Referred = _context.TutoringSessionStudents.Where(y => y.SupportOfficeId.HasValue && y.TutoringSession.TermId == termId && y.TutoringSession.Tutor.CareerId == x.Id).Count(),
                    Attended = _context.TutoringSessionStudents.Where(y => y.Attended && y.TutoringSession.TermId == termId && y.TutoringSession.Tutor.CareerId == x.Id).Count()
                }).OrderBy(x => x.Code).ToListAsync();

            return data;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllTutoringsMadeByTutorByFacultyIdAsDataTable(DataTablesStructs.SentParameters sentParameters, Guid ? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Tutors.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    name = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    career = x.Career.Name,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<List<TutorTemplate>> GetTutorsReportExcel(Guid? careerId = null, int? type = null)
        {          
            var query = _context.Tutors
                .AsNoTracking();

            if (careerId.HasValue)
                query = query.Where(q => q.CareerId == careerId);

            if (type.HasValue && type != 0)
                query = query.Where(q => q.Type == type);


            var data = await query
                .Select(x => new TutorTemplate
                {
                    Career = x.Career.Name,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Dni = x.User.Dni,
                    UserName = x.User.UserName,
                    Type = ConstantHelpers.TUTORING.TYPETUTOR.VALUES.ContainsKey(x.Type) ? ConstantHelpers.TUTORING.TYPETUTOR.VALUES[x.Type] : ""
                })
                .ToListAsync();

            return data;
        }
    }
}
