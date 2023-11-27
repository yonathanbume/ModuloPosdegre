using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyPortalGeneralRepository : Repository<TransparencyPortalGeneral>, ITransparencyPortalGeneralRepository
    {
        public TransparencyPortalGeneralRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TransparencyPortalGeneral>> GetByType(int type)
        {
            return await _context.TransparencyPortalGenerals.Where(x => x.Type == type).ToListAsync();
        }

        public async Task<TransparencyPortalGeneral> GetFirstByType(int type)
        {
            var general = await _context.TransparencyPortalGenerals
                .Where(x => x.Type == type)
                .FirstOrDefaultAsync();

            if (general == null)
            {
                general = new TransparencyPortalGeneral
                {
                    Title = type == 1 ? "Misión" : type == 2  ? "Visión" : "",
                    Content = "",
                    Type = type
                };

                await _context.TransparencyPortalGenerals.AddAsync(general);
                await _context.SaveChangesAsync();
            }

            return general;
        }
    }
}
