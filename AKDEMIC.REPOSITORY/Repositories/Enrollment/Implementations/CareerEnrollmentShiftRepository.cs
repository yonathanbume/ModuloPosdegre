using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.REPOSITORY.Extensions;
using System.Security.Claims;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using System.Linq.Expressions;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CareerEnrollmentShiftRepository : Repository<CareerEnrollmentShift>, ICareerEnrollmentShiftRepository
    {
        public CareerEnrollmentShiftRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CareerEnrollmentShift>> GetAllByEnrollmentShift(Guid enrollmentShiftId)
        {
            return await _context.CareerEnrollmentShifts.Where(x => x.EnrollmentShiftId == enrollmentShiftId).ToListAsync();
        }

        public async Task<object> GetDataDatatableClientSide(Guid enrollmentShiftId, ClaimsPrincipal user = null)
        {
            var onlyRegulars = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_TURN_ONLY_FOR_REGULAR));

            var queryStudents = _context.Students
                .Where(x => !x.GraduationTermId.HasValue)
                .AsNoTracking();

            if (onlyRegulars)
            {
                queryStudents = queryStudents
                    .Where(x => x.Status == ConstantHelpers.Student.States.REGULAR ||
                            x.Status == ConstantHelpers.Student.States.UNBEATEN);
            }
            else
                queryStudents = queryStudents.FilterActiveStudents();

            var students = await queryStudents.Select(x => x.CareerId).ToArrayAsync();

            var careerShifts = await _context.CareerEnrollmentShifts
               .Where(x => x.EnrollmentShiftId == enrollmentShiftId)
               .Select(x => new
               {
                   x.CareerId,
                   StartTime = x.StartDateTime.ToLocalDateTimeFormat(),//.ToString(ConstantHelpers.FORMATS.DATETIME, new System.Globalization.CultureInfo("en-US")),
                   EndTime = x.EndDateTime.ToLocalDateTimeFormat()//.ToString(ConstantHelpers.FORMATS.DATETIME, new System.Globalization.CultureInfo("en-US"))
               }).ToListAsync();

            var query = _context.Careers.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId);
            }

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    career = x.Name,
                    faculty = x.Faculty.Name,
                    students = students.Where(y => y == x.Id).Count(),
                    startDatetime = careerShifts.Any(y => y.CareerId == x.Id) ? careerShifts.Where(y => y.CareerId == x.Id).Select(y => y.StartTime).FirstOrDefault() : null,
                    endDatetime = careerShifts.Any(y => y.CareerId == x.Id) ? careerShifts.Where(y => y.CareerId == x.Id).Select(y => y.EndTime).FirstOrDefault() : null,
                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerShiftDatatable(DataTablesStructs.SentParameters sentParameters, Guid enrollmentShiftId, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<CareerEnrollmentShift, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.WasExecuted;
                    break;
                default:
                    break;
            }

            var query = _context.CareerEnrollmentShifts
                .Where(x => x.EnrollmentShiftId == enrollmentShiftId)
                .AsNoTracking();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(x => x.Career.AcademicCoordinatorId == userId || x.Career.CareerDirectorId == userId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Career.Code.ToUpper().Contains(search.ToUpper()) || x.Career.Name.ToUpper().Contains(search.ToUpper()) || x.Career.Faculty.Name.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Career.Code,
                    careerId = x.CareerId,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    //students = students.Where(y => y == x.Id).Count(),
                    startDatetime = x.StartDateTime.ToLocalDateTimeFormat(),
                    endDatetime = x.EndDateTime.ToLocalDateTimeFormat(),
                    wasExecuted = x.WasExecuted
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

        public async Task<CareerEnrollmentShift> Get(Guid termId, Guid careerId)
        {
            var careerShift = await _context.CareerEnrollmentShifts.FirstOrDefaultAsync(x => x.EnrollmentShift.TermId == termId && x.CareerId == careerId);
            return careerShift;
        }
    }
}
