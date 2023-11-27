using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ActivityProcedureService : IActivityProcedureService
    {
        private readonly IActivityProcedureRepository _activityProcedureRepository;

        public ActivityProcedureService(IActivityProcedureRepository activityProcedureRepository)
        {
            _activityProcedureRepository = activityProcedureRepository;
        }

        public async Task InsertActivityProcedure(ActivityProcedure activityProcedure) =>
            await _activityProcedureRepository.Insert(activityProcedure);

        public async Task UpdateActivityProcedure(ActivityProcedure activityProcedure) =>
            await _activityProcedureRepository.Update(activityProcedure);

        public async Task DeleteActivityProcedure(ActivityProcedure activityProcedure) =>
            await _activityProcedureRepository.Delete(activityProcedure);

        public async Task<ActivityProcedure> GetActivityProcedureById(Guid id) =>
            await _activityProcedureRepository.Get(id);

        public async Task<IEnumerable<ActivityProcedure>> GetAllActivityProcedures() =>
            await _activityProcedureRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetActivityProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _activityProcedureRepository.GetActivityProcedureDatatable(sentParameters, search);
    }
}
