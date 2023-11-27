using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Implementations
{
    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquipmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? dependencyId = null)
        {
            Expression<Func<Equipment, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Brand); break;
                case "1":
                    orderByPredicate = ((x) => x.Id); break;
                default:
                    orderByPredicate = ((x) => x.Brand); break;
            }

            var query = _context.Equipments.AsNoTracking();

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Brand.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Brand,
                    x.Model,
                    x.YearModel,
                    //EquipmentState = ConstantHelpers.CURRENT_STATES.VALUES.ContainsKey(x.EquipmentState)
                    //            ? ConstantHelpers.CURRENT_STATES.VALUES[x.EquipmentState]
                    //            : "Desconocido",
                    State = x.State.Name ?? "-",
                    Dependency = x.Dependency.Name ?? "-",
                    EquipmentType = x.EquipmentType.Name,
                    PurchaseDate = x.PurchaseDate.ToLocalDateFormat()
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

        public async Task<object> GetEquipmentReportChart(Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null)
        {
            var query = _context.Equipments
                            .AsQueryable();

            if (dependencyId != null)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (equipmentTypeId != null)
                query = query.Where(x => x.EquipmentTypeId == equipmentTypeId);

            if (state != null)
                query = query.Where(x => x.StateId == state);

            var data = await _context.Dependencies
                .Select(x => new
                {
                    Dependency = x.Name,
                    Count = query.Where(y => y.DependencyId == x.Id).Count()
                }).ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Dependency).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquipmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null, string searchValue = null)
        {
            var query = _context.Equipments
                .Include(x => x.Dependency)
                .Include(x => x.EquipmentType)
                .Include(x=>x.State)
                .AsNoTracking();

            if (dependencyId != null)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (equipmentTypeId != null)
                query = query.Where(x => x.EquipmentTypeId == equipmentTypeId);

            if (state != null)
                query = query.Where(x => x.StateId == state);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Brand.ToUpper().Contains(searchValue.ToUpper()));
            }
            var queryclient =await query
                .Select(x => new
                {
                    x.StateId,
                    StateName = x.State.Name,
                    x.EquipmentType, 
                    x.Dependency
                })
                .ToListAsync();

            var grouped = queryclient
                .GroupBy(x => new { x.StateId, x.EquipmentType, x.Dependency })
                .Select(x => new
                {
                    Dependency = x.Select(y => y.Dependency.Name ?? "otros").FirstOrDefault(),
                    //EquipmentState = ConstantHelpers.CURRENT_STATES.VALUES.ContainsKey(x.Key.EquipmentState)
                    //            ? ConstantHelpers.CURRENT_STATES.VALUES[x.Key.EquipmentState]
                    //            : "Desconocido",
                    EquipmentState = x.Select(y=>y.StateName).FirstOrDefault(),
                    EquipmentType = x.Select(y => y.EquipmentType.Name).FirstOrDefault(),
                    Count = x.Count()
                });

            int recordsFiltered = await query.CountAsync();

            var data = grouped
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

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
