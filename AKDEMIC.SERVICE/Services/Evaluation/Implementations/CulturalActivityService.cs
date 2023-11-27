using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates.CulturalActivity;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class CulturalActivityService : ICulturalActivityService
    {
        private readonly ICulturalActivityRespository _culturalActivityRespository;

        public CulturalActivityService(ICulturalActivityRespository culturalActivityRespository)
        {
            _culturalActivityRespository = culturalActivityRespository;
        }

        public async Task InsertCulturalActivity(CulturalActivity culturalActivity) =>
            await _culturalActivityRespository.Insert(culturalActivity);

        public async Task UpdateCulturalActivity(CulturalActivity culturalActivity) =>
            await _culturalActivityRespository.Update(culturalActivity);

        public async Task DeleteCulturalActivity(CulturalActivity culturalActivity) =>
            await _culturalActivityRespository.Delete(culturalActivity);

        public async Task<CulturalActivity> GetCulturalActivityById(Guid id) =>
            await _culturalActivityRespository.Get(id);

        public async Task<IEnumerable<CulturalActivity>> GetaLCulturalActivities() =>
            await _culturalActivityRespository.GetAll();

        public async Task<List<CulturalActivityTemplate>> GetEventsSiscoToHome() =>
            await _culturalActivityRespository.GetEventsSiscoToHome();

        public async Task<List<CulturalActivityTemplate>> GetEventsSiscoAllToHome() =>
            await _culturalActivityRespository.GetEventsSiscoAllToHome();

        public async Task<CulturalActivityHomeTemplate> GetCulturalActivity(Guid id) =>
            await _culturalActivityRespository.GetCulturalActivity(id);

        public async Task<List<CulturalActivityHomeTemplate>> GetCulturalActivities(int page) =>
            await _culturalActivityRespository.GetCulturalActivities(page);

        public async Task<bool> HasMembers(Guid id) =>
            await _culturalActivityRespository.HasMembers(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesDataTable(DataTablesStructs.SentParameters parameters, string search) =>
            await _culturalActivityRespository.GetCulturalActivitiesDataTable(parameters, search);

        public async Task<bool> ExistCulturalActivityName(string name, Guid? id = null) =>
            await _culturalActivityRespository.ExistCulturalActivityName(name, id);

        public Task<DataTablesStructs.ReturnedData<object>> GetCulturalActivitiesByYearDataTable(DataTablesStructs.SentParameters sentParameters, int? year = null)
            => _culturalActivityRespository.GetCulturalActivitiesByYearDataTable(sentParameters, year);

        public Task<object> GetCulturalActivitiesByYearChart(int? year = null)
            => _culturalActivityRespository.GetCulturalActivitiesByYearChart(year);

        public Task<object> GetCulturalActivitiesYear()
            => _culturalActivityRespository.GetCulturalActivitiesYear();

        public Task<object> GetActivitiesPerCareer()
            => _culturalActivityRespository.GetActivitiesPerCareer();

        public Task<object> GetActivityTypesPerCareer()
            => _culturalActivityRespository.GetActivityTypesPerCareer();
    }
}
