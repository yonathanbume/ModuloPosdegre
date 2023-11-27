using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringAttendanceRepository : Repository<TutoringAttendance>, ITutoringAttendanceRepository
    {
        public TutoringAttendanceRepository(AkdemicContext context) : base(context)
        {
        }

        public async override Task<TutoringAttendance> Get(Guid tutoringAttendanceId)
        => await _context.TutoringAttendances
                .Include(x => x.TutoringStudent.Student.User)
                .FirstOrDefaultAsync(x => x.Id == tutoringAttendanceId);

        public async Task<IEnumerable<TutoringAttendance>> GetAllByTutorId(string tutorId, Guid? tutoringStudentId = null)
        {
            var query = _context.TutoringAttendances
                .Include(x => x.SupportOffice)
                .Include(x => x.TutoringStudent.Student.User)
                .Where(x => x.TutorId == tutorId)
                .AsQueryable();

            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringStudentStudentId == tutoringStudentId);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<TutoringAttendance>> GetTutoringAttendancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, string tutorId = null, Guid? tutoringStudentId = null, Guid? termId = null, Guid? careerId = null)
        {
            Expression<Func<TutoringAttendance, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.FullName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.Dni);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.UserName);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.SupportOffice.Name);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                default:
                    orderByPredicate = ((x) => x.TutoringStudent.Student.User.FullName);

                    break;
            }

            var query = _context.TutoringAttendances
                .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(x => x.TutoringStudent.Student.CareerId == careerId);

            if (supportOfficeId.HasValue)
                query = query.Where(x => x.SupportOfficeId == supportOfficeId);

            if (tutoringStudentId.HasValue)
                query = query.Where(x => x.TutoringStudentStudentId == tutoringStudentId);

            if (!string.IsNullOrEmpty(tutorId))
                query = query.Where(x => x.TutorId == tutorId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.TutoringStudent.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.TutoringStudent.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringAttendance
                {
                    Id = x.Id,
                    TutoringStudentStudentId = x.TutoringStudentStudentId,
                    TutoringStudent = new TutoringStudent
                    {
                        Student = new ENTITIES.Models.Generals.Student
                        {
                            Career = new ENTITIES.Models.Generals.Career
                            {
                                Name = x.TutoringStudent.Student.Career.Name,
                            },
                            CurrentAcademicYear = x.TutoringStudent.Student.CurrentAcademicYear,
                            User = new ENTITIES.Models.Generals.ApplicationUser
                            {
                                Name = x.TutoringStudent.Student.User.Name,
                                PaternalSurname = x.TutoringStudent.Student.User.PaternalSurname,
                                MaternalSurname = x.TutoringStudent.Student.User.MaternalSurname,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                FullName = x.TutoringStudent.Student.User.FullName
                            }
                        }
                    },
                    CreatedAt = x.CreatedAt,
                    SupportOffice = new SupportOffice
                    {
                        Name = x.SupportOffice.Name
                    }
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringAttendance>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<TutoringAttendance>> GetTutoringAttendances(Guid supportOfficeId, Guid? termId = null, Guid? careerId = null, string searchValue = null)
        {
            var query = _context.TutoringAttendances.Where(x => x.SupportOfficeId == supportOfficeId)
                .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(x => x.TutoringStudent.Student.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.TutoringStudent.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.TutoringStudent.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));


            query = query.AsQueryable();

            var data = await query
                .Select(x => new TutoringAttendance
                {
                    Id = x.Id,
                    TutoringStudentStudentId = x.TutoringStudentStudentId,
                    TutoringStudent = new TutoringStudent
                    {
                        Student = new ENTITIES.Models.Generals.Student
                        {
                            Career = new ENTITIES.Models.Generals.Career
                            {
                                Name = x.TutoringStudent.Student.Career.Name,
                            },
                            CurrentAcademicYear = x.TutoringStudent.Student.CurrentAcademicYear,
                            User = new ENTITIES.Models.Generals.ApplicationUser
                            {
                                Name = x.TutoringStudent.Student.User.Name,
                                PaternalSurname = x.TutoringStudent.Student.User.PaternalSurname,
                                MaternalSurname = x.TutoringStudent.Student.User.MaternalSurname,
                                Dni = x.TutoringStudent.Student.User.Dni,
                                UserName = x.TutoringStudent.Student.User.UserName,
                                FullName = x.TutoringStudent.Student.User.FullName
                            }
                        }
                    },
                    CreatedAt = x.CreatedAt,
                    SupportOffice = new SupportOffice
                    {
                        Name = x.SupportOffice.Name
                    }
                })
                .ToListAsync();

            return data;
        }
        public async Task<TutoringAttendance> GetAllByStudenIdAndSupportOfficeId(Guid tutoringStudentId, Guid supportOfficeId)
            => await _context.TutoringAttendances.Where(x => x.TutoringStudentStudentId == tutoringStudentId && x.SupportOfficeId == supportOfficeId).FirstOrDefaultAsync();
        public async Task<TutoringAttendance> GetWithData(Guid id)
            => await _context.TutoringAttendances.Include(x => x.TutoringStudent).ThenInclude(x => x.Student).ThenInclude(x => x.User).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<object> GetInformation(Guid id)
        {
            var result = await _context.TutoringAttendances
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    Id = x.Id,
                    Tutor = x.Tutor.User.FullName,
                    TutorId = x.TutorId,
                    StudentId = x.TutoringStudent.StudentId,                    
                    Student = $"{x.TutoringStudent.Student.User.UserName} - {x.TutoringStudent.Student.User.FullName}",
                    TermId = x.TutoringStudent.TermId,
                    Term = x.TutoringStudent.Term.Name,
                    Observation = x.Observation
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
