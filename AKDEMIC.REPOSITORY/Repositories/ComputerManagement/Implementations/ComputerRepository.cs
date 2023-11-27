using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Templates.Computer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Implementations
{
    public class ComputerRepository:Repository<Computer> ,IComputerRepository
    {
        public ComputerRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? dependencyId = null,
            string brand = null, int? start_year = null, int? end_year = null, Guid? status = null, string start_purchase = null, 
            string end_purchase = null, string start_createdat = null, string end_createdat = null)
        {
            Expression<Func<Computer, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Brand); break;
                case "1":
                    orderByPredicate = ((x) => x.Id); break;
                default:
                    orderByPredicate = ((x) => x.Brand); break;
            }

            var query = _context.Computers.AsNoTracking();

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(x => x.Brand.ToLower().Trim().Contains(brand.ToLower().Trim()));

            if (start_year.HasValue)
                query = query.Where(x => Convert.ToInt16(x.YearModel) >= start_year);

            if (end_year.HasValue)
                query = query.Where(x => Convert.ToInt16(x.YearModel) <= end_year);

            if (status.HasValue)
                query = query.Where(x => x.StateId == status);

            if(!string.IsNullOrEmpty(start_purchase))
            {
                var startPurchaseDate = ConvertHelpers.DatepickerToDatetime(start_purchase);
                query = query.Where(x => x.PurchaseDate.Date >= startPurchaseDate.Date);
            }

            if (!string.IsNullOrEmpty(end_purchase))
            {
                var endPurchaseDate = ConvertHelpers.DatepickerToDatetime(end_purchase);
                query = query.Where(x => x.PurchaseDate.Date <= endPurchaseDate.Date);
            }

            if (!string.IsNullOrEmpty(start_createdat))
            {
                var createdAt = ConvertHelpers.DatepickerToDatetime(start_createdat);
                query = query.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Date >= createdAt.Date);
            }

            if (!string.IsNullOrEmpty(end_createdat))
            {
                var createdAt = ConvertHelpers.DatepickerToDatetime(start_createdat);
                query = query.Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Date <= createdAt.Date);
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Brand.ToLower().Contains(searchValue) || x.Code.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Brand,
                    x.Model,
                    x.YearModel,
                    //State = ConstantHelpers.CURRENT_STATES.VALUES.ContainsKey()
                    //            ? ConstantHelpers.CURRENT_STATES.VALUES[x.State]
                    //            : "Desconocido",
                    State = x.State.Name ?? "-",//x?.state?.name ??.... a==true?b:""
                    WarrantyExpirationDate = x.WarrantyExpirationDate.ToLocalDateFormat(),
                    ComputerSupplier = x.ComputerSupplier.Name,
                    Dependency = x.Dependency.Name ?? "-",
                    //Type = ConstantHelpers.CURRENT_COMPUTERS_TYPES.VALUES.ContainsKey(x.Type)
                    //            ? ConstantHelpers.CURRENT_COMPUTERS_TYPES.VALUES[x.Type] 
                    //            : "Desconocido",
                    Type = x.Type.Name ?? "-",
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

        public async Task<object> GetComputerReportChart(Guid? dependencyId, Guid? type = null, Guid? state = null)
        {
            var query = _context.Computers
                .AsQueryable();

            if (dependencyId != null)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (type != null)
                query = query.Where(x => x.TypeId == type);

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

        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId, Guid? type = null, Guid? state = null, string searchValue = null)
        {
            var query = _context.Computers
                .Include(x=>x.Dependency)
                .Include(x=>x.State)
                .Include(x=>x.Type)
                .AsNoTracking();

            if (dependencyId != null)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (type != null)
                query = query.Where(x => x.TypeId == type);

            if (state != null)
                query = query.Where(x => x.StateId == state);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Brand.ToUpper().Contains(searchValue.ToUpper()));
            }
            var queryclient = await query.ToListAsync();
            var grouped = queryclient
                .GroupBy(x => new { x.StateId, x.TypeId, x.DependencyId })
                .Select(x => new
                {
                    Dependency = x.Select(y => y.Dependency.Name ?? "otros").FirstOrDefault(),
                    //State = ConstantHelpers.CURRENT_STATES.VALUES.ContainsKey(x.Key.State)
                    //        ? ConstantHelpers.CURRENT_STATES.VALUES[x.Key.State]
                    //        : "Desconocido",
                    State  = x.Select(y=>y.State.Name).FirstOrDefault(),
                    //Type = ConstantHelpers.CURRENT_COMPUTERS_TYPES.VALUES.ContainsKey(x.Key.TypeId)
                    //        ? ConstantHelpers.CURRENT_COMPUTERS_TYPES.VALUES[x.Key.TypeId]
                    //        : "Desconocido",
                    Type = x.Select(y => y.Type.Name).FirstOrDefault(),
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

        public async Task<object> GetReportByDependencyChart()
        {
            var query = await _context.Computers.Include(x=>x.Dependency).ToListAsync();
            var hardwarecount =await _context.Hardwares.Select(y => y.Computer.Dependency.Id).ToListAsync();
            var softwarecount =await _context.Softwares.Select(y => y.Computer.Dependency.Id).ToListAsync();

            var parentResult = query
                .GroupBy(x => x.Dependency)
                .Select(x => new
                {
                    name = x.Key.Name,
                    y = hardwarecount.Where(y=>y == x.Key.Id).Count() + softwarecount.Where(y => y == x.Key.Id).Count(),
                    drilldown = x.Key.Name
                })
                .ToArray();

            var details = query
                .GroupBy(x => x.Dependency)
                .Select(x => new ReportByDependecyTemplate
                {
                    Name = x.Key.Name,
                    Id = x.Key.Name
                })
                .ToArray();

            foreach (var item in details)
            {
                var objectDetails = new List<object[]>();
                var computerByDependecy = query.Where(x => x.Dependency.Name == item.Name);
                var softwares = await _context.Softwares.Where(x => x.Computer.Dependency.Name == item.Name).CountAsync();
                var hardwares = await _context.Hardwares.Where(x => x.Computer.Dependency.Name == item.Name).CountAsync();
                objectDetails.Add(new object[] {"Software", softwares });
                objectDetails.Add(new object[] {"Hardware", hardwares });

                item.Data = objectDetails;
            }

            return new { parentResult, details };
        }
    }
}
