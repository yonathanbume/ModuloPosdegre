using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class AdministrativeTableRepository: Repository<AdministrativeTable>, IAdministrativeTableRepository
    {
        public AdministrativeTableRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, int type,Guid? id = null)
        {
            return await _context.AdministrativeTables.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Type == type && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAdministrativeTablesDatatable(DataTablesStructs.SentParameters sentParameters, int type, string searchValue = null)
        {
            Expression<Func<AdministrativeTable, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Type);
                    break;
            }

            var query = _context.AdministrativeTables.AsNoTracking();

            if (type != 0)
            {
                query = query.Where(x => x.Type == type);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) 
                                    || x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Type,
                    TypeText = ConstantHelpers.ADMINISTRATIVETABLE_TYPE.VALUES.ContainsKey(x.Type) ?
                        ConstantHelpers.ADMINISTRATIVETABLE_TYPE.VALUES[x.Type] : "",
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

        public async Task<object> GetSelect2(int? type = null)
        {
            var query = _context.AdministrativeTables.AsNoTracking();

            if (type != null)
            {
                query = query.Where(x => x.Type == type.Value);
            }

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return data;
        }
    }
}
