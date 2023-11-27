using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        public AchievementService(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }
        
        public async Task<DataTablesStructs.ReturnedData<AchievementTemplate>> GetAllAchievementDatatable(DataTablesStructs.SentParameters sentParameters, string headline, byte status) =>
            await _achievementRepository.GetAllAchievementDatatable(sentParameters, headline, status);

        public async Task InsertAchievement(Achievement achievement) =>
            await _achievementRepository.Insert(achievement);


        public async Task UpdateAchievement(Achievement achievement) =>
            await _achievementRepository.Update(achievement);

        public async Task DeleteAchievement(Achievement achievement) =>
            await _achievementRepository.Delete(achievement);
        public async Task<Achievement> GetAchievementById(Guid id) =>
            await _achievementRepository.Get(id);

        public async Task<AchievementTemplate> GetAchievementTemplateById(Guid id) =>
            await _achievementRepository.GetAchievementById(id);

        public async Task<List<AchievementTemplate>> GetAchievementToHome() =>
            await _achievementRepository.GetAchievementToHome();
    }
}
