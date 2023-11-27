using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class MissionVisionService : IMissionVisionService
    {
        private readonly IMissionVisionRepository _missionVisionRepository;

        public MissionVisionService(IMissionVisionRepository missionVisionRepository)
        {
            _missionVisionRepository = missionVisionRepository;
        }

        public async Task InsertMissionVision(MissionVision missionVision) =>
            await _missionVisionRepository.Insert(missionVision);

        public async Task UpdateMissionVision(MissionVision missionVision) =>
            await _missionVisionRepository.Update(missionVision);

        public async Task DeleteMissionVision(MissionVision missionVision) =>
            await _missionVisionRepository.Delete(missionVision);

        public async Task<MissionVision> GetMissionVisionById(Guid id) =>
            await _missionVisionRepository.Get(id);

        public async Task<List<MissionVision>> GetMissionVision() =>
            await _missionVisionRepository.GetMissionVision();
    }
}
