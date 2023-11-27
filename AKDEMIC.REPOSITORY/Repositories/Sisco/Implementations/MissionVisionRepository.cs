using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class MissionVisionRepository : Repository<MissionVision>, IMissionVisionRepository
    {
        public MissionVisionRepository(AkdemicContext context) : base(context) { }

        public async Task<List<MissionVision>> GetMissionVision()
        {
            var model = await _context.MissionVisions.Skip(0).Take(2)
                .Select(x => new MissionVision
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    UrlImage = x.UrlImage
                }).ToListAsync();

            return model;
        }
    }
}
