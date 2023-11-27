using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class SupplierCategoryRepository : Repository<SupplierCategory>, ISupplierCategoryRepository
    {
        public SupplierCategoryRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? id = null)
        {
            return await _context.SupplierCategories.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplierCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<SupplierCategory, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.SupplierCategories.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetSuppliersReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? supplierCategoryId = null, string searchValue = null)
        {
            Expression<Func<Supplier, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.RUC); break;
                case "2":
                    orderByPredicate = ((x) => x.Address); break;
                case "3":
                    orderByPredicate = ((x) => x.Email); break;
                case "4":
                    orderByPredicate = ((x) => x.PhoneNumber); break;
                case "5":
                    orderByPredicate = ((x) => x.State); break;
                case "6":
                    orderByPredicate = ((x) => x.SupplierCategory.Name); break;
            }

            var query = _context.Suppliers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string search = searchValue.Trim();
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) ||
                                    x.RUC.ToUpper().Contains(search.ToUpper()) ||
                                    x.Address.ToUpper().Contains(search.ToUpper()) ||
                                    x.Email.ToUpper().Contains(search.ToUpper()) ||
                                    x.PhoneNumber.ToUpper().Contains(search.ToUpper()) ||
                                    x.SupplierCategory.Name.ToUpper().Contains(search.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.RUC,
                    x.Address,
                    x.Email,
                    x.PhoneNumber,
                    state = ConstantHelpers.ECONOMICMANAGEMENT.SUPPLIERCATEGORYSTATES.STATES.ContainsKey(x.State) ?
                        ConstantHelpers.ECONOMICMANAGEMENT.SUPPLIERCATEGORYSTATES.STATES[x.State]: "Estado No Conocido",
                    supplierCategory = x.SupplierCategory.Name
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
