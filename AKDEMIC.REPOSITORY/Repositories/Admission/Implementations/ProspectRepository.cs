using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ProspectRepository : Repository<Prospect>, IProspectRepository
    {
        public ProspectRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyInRange(int start, int end)
        {
            return await _context.Prospects.AnyAsync(x => (start >= x.Start && start <= x.End) ||
                                        (end >= x.Start && end <= x.End));
        }
        public async Task<bool> Exists(int folderNum)
        {
            return await _context.Prospects.AnyAsync(x => x.Status==1 && (folderNum >= x.Start  && folderNum <= x.End));
        }
        public async Task<Prospect> GetByNumber(int folderNum)
        {
            return await _context.Prospects.Where(x => x.Status==1 && (folderNum >= x.Start  && folderNum <= x.End)).FirstOrDefaultAsync();
        }
        public async Task<DataTablesStructs.ReturnedData<Prospect>> GetDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.Prospects.AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                        .Skip(sentParameters.PagingFirstRecord)
                        .Take(sentParameters.RecordsPerDraw)
                        .ToListAsync();

            var recordsTotal = data.Count;
            return new DataTablesStructs.ReturnedData<Prospect>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> CanDelete(Guid id)
        {
            var prospect = await _context.Prospects.Where(x => x.Id == id).FirstOrDefaultAsync();
            var list = await _context.Postulants.Select(x => x.AdmissionFolder.Value).ToListAsync();
            for (int i = prospect.Start; i <= prospect.End; i++)
            {
                if(list.Any(x => x == i))
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<List<Prospect>> GetActiveProspects()
        {
            return await _context.Prospects.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE).ToListAsync();
        }
    }
}
