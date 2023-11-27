using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class YearInformationRepository : Repository<YearInformation>, IYearInformationRepository
    {
        public YearInformationRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByYear(int year, Guid? id = null)
        {
            return await _context.YearInformations.AnyAsync(x => x.Year == year && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllYearInformationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<YearInformation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Year);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.YearInformations.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Year.ToString().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Year,
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

        public async Task<string> GetNameByYear(int year)
        {
            string name = await _context.YearInformations.Where(x => x.Year == year).Select(x => x.Name).FirstOrDefaultAsync() ?? "";

            return name;
        }
    }
}
