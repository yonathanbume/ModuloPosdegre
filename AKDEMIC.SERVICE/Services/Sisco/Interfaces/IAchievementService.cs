using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface IAchievementService
    {
        Task<DataTablesStructs.ReturnedData<AchievementTemplate>> GetAllAchievementDatatable(DataTablesStructs.SentParameters sentParameters, string headline, byte status);
        Task InsertAchievement(Achievement Achievement);
        Task UpdateAchievement(Achievement Achievement);
        Task DeleteAchievement(Achievement Achievement);
        Task<Achievement> GetAchievementById(Guid id);
        Task<AchievementTemplate> GetAchievementTemplateById(Guid id);
        Task<List<AchievementTemplate>> GetAchievementToHome();
    }
}
