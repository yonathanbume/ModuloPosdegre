using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class InternalOutputRepository : Repository<InternalOutput>, IInternalOutputRepository
    {
        public InternalOutputRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalOuputsDatatable(DataTablesStructs.SentParameters parameters, string search)
        {
            Expression<Func<InternalOutput, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ToUser.UserName;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.InternalOutputs
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();


            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => (x.Dependency.Name.ToLower().Contains(search.ToLower())));

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    dependency = x.Dependency.Name,
                    toUser = x.ToUser.UserName
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalOutputItemsDatatable(DataTablesStructs.SentParameters parameters, Guid internalOutputId,string search)
        {
            var query = _context.InternalOutputItems
                .Where(x=>x.InternalOutputId == internalOutputId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.CatalogItem.Code.ToLower().Contains(search.ToLower()) || x.CatalogItem.Description.ToLower().Contains(search.ToLower()));

            var data = await query
                //.OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.CatalogItem.Description,
                    x.CatalogItem.Code,
                    x.Quantity
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
