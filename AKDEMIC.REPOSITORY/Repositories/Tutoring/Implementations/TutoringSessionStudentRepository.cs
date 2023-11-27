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
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringSessionStudentRepository : Repository<TutoringSessionStudent>, ITutoringSessionStudentRepository
    {
        public TutoringSessionStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTutorIdAndTutoringStudentId(Guid tutoringStudentId, string tutorId, bool? absent = null, Guid? termId = null)
        {
            var query = _context.TutoringSessionStudents
                .Where(x => x.TutoringStudentStudentId == tutoringStudentId && x.TutoringSession.TutorId == tutorId && x.TutoringSession.TermId == termId)
                .AsQueryable();

            if (absent.HasValue)
                query = query.Where(x => x.Absent == absent.Value);

            return await query.AnyAsync();
        }

        public override async Task<TutoringSessionStudent> Get(Guid tutoringSessionStudentId)
            => await _context.TutoringSessionStudents
                .Include(x => x.TutoringSession.Tutor.User)
                .Include(x => x.TutoringSession.Classroom.Building.Campus)
                .Include(x => x.TutoringStudent.Student.User)
                .Where(x => x.Id == tutoringSessionStudentId)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringSessionId(Guid tutoringSessionId)
            => await _context.TutoringSessionStudents
                .Include(x => x.TutoringStudent)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.User)
                .Where(x => x.TutoringSessionId == tutoringSessionId)
                .ToListAsync();

        public async Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringStudentId(Guid tutoringStudentId)
            => await _context.TutoringSessionStudents
                .Include(x => x.SupportOffice)
                .Include(x => x.TutoringStudent.Student.User)
                .Include(x => x.TutoringSession.Tutor.User)
                .Include(x => x.TutoringSession.Classroom.Building.Campus)
                .Include(x => x.TutoringSession.TutoringSessionProblems)
                    .ThenInclude(x => x.TutoringProblem)
                .Where(x => x.TutoringStudentStudentId == tutoringStudentId)
                .ToListAsync();

        public async Task<TutoringSessionStudent> GetByTutoringSessionIdAndTutoringStudentId(Guid tutoringSessionId, Guid tutoringStudentId)
            => await _context.TutoringSessionStudents
                .Include(x => x.SupportOffice)
                .Include(x => x.TutoringStudent.Student.User)
                .Include(x => x.TutoringStudent.Student.Campus)
                .Include(x => x.TutoringStudent.Student.Career)
                .Include(x => x.TutoringStudent.Student.AdmissionTerm)
                .Include(x => x.TutoringSession.Tutor.User)
                .Include(x => x.TutoringSession.Classroom.Building.Campus)
                .Where(x => x.TutoringSessionId == tutoringSessionId && x.TutoringStudentStudentId == tutoringStudentId)
                .FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null)
        {

            Expression<Func<TutoringSessionStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.UserName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.Campus.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.Career.Name);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.TutoringSession.StartTime);

                    break;
                case "7":
                    orderByPredicate = ((x) => x.SendTime);

                    break;
                case "8":
                    orderByPredicate = ((x) => x.TutoringSession.Tutor.User.FullName);
                    break;
                default:
                    orderByPredicate = ((x) => x.SendTime);

                    break;
            }
            var query = _context.TutoringSessionStudents
                   .AsNoTracking();


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


            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.TutoringStudent.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.TutoringStudent.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                            || x.TutoringStudent.Student.User.Dni.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
                            CurrentAcademicYear = x.TutoringStudent.Student.CurrentAcademicYear,
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
                                FullName = x.TutoringStudent.Student.User.FullName,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                Picture = x.TutoringStudent.Student.User.Picture,
                                PhoneNumber = x.TutoringStudent.Student.User.PhoneNumber
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
                            Type = x.TutoringSession.Tutor.Type,
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

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringSessionStudent>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<TutoringSessionStudent>> GetTutoringSessionStudents(Guid supportOfficeId, string search = null, Guid? careerId = null, Guid? termId = null)
        {
            var query = _context.TutoringSessionStudents.Where(tss => tss.SupportOfficeId == supportOfficeId && tss.SupportOfficeId != null && tss.Attended == true)
                   .AsNoTracking();

            if (careerId.HasValue)
                query = query.Where(x => x.TutoringStudent.Student.CareerId == careerId);

            if (termId.HasValue)
                query = query.Where(x => x.TutoringSession.TermId == termId.Value);


            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.TutoringStudent.Student.User.FullName.ToUpper().Contains(search.ToUpper())
                            || x.TutoringStudent.Student.User.UserName.ToUpper().Contains(search.ToUpper())
                            || x.TutoringStudent.Student.User.Dni.ToUpper().Contains(search.ToUpper()));

            query = query.AsQueryable();

            var data = await query
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
                            CurrentAcademicYear = x.TutoringStudent.Student.CurrentAcademicYear,
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
                                FullName = x.TutoringStudent.Student.User.FullName,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                Picture = x.TutoringStudent.Student.User.Picture,
                                PhoneNumber = x.TutoringStudent.Student.User.PhoneNumber
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
                            Type = x.TutoringSession.Tutor.Type,
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

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetHistoryTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null)
        {
            Expression<Func<TutoringSessionStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.UserName);

                    break;
                //case "5":
                //    orderByPredicate = ((x) => x.TutoringStudent.Student.Campus.Name);

                //    break;
                case "5":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.Career.Name);

                    break;
                //case "6":
                //    orderByPredicate = ((x) => x.TutoringStudent.Student.AdmissionTerm.Name);

                //    break;
                case "6":
                    orderByPredicate = ((x) => new { x.TutoringSession.StartTime.Date, x.TutoringSession.StartTime.TimeOfDay, et = x.TutoringSession.EndTime.TimeOfDay });

                    break;
                //case "7":
                //    orderByPredicate = ((x) => new { x.TutoringSession.Classroom.Description, x.TutoringSession.Classroom.Building.Name, x.TutoringSession.Classroom.Building.Campus.Name });

                //    break;
                case "7":
                    orderByPredicate = ((x) => x.TutoringSession.Tutor.User.FullName);

                    break;
                case "8":
                    orderByPredicate = ((x) => x.SendTime);

                    break;
                default:
                    //orderByPredicate = ((x) => x.TutoringStudent.Student.User.FullName);

                    break;
            }
            var query = _context.TutoringSessionStudents
                   .AsNoTracking();


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

            //if (!string.IsNullOrEmpty(tutorId))
            //    query = query.Where(x => x.TutoringSession.TutorId == tutorId);


            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.TutoringStudent.Student.User.Name.ToUpper().Contains(searchValue)
                            || x.TutoringStudent.Student.User.MaternalSurname.ToUpper().Contains(searchValue)
                            || x.TutoringStudent.Student.User.PaternalSurname.ToUpper().Contains(searchValue)
                            || x.TutoringStudent.Student.User.UserName.ToUpper().Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
                            CurrentAcademicYear = x.TutoringStudent.Student.CurrentAcademicYear,
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
                                FullName = x.TutoringStudent.Student.User.FullName,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                Picture = x.TutoringStudent.Student.User.Picture,
                                PhoneNumber = x.TutoringStudent.Student.User.PhoneNumber
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

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringSessionStudent>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<TutoringSessionStudent>> GetWithData(Guid tutoringSessionId, Guid tutoringStudentId)
        {
            var query = _context.TutoringSessionStudents.Where(x => x.TutoringSessionId == tutoringSessionId && x.TutoringStudentStudentId == tutoringStudentId).AsQueryable();

            var data = await query
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
                        Observations = x.TutoringSession.Observations,
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

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringSessionStudentsDatatableByStudent(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null, string search = null)
        {
            Expression<Func<TutoringSessionStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.TutoringStudent.Term.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.TutoringSession.StartTime;
                    break;
                case "2":
                    orderByPredicate = (x) => x.TutoringSession.Classroom.Building.Campus.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.TutoringSession.Classroom.Building.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.TutoringSession.Classroom.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.TutoringSession.Tutor.User.FullName;
                    break;
                default:
                    orderByPredicate = (x) => x.TutoringStudent.Term.Name;
                    break;
            }

            var query = _context.TutoringSessionStudents.Where(x => x.TutoringStudentStudentId == studentId && x.TutoringSession.EndTime < DateTime.UtcNow)
                   .AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.TutoringStudentTermId == termId);


            if (!string.IsNullOrEmpty(search))
            {
                string searchTrim = search.Trim().ToUpper();
                query = query
                    .Where(x => x.TutoringSession.Tutor.User.Name.ToUpper().Contains(searchTrim)
                            || x.TutoringSession.Tutor.User.MaternalSurname.ToUpper().Contains(searchTrim)
                            || x.TutoringSession.Tutor.User.PaternalSurname.ToUpper().Contains(searchTrim)
                            || x.TutoringSession.Tutor.User.FullName.ToUpper().Contains(searchTrim)
                            || x.TutoringSession.Tutor.User.UserName.ToUpper().Contains(searchTrim));
            }


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    termId = x.TutoringStudentTermId,
                    termName = x.TutoringStudent.Term.Name,
                    tutoringStudentId = x.TutoringStudentStudentId,
                    tutoringSessionId = x.TutoringSessionId,
                    sede = x.TutoringSession.Classroom.Building.Campus.Name,
                    pabellon = x.TutoringSession.Classroom.Building.Name,
                    aula = x.TutoringSession.Classroom.Description,
                    typeAula = x.TutoringSession.Classroom.ClassroomType.Name,
                    duration = x.TutoringSession.EndTime.Subtract(x.TutoringSession.StartTime).TotalHours,
                    session = $"{x.TutoringSession.StartTime.ToLocalDateFormat()} {x.TutoringSession.StartTime.ToLocalTimeFormat()} - {x.TutoringSession.EndTime.ToLocalTimeFormat()}",
                    tutor = x.TutoringSession.Tutor.User.FullName,
                    datetime = x.SendTime.ToLocalDateTimeFormat() ?? "---",
                    status = x.TutoringSession.IsDictated ? "Cumplió" : "No cumplió"
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

        public async Task<IEnumerable<TutoringSessionStudent>> GetAllWithInclude()
            => await _context.TutoringSessionStudents.Include(x => x.TutoringSession).ThenInclude(x => x.Tutor).ToListAsync();


        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
        {
            Expression<Func<TutoringSessionStudent, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.TutoringSession.StartTime); break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringSession.StartTime.TimeOfDay); break;
                case "2":
                    orderByPredicate = ((x) => x.Absent); break;
                case "3":
                    orderByPredicate = ((x) => x.SupportOffice.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.SendTime); break;

                default:
                    orderByPredicate = ((x) => x.TutoringSession.StartTime);
                    break;
            }

            var query = _context.TutoringSessionStudents
                .Where(x => x.TutoringStudent.StudentId == studentId && x.TutoringStudent.TermId == termId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    //id = x.Id,
                    //tutoringStudentId = x.TutoringStudentStudentId,
                    //tutoringSessionId = x.TutoringSessionId,
                    //sede = x.TutoringSession.Classroom.Building.Campus.Name,
                    //pabellon = x.TutoringSession.Classroom.Building.Name,
                    //aula = x.TutoringSession.Classroom.Description,
                    //typeAula = x.TutoringSession.Classroom.ClassroomType.Name,
                    date = x.TutoringSession.StartTime.ToLocalDateFormat(),
                    session = $"{x.TutoringSession.StartTime.ToLocalTimeFormat()} - {x.TutoringSession.EndTime.ToLocalTimeFormat()}",
                    duration = x.TutoringSession.EndTime.Subtract(x.TutoringSession.StartTime).TotalHours,

                    tutor = x.TutoringSession.Tutor.User.FullName,

                    attended = x.Absent ? "NO" : "SI",

                    referredAt = x.SendTime.ToLocalDateTimeFormat() ?? "--",
                    referredTo = x.SupportOfficeId.HasValue ? x.SupportOffice.Name : "--",
                    
                    type = x.TutoringSession.TutoringSessionStudents.Count > 1 ? "Grupal" : "Individual",

                    observations = x.TutoringSession.Observations

                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
