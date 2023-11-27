using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Implementations
{
    public class HardwareRepository:Repository<Hardware> , IHardwareRepository
    {
        public HardwareRepository(AkdemicContext context):base(context) { }

        public async Task<IEnumerable<Hardware>> GetHardwaresByComputer(Guid computerId)
        {
            var query = _context.Hardwares
                .Where(x => x.ComputerId == computerId);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHardwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null)
        {
            Expression<Func<Hardware, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ComputerId); break;
                case "1":
                    orderByPredicate = ((x) => x.Id); break;
                default:
                    orderByPredicate = ((x) => x.ComputerId); break;
            }

            var query = _context.Hardwares
                .Where(x => x.ComputerId == computerId)
                .AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Brand.ToUpper().Contains(searchValue.ToUpper())
                                || x.Model.ToUpper().Contains(searchValue.ToUpper())
                                || x.HardwareType.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Model,
                    x.Brand,
                    //State = ConstantHelpers.CURRENT_STATES.VALUES.ContainsKey(x.State)
                    //        ? ConstantHelpers.CURRENT_STATES.VALUES[x.State]
                    //        : "Desconocido",
                    State = x.State.Name,
                    HardwareType = x.HardwareType.Name,
                    x.Description
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
