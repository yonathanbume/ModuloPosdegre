using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<DataTablesStructs.ReturnedData<AchievementTemplate>> GetAllAchievementDatatable(DataTablesStructs.SentParameters sentParameters, string headline = null, byte? status = null);
        Task<AchievementTemplate> GetAchievementById(Guid id);
        Task<List<AchievementTemplate>> GetAchievementToHome();
    }
}
