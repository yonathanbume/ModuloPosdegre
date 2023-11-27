using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class EmailManagementRepository : Repository<EmailManagement> , IEmailManagementRepository
    {
        public EmailManagementRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyBySystem(int system, Guid? ignoredId = null)
            => await _context.EmailManagements.AnyAsync(x => x.System == system && x.Id != ignoredId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEmailManagementDatatable(DataTablesStructs.SentParameters parameters, int? system)
        {
            var query = _context.EmailManagements.AsQueryable();

            if (system.HasValue)
                query = query.Where(x => x.System == system);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new 
                {
                    x.Id,
                    x.Email,
                    system = ConstantHelpers.Solution.Values[x.System]
                })
                .ToListAsync();

            int recordsTotal = data.Count;

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
