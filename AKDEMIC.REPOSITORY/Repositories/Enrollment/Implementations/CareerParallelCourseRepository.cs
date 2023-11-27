using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CareerParallelCourseRepository : Repository<CareerParallelCourse>, ICareerParallelCourseRepository
    {
        public CareerParallelCourseRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> CheckParallelCourseExist(CareerParallelCourse careerParallelCourse)
        {
            return await _context.CareerParallelCourses.AnyAsync(x => x.Id != careerParallelCourse.Id && x.CareerId == careerParallelCourse.CareerId && x.AcademicYear == careerParallelCourse.AcademicYear);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
        {
            Expression<Func<CareerParallelCourse, dynamic>> orderByPredicate = null;

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
                default:
                    break;
            }

            var query = _context.CareerParallelCourses.AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Career.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Career.Code.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    careerId = x.CareerId,
                    career = x.Career.Name,
                    code = x.Career.Code,
                    faculty = x.Career.Faculty.Name,
                    year = x.AcademicYear,
                    quantity = x.Quantity,
                    admin = x.AppliesForAdmin,
                    students = x.AppliesForStudents,
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

        public async Task<CareerParallelCourse> GetByCareeerAndAcademicYear(Guid careerId, byte academicYear)
        {
            return await _context.CareerParallelCourses
                        .Where(x => x.CareerId == careerId && x.AcademicYear == academicYear)
                        .FirstOrDefaultAsync();
        }
    }
}
