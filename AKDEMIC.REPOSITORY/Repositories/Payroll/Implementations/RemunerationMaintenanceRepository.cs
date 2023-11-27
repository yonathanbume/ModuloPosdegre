using AKDEMIC.CORE.Extensions;
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
    public class RemunerationMaintenanceRepository : Repository<RemunerationMaintenance>, IRemunerationMaintenanceRepository
    {
        public RemunerationMaintenanceRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllRemunerationMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<RemunerationMaintenance, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.RemunerationCode);
                    break;
                case "1":
                    orderByPredicate = (x) => x.RemunerationDescription;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Import;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Pdt;
                    break;
                case "4":
                    orderByPredicate = (x) => x.ServerType.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.ConceptType.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ConceptCode;
                    break;
            }

            var query = _context.RemunerationMaintenances.AsNoTracking();


            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.Trim().ToUpper();
                query = query.Where(x => x.RemunerationDescription.ToUpper().Contains(searchTrim)
                                    || x.RemunerationCode.ToUpper().Contains(searchTrim));
            }



            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.RemunerationCode,
                    x.RemunerationDescription,
                    x.Import,
                    x.Pdt,
                    ServerTypeName = x.ServerType.Name,
                    ConceptTypeName = x.ConceptType.Name,
                    x.ConceptCode,
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
