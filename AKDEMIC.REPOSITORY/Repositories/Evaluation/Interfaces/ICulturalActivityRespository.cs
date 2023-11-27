using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates.CulturalActivity;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface ICulturalActivityRespository : IRepository<CulturalActivity>
    {
        Task<List<CulturalActivityTemplate>> GetEventsSiscoToHome();
        Task<List<CulturalActivityTemplate>> GetEventsSiscoAllToHome();
        Task<object> GetCulturalActivitiesYear();
        Task<bool> HasMembers(Guid id);
        Task<CulturalActivityHomeTemplate> GetCulturalActivity(Guid id);
        Task<List<CulturalActivityHomeTemplate>> GetCulturalActivities(int page);
        Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesDataTable(DataTablesStructs.SentParameters parameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesByYearDataTable(DataTablesStructs.SentParameters sentParameters, int? year = null);
        Task<object> GetCulturalActivitiesByYearChart(int? year = null);
        Task<bool> ExistCulturalActivityName(string name, Guid? id);
        Task<object> GetActivitiesPerCareer();
        Task<object> GetActivityTypesPerCareer();
    }
}
