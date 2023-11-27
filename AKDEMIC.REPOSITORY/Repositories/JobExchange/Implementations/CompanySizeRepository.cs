﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class CompanySizeRepository : Repository<CompanySize>, ICompanySizeRepository
    {
        public CompanySizeRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompanySizeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.CompanySizes                
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Contains(searchValue));
            }

            Expression<Func<CompanySize, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                name = x.Name

            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetCompanySizeSelect2()
        {
            var result = _context.CompanySizes.Select(x => new
            {
                id = x.Id,
                text = x.Name    
            });
            return await result.ToListAsync();
        }
    }
}
