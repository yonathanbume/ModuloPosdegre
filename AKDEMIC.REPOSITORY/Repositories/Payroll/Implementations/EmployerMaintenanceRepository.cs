using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class EmployerMaintenanceRepository : Repository<EmployerMaintenance>, IEmployerMaintenanceRepository
    {
        public EmployerMaintenanceRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.EmployerMaintenances.AnyAsync(x => x.Code == code && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEmployerMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<EmployerMaintenance, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Percentage;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ConceptType.Name;
                    break;

            }

            var query = _context.EmployerMaintenances.AsNoTracking();


            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.Trim().ToUpper();
                query = query.Where(x => x.Description.ToUpper().Contains(searchTrim)
                                    || x.Code.ToUpper().Contains(searchTrim));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Description,
                    x.Percentage,
                    ConceptTypeName = x.ConceptType.Name,
                    x.Pdt,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
