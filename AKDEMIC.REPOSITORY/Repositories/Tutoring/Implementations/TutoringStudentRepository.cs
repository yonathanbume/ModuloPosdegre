using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringStudentRepository : Repository<TutoringStudent>, ITutoringStudentRepository
    {
        public TutoringStudentRepository(AkdemicContext context) : base(context) { }

        public async Task<TutoringStudent> GetWithData(Guid tutoringStudentId, Guid termId)
            => await _context.TutoringStudents
            .Where(t => t.StudentId == tutoringStudentId && t.TermId == termId)
            .Include(t => t.Student).ThenInclude(t => t.User)
            .Include(x => x.Term)
            .FirstOrDefaultAsync();
        public async Task<bool> AnyByTermId(Guid tutoringStudentId, Guid termId)
            => await _context.TutoringStudents.AnyAsync(x => x.TermId == termId && x.StudentId == tutoringStudentId);
        public override async Task<IEnumerable<TutoringStudent>> GetAll()
            => await _context.TutoringStudents
                .Include(t => t.Student)
                .ThenInclude(t => t.User)
                .ToListAsync();

        public async Task<TutoringStudent> GetByUserId(string userId)
            => await _context.TutoringStudents
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Student)
                .ThenInclude(x => x.Career)
                .Where(x => x.Student.UserId == userId)
                .FirstOrDefaultAsync();

        public async Task<TutoringStudent> GetByStudentId(Guid studentId)
         => await _context.TutoringStudents
             .Include(x => x.Student)
             .ThenInclude(x => x.User)
             .Include(x => x.Student)
             .ThenInclude(x => x.Career)
             .Where(x => x.StudentId == studentId)
             .FirstOrDefaultAsync();

        public async Task<IEnumerable<TutoringStudent>> GetAllByCareerId(Guid careerId)
            => await _context.TutoringStudents
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Where(x => x.Student.CareerId == careerId)
                .ToListAsync();

        public async Task<IEnumerable<TutoringStudent>> GetNewTutoringStudentsForTutor(string tutorId, string query, Guid? careerId = null)
        {
            var queryData = _context.TutoringStudents
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Student)
                .ThenInclude(x => x.AcademicHistories)
                .Where(x => !x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId))
                .AsQueryable();
            if (careerId.HasValue)
                queryData = queryData.Where(x => x.Student.CareerId == careerId.Value);
            if (!string.IsNullOrEmpty(query))
                queryData = queryData
                    .Where(x => x.Student.User.Name.Contains(query)
                            || x.Student.User.MaternalSurname.Contains(query)
                            || x.Student.User.PaternalSurname.Contains(query));
            return await queryData.ToListAsync();
        }

        public async Task<object> GetNewTutoringStudentsAllForTutor(Select2Structs.RequestParameters requestParameters, Guid termId, string query, Guid? careerId = null, int? academicYear = null, int? condition = null)
        {
            var queryData = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)).AsNoTracking();

            if (careerId.HasValue)
                queryData = queryData.Where(q => q.CareerId == careerId);

            if (academicYear.HasValue && academicYear != 0)
                queryData = queryData.Where(x => x.CurrentAcademicYear == academicYear);
            if (condition.HasValue && condition != 0)
            {
                if (condition == 1)
                {
                    queryData = queryData.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Try == 1));
                }
                else if (condition == 2)
                {
                    queryData = queryData.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Try == 2));
                }
                else
                {
                    queryData = queryData.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Try > 2));
                }
            }

            queryData = queryData.Where(x => !x.TutoringStudents.Any(y => y.TutorTutoringStudents.Any(i => i.TutoringStudentTermId == termId)));

            //var students = queryData.AsQueryable();
            //var tutorTutoringStudent = _context.TutorTutoringStudents.Where(x => !students.Select(x => x.Id).Contains(x.TutoringStudentStudentId) && x.TutoringStudentTermId == termId).AsNoTracking();

            // var studentHash = students.Select(x => x.Id).ToHashSet();
            //var tutoringHash = tutorTutoringStudent.Select(x => x.TutoringStudentStudentId).ToHashSet();

            //queryData = queryData
            //    .Where(x => !tutoringHash.Contains(x.Id));

            if (!string.IsNullOrEmpty(query))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    query = $"\"{query}*\"";
                    queryData = queryData.Where(x => EF.Functions.Contains(x.User.FullName, query));
                }
                else
                    queryData = queryData.Where(x => x.User.FullName.ToUpper().Contains(query.ToUpper()) || x.User.UserName.ToUpper().Contains(query.ToUpper()));
            }

            var result = await queryData
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .Take(5).ToListAsync();

            return result
                .OrderBy(x => x.text);

        }

        public async Task<IEnumerable<TutoringStudent>> GetAllAssignedToTutor(string tutorId, string search = null, Guid? termId = null)
        {
            var query = _context.TutoringStudents
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                 .Include(x => x.Student)
                .ThenInclude(x => x.Career)
                .Where(x => x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId));

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Student.User.Name.ToUpper().Contains(search)
                    || x.Student.Career.Name.ToUpper().Contains(search));

            return await query.ToListAsync();

        }

        public async Task<(IEnumerable<TutoringSessionStudent> pagedList, int count)> GetAllBySupportOfficeIdAndPaginationParameter(PaginationParameter paginationParameter, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null)
        {
            var query = _context.TutoringSessionStudents
                .AsQueryable();

            if (supportOfficeId.HasValue)
                query = query.Where(tss => tss.SupportOfficeId == supportOfficeId);

            if (attended.HasValue)
                query = query.Where(tss => tss.SupportOfficeId != null && tss.Attended == attended.Value);
            else
                query = query.Where(tss => tss.SupportOfficeId != null);

            if (careerId.HasValue)
                query = query.Where(x => x.TutoringStudent.Student.CareerId == careerId);

            if (termId.HasValue)
                query = query.Where(x => x.TutoringSession.TermId == termId.Value);

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutoringSession.TutorId == tutorId);

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.TutoringStudent.Student.User.Name.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.User.MaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.User.Dni.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.Campus.Name.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.Career.Name.Contains(paginationParameter.SearchValue) ||
                                    x.TutoringStudent.Student.AdmissionTerm.Name.Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringStudent.Student.User.RawFullName)
                        : query.OrderBy(x => x.TutoringStudent.Student.User.RawFullName);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringStudent.Student.User.Dni)
                        : query.OrderBy(x => x.TutoringStudent.Student.User.Dni);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringStudent.Student.User.UserName)
                        : query.OrderBy(x => x.TutoringStudent.Student.User.UserName);
                    break;
                //case "3":
                //    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                //        ? query.OrderByDescending(x => x.TutoringStudent.Student.Campus.Name)
                //        : query.OrderBy(x => x.TutoringStudent.Student.Campus.Name);
                //    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringStudent.Student.Career.Name)
                        : query.OrderBy(x => x.TutoringStudent.Student.Career.Name);
                    break;
                case "4":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringStudent.Student.AdmissionTerm.Name)
                        : query.OrderBy(x => x.TutoringStudent.Student.AdmissionTerm.Name);
                    break;
                case "5":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringSession.StartTime.ToLocalDateFormat() + x.TutoringSession.StartTime.ToLocalTimeFormat() + x.TutoringSession.EndTime.ToLocalTimeFormat())
                        : query.OrderBy(x => x.TutoringSession.StartTime.ToLocalDateFormat() + x.TutoringSession.StartTime.ToLocalTimeFormat() + x.TutoringSession.EndTime.ToLocalTimeFormat());
                    break;
                case "6":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringSession.Classroom.Description + x.TutoringSession.Classroom.Building.Name + x.TutoringSession.Classroom.Building.Campus.Name)
                        : query.OrderBy(x => x.TutoringSession.Classroom.Description + x.TutoringSession.Classroom.Building.Name + x.TutoringSession.Classroom.Building.Campus.Name);
                    break;
                case "7":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.TutoringSession.Tutor.User.RawFullName)
                        : query.OrderBy(x => x.TutoringSession.Tutor.User.RawFullName);
                    break;
                case "8":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.SendTime)
                        : query.OrderBy(x => x.SendTime);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new TutoringSessionStudent
                {
                    Id = x.Id,
                    TutoringStudentStudentId = x.TutoringStudentStudentId,
                    TutoringSessionId = x.TutoringSessionId,
                    SendTime = x.SendTime,
                    SupportOffice = x.SupportOfficeId.HasValue
                        ? new SupportOffice
                        {
                            Id = x.SupportOffice.Id,
                            Name = x.SupportOffice.Name
                        } : null,
                    TutoringStudent = new TutoringStudent
                    {
                        StudentId = x.TutoringStudent.StudentId,
                        Student = new Student
                        {
                            Status = x.TutoringStudent.Student.Status,
                            Campus = new ENTITIES.Models.Enrollment.Campus
                            {
                                Name = x.TutoringStudent.Student.Campus.Name
                            },
                            Career = new Career
                            {
                                Name = x.TutoringStudent.Student.Career.Name
                            },
                            AdmissionTerm = new ENTITIES.Models.Enrollment.Term
                            {
                                Name = x.TutoringStudent.Student.AdmissionTerm.Name
                            },
                            User = new ApplicationUser
                            {
                                Name = x.TutoringStudent.Student.User.Name,
                                PaternalSurname = x.TutoringStudent.Student.User.PaternalSurname,
                                MaternalSurname = x.TutoringStudent.Student.User.MaternalSurname,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                Picture = x.TutoringStudent.Student.User.Picture,
                                FullName = x.TutoringStudent.Student.User.FullName
                            }
                        }
                    },
                    TutoringSession = new TutoringSession
                    {
                        StartTime = x.TutoringSession.StartTime,
                        EndTime = x.TutoringSession.EndTime,
                        IsDictated = x.TutoringSession.IsDictated,
                        Classroom = new ENTITIES.Models.Enrollment.Classroom
                        {
                            Description = x.TutoringSession.Classroom.Description,
                            Building = new ENTITIES.Models.Enrollment.Building
                            {
                                Name = x.TutoringSession.Classroom.Building.Name,
                                Campus = new ENTITIES.Models.Enrollment.Campus
                                {
                                    Name = x.TutoringSession.Classroom.Building.Campus.Name
                                }
                            }
                        },
                        Tutor = new Tutor
                        {
                            User = new ApplicationUser
                            {
                                Name = x.TutoringSession.Tutor.User.Name,
                                MaternalSurname = x.TutoringSession.Tutor.User.PaternalSurname,
                                PaternalSurname = x.TutoringSession.Tutor.User.MaternalSurname,
                                FullName = x.TutoringSession.Tutor.User.FullName
                            }
                        }
                    }
                }).ToListAsync();

            return (result, count);
        }

        public async Task<int> CountByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null, Guid? termId = null)
        {
            var query = _context.TutoringStudents.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.Student.CareerId == careerId.Value);
            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tss => tss.TutorId == tutorId));

            return await query.CountAsync();
        }

        public async Task<int> SumTimesUpdatedByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null)
        {
            var query = _context.TutoringStudents.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.Student.CareerId == careerId.Value);

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tss => tss.TutorId == tutorId));

            return await query.SumAsync(x => x.TimesUpdated);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsAdminViewDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null)
        {
            Expression<Func<TutoringStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.TutorTutoringStudents.Count != 0 ? x.TutorTutoringStudents.FirstOrDefault().Tutor.User.FullName : "");
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.User.Dni);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Student.AdmissionTerm.Name);
                    break;
                case "7":
                    orderByPredicate = ((x) => x.Student.User.Email);
                    break;
                case "8":
                    orderByPredicate = ((x) => x.Student.User.PhoneNumber); 
                    break;
                default:
                    orderByPredicate = ((x) => x.TutorTutoringStudents.Count != 0 ? x.TutorTutoringStudents.FirstOrDefault().Tutor.User.FullName : "");
                    break;
            }

            var query = _context.TutoringStudents
                   .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(q => q.Student.CareerId == careerId);


            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId));

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.TutorTutoringStudents.FirstOrDefault().Tutor.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();
            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    tutor = x.TutorTutoringStudents.Count != 0 ? x.TutorTutoringStudents.FirstOrDefault().Tutor.User.FullName : "",
                    studentId = x.StudentId,
                    studentPicture = x.Student.User.Picture,
                    studentStatus = x.Student.Status,
                    studentCampus = x.Student.Campus.Name,
                    studentCareer = x.Student.Career.Name,
                    admissionTerm = x.Student.AdmissionTerm.Name,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    x.Student.User.Email,
                    x.Student.User.PhoneNumber,
                    currentAcademicYear = x.Student.CurrentAcademicYear.ToString(),
                    dni = x.Student.User.Dni,
                    disapproved = x.Student.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Try == 2),
                    risk = x.Student.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Try > 2),
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId = null, Guid? termId = null, string searchValue = null, Guid? careerId = null)
        {
            Expression<Func<TutoringStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.User.Document);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Student.AdmissionTerm.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.Student.CurrentAcademicYear);
                    break;
                case "7":
                    orderByPredicate = ((x) => x.Student.User.Email);
                    break;
                case "8":
                    orderByPredicate = ((x) => x.Student.User.PhoneNumber);
                    break;
                default:
                    orderByPredicate = ((x) => x.Student.CurrentAcademicYear);
                    break;
            }

            var query = _context.TutoringStudents
                .AsNoTracking();

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId));

            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(q => q.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    studentId = x.StudentId,
                    picture = x.Student.User.Picture,
                    name = x.Student.User.FullName,
                    document = x.Student.User.Document,
                    username = x.Student.User.UserName,
                    academicYear = x.Student.CurrentAcademicYear,
                    admissionTerm = x.Student.AdmissionTerm.Name,
                    disapproved = x.Student.StudentSections.Any(y => y.Section.CourseTerm.TermId == x.TermId && y.Try == 2),
                    risk = x.Student.StudentSections.Any(y => y.Section.CourseTerm.TermId == x.TermId && y.Try > 2),
                    tutor = x.TutorTutoringStudents.Any() ? x.TutorTutoringStudents.Select(y => y.Tutor.User.FullName).FirstOrDefault() : "--",
                    email = x.Student.User.Email,
                    phoneNumber = x.Student.User.PhoneNumber,
                    career = x.Student.Career.Name
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }


        public async Task<DataTablesStructs.ReturnedData<Student>> GetTutoringStudentsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? careerId = null, string tutorId = null, int? color = null)
        {

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Campus.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.AdmissionTerm.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
            }

            //var query = _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == termId).AsQueryable();
            //var test = await _context.StudentSections.Where(x => x.Section.CourseTerm.TermId == termId).Select(x => x.Student.UserId).ToListAsync();
            var query = _context.Students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsQueryable();

            //var query = _context.StudentSections.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(q => q.CareerId == careerId);



            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Student
                {
                    Id = x.Id,
                    Campus = new ENTITIES.Models.Enrollment.Campus
                    {
                        Name = x.Campus.Name
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name
                    },
                    AdmissionTerm = new ENTITIES.Models.Enrollment.Term
                    {
                        Name = x.AdmissionTerm.Name
                    },

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
                    StudentSections = x.StudentSections
                        .Select(q => new StudentSection
                        {
                            StudentId = q.StudentId,
                            Try = q.Try,
                            Section = new Section
                            {
                                CourseTerm = new CourseTerm
                                {
                                    TermId = q.Section.CourseTerm.TermId
                                }
                            }
                        }).ToList(),
                    AcademicHistories = x.AcademicHistories
                            .Select(ah => new ENTITIES.Models.Intranet.AcademicHistory
                            {
                                Approved = ah.Approved,
                                CourseId = ah.CourseId,
                                Try = ah.Try
                            }).ToList()

                }).ToListAsync();

            var recordsTotal = data.Count;


            //if (color.HasValue && color != 0)
            //{
            //    switch (color)
            //    {
            //        case 1:
            //            data = data.Where(x => x.StudentSections.Any(g => g.Max(ah => ah.Try) == 1)).ToList();
            //            recordsFiltered = data.Count();
            //            recordsTotal = data.Count;
            //            break;
            //        case 2:
            //            data = data.Where(x => x.StudentSections.GroupBy(ah => ah.StudentId).Any(g => g.Max(ah => ah.Try) == 2)).ToList();
            //            recordsFiltered = data.Count();
            //            recordsTotal = data.Count;
            //            break;
            //        case 3:
            //            data = data.Where(x => x.StudentSections.GroupBy(ah => ah.StudentId).Any(g => g.Max(ah => ah.Try) > 2)
            //            || x.Status == ConstantHelpers.Student.States.SANCTIONED
            //            || x.Status == ConstantHelpers.Student.States.OBSERVED).ToList();
            //            recordsFiltered = data.Count();
            //            recordsTotal = data.Count;
            //            break;
            //        default: break;
            //    }
            //}

            return new DataTablesStructs.ReturnedData<Student>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<TutoringStudent>> GetTutoringStudentsPdf(Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null)
        {
            var query = _context.TutoringStudents
                   .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(q => q.Student.CareerId == careerId);
            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId));

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Student.User.Name.ToUpper().Contains(searchValue)
                            || x.Student.User.MaternalSurname.ToUpper().Contains(searchValue)
                            || x.Student.User.PaternalSurname.ToUpper().Contains(searchValue)
                            || x.Student.User.UserName.ToUpper().Contains(searchValue));

            query = query.AsQueryable();

            var data = await query
                .Select(x => new TutoringStudent
                {
                    TutorTutoringStudents = x.TutorTutoringStudents.Where(y => y.TutoringStudentStudentId == x.StudentId).Select(y => new TutorTutoringStudent
                    {
                        Tutor = new Tutor
                        {
                            User = new ApplicationUser
                            {
                                FullName = y.Tutor.User.FullName
                            }
                        }
                    }).ToList(),
                    StudentId = x.StudentId,
                    Student = new Student
                    {
                        Campus = new ENTITIES.Models.Enrollment.Campus
                        {
                            Name = x.Student.Campus.Name
                        },
                        Career = new Career
                        {
                            Name = x.Student.Career.Name
                        },
                        AdmissionTerm = new ENTITIES.Models.Enrollment.Term
                        {
                            Name = x.Student.AdmissionTerm.Name
                        },
                        CurrentAcademicYear = x.Student.CurrentAcademicYear,
                        User = new ApplicationUser
                        {
                            Name = x.Student.User.Name,
                            PaternalSurname = x.Student.User.PaternalSurname,
                            MaternalSurname = x.Student.User.MaternalSurname,
                            Dni = x.Student.User.Dni,
                            UserName = x.Student.User.UserName,
                            Picture = x.Student.User.Picture,
                            FullName = x.Student.User.FullName
                        },
                        AcademicHistories = x.Student.AcademicHistories
                            .Select(ah => new ENTITIES.Models.Intranet.AcademicHistory
                            {
                                Approved = ah.Approved,
                                CourseId = ah.CourseId,
                                Try = ah.Try
                            }).ToList()
                    }
                }).ToListAsync();


            return data;
        }

        public async Task<IEnumerable<TutoringStudent>> GetAllWithInclude()
            => await _context.TutoringStudents.Include(x => x.Student).Include(x => x.TutorTutoringStudents).ThenInclude(x => x.Tutor).ToListAsync();
        public async Task<bool> AnyByUser(string userId)
            => await _context.TutoringStudents.AnyAsync(x => x.Student.UserId == userId);

        public async Task<object> GetTutoringStudentsProgressReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, string search = null, bool? showOnlyDissaproved = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var prevTerm = await _context.Terms
                .Where(x => x.StartDate < term.StartDate && (x.Number == "1" || x.Number == "2"))
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .FirstOrDefaultAsync();

            Expression<Func<TutoringStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.TutorTutoringStudents.Count != 0 ? x.TutorTutoringStudents.FirstOrDefault().Tutor.User.FullName : "");
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;           
                default:
                    break;
            }
         
            var query = _context.TutoringStudents
                .Where(x => x.TermId == termId && x.TutorTutoringStudents.Any())
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(search.ToUpper()) || x.Student.User.FullName.ToUpper().Contains(search.ToUpper()));

            if (showOnlyDissaproved == true)
                query = query.Where(x => x.Student.AcademicHistories.Any(y => y.TermId == term.Id && !y.Approved));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.StudentId,
                    tutor = x.TutorTutoringStudents.Select(y => y.Tutor.User.FullName).FirstOrDefault(),
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    
                    term = term.Id,
                    termName = term.Name,

                    prevTerm = prevTerm == null ? Guid.Empty : prevTerm.Id,
                    prevTermName = prevTerm == null ? "-" : prevTerm.Name,

                    grade = x.Student.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.WeightedAverageGrade).FirstOrDefault(),
                    totalCredits = x.Student.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.TotalCredits).FirstOrDefault(),
                    disapprovedCredits = x.Student.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.TotalCredits - y.ApprovedCredits).FirstOrDefault(),
                    
                    prevGrade = prevTerm == null ? 0.0M : x.Student.AcademicSummaries.Where(y => y.TermId == prevTerm.Id).Select(y => y.WeightedAverageGrade).FirstOrDefault(),
                    prevTotalCredits = prevTerm == null ? 0.0M : x.Student.AcademicSummaries.Where(y => y.TermId == prevTerm.Id).Select(y => y.TotalCredits).FirstOrDefault(),
                    prevDisapprovedCredits = prevTerm == null ? 0.0M : x.Student.AcademicSummaries.Where(y => y.TermId == prevTerm.Id).Select(y => y.TotalCredits - y.ApprovedCredits).FirstOrDefault()
                }).ToListAsync();

            return new
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetAllSelect2(Guid termId, Guid? careerId = null)
        {
            var query = _context.TutoringStudents
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            var result = await query
                .Select(x => new
                {
                    id = x.StudentId,
                    text = $"{x.Student.User.UserName} - {x.Student.User.FullName}"
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetAllSearchStudent(string searchValue, Guid termId, Guid? careerId = null)
        {
            var query = _context.TutoringStudents
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.Student.User.FullName, searchValue) || EF.Functions.Contains(x.Student.User.UserName, searchValue));
                }
                else
                {
                    query = query
                        .Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
                }
            }

            var result = await query
                .OrderBy(x => x.Student.User.UserName)
                .Select(x => new
                {
                    id = x.StudentId,
                    text = $"{x.Student.User.UserName} - {x.Student.User.FullName}"
                })
                .Take(5)
                .ToListAsync();

            return result;
        }

        public async Task<List<TutoringStudentTemplate>> GetTutoringStudentsReportExcel(Guid? termId = null, Guid? careerId = null, string tutorId = null)
        {        
            var query = _context.TutoringStudents
                   .AsNoTracking();


            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(q => q.Student.CareerId == careerId);


            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorTutoringStudents.Any(tt => tt.TutorId == tutorId));

            var recordsFiltered = await query.CountAsync();
            query = query.AsQueryable();

            var data = await query
                .Select(x => new TutoringStudentTemplate
                {
                    Name = x.Student.User.Name,
                    PaternalSurname = x.Student.User.PaternalSurname,
                    MaternalSurname = x.Student.User.MaternalSurname,
                    UserName = x.Student.User.UserName,
                    Dni = x.Student.User.Dni,
                    Career = x.Student.Career.Name,
                    AdmissionTerm = x.Student.AdmissionTerm.Name,
                    Email = x.Student.User.Email,
                    PhoneNumber = x.Student.User.PhoneNumber
                })
                .ToListAsync();

            return data;             
        }
    }
}
