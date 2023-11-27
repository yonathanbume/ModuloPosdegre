using AKDEMIC.ENTITIES.Models.Sisco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface IMissionVisionService
    {
        Task InsertMissionVision(MissionVision missionVision);
        Task UpdateMissionVision(MissionVision missionVision);
        Task DeleteMissionVision(MissionVision missionVision);
        Task<MissionVision> GetMissionVisionById(Guid id);
        Task<List<MissionVision>> GetMissionVision();
    }
}
