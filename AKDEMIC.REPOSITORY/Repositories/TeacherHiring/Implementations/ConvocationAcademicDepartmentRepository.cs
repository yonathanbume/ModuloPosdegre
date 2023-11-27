using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Implementations
{
    public class ConvocationAcademicDepartmentRepository : Repository<ConvocationAcademicDeparment> , IConvocationAcademicDepartmentRepository
    {
        public ConvocationAcademicDepartmentRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search)
        {
            Expression<Func<ConvocationAcademicDeparment, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.AcademicDepartment.Name); break;
                default:
                    orderByPredicate = ((x) => x.AcademicDepartment.Name); break;
            }

            var query = _context.ConvocationAcademicDeparments.Where(x=>x.ConvocationId == convocationId).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.AcademicDepartment.Name.ToLower().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      academicDepartment = x.AcademicDepartment.Name,
                      x.Vacancies
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByAcademicDepartmentId(Guid convocationId, Guid academicDepartmentId, Guid? ignoredId = null)
            => await _context.ConvocationAcademicDeparments.AnyAsync(x => x.ConvocationId == convocationId && x.AcademicDepartmentId == academicDepartmentId && x.Id != ignoredId);
    }
}
