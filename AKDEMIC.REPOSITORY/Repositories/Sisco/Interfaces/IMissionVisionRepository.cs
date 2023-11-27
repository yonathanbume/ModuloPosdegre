using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface IMissionVisionRepository : IRepository<MissionVision>
    {
        Task<List<MissionVision>> GetMissionVision();
    }
}
