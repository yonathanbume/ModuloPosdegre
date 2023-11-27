using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class PrivateManagementPensionFundRepository:Repository<PrivateManagementPensionFund>, IPrivateManagementPensionFundRepository
    {
        public PrivateManagementPensionFundRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? id = null)
        {
            return await _context.PrivateManagementPensionFunds.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PrivateManagementPensionFund, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Contribution);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Insurance);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Commission);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.InsurableAmount);
                    break;
            }

            var query = _context.PrivateManagementPensionFunds.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Contribution,
                    x.Insurance,
                    x.Commission,
                    x.InsurableAmount
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetPayrollPrivateManagementPensionDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conceptTypeId = null,string searchValue = null)
        {
            Expression<Func<PrivateManagementPensionFund, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ConceptType.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Contribution);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Insurance);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Commission);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.InsurableAmount);
                    break;
            }

            var query = _context.PrivateManagementPensionFunds.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            if(conceptTypeId != null)
            {
                query = query.Where(x => x.ConceptTypeId == conceptTypeId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Contribution,
                    x.Insurance,
                    x.Commission,
                    x.InsurableAmount,
                    ConceptTypeName = x.ConceptTypeId == null ? "-" : x.ConceptType.Name
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
