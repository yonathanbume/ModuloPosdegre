using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates.CulturalActivity;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface ICulturalActivityService
    {
        Task InsertCulturalActivity(CulturalActivity culturalActivity);
        Task UpdateCulturalActivity(CulturalActivity culturalActivity);
        Task DeleteCulturalActivity(CulturalActivity culturalActivity);
        Task<CulturalActivity> GetCulturalActivityById(Guid id);
        Task<IEnumerable<CulturalActivity>> GetaLCulturalActivities();
        Task<List<CulturalActivityTemplate>> GetEventsSiscoToHome();
        Task<List<CulturalActivityTemplate>> GetEventsSiscoAllToHome();
        Task<CulturalActivityHomeTemplate> GetCulturalActivity(Guid id);
        Task<List<CulturalActivityHomeTemplate>> GetCulturalActivities(int page);
        Task<bool> HasMembers(Guid id);
        Task<object> GetCulturalActivitiesYear();
        Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesByYearDataTable(DataTablesStructs.SentParameters sentParameters, int? year = null);
        Task<object> GetCulturalActivitiesByYearChart(int? year = null);
        Task<bool> ExistCulturalActivityName(string name, Guid? id = null);
        Task<object> GetActivitiesPerCareer();
        Task<object> GetActivityTypesPerCareer();
    }
}
