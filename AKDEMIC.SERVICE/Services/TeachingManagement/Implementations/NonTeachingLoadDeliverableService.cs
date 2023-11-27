using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class NonTeachingLoadDeliverableService : INonTeachingLoadDeliverableService
    {
        private readonly INonTeachingLoadDeliverableRepository _nonTeachingLoadDeliverableRepository;

        public NonTeachingLoadDeliverableService(INonTeachingLoadDeliverableRepository nonTeachingLoadDeliverableRepository)
        {
            _nonTeachingLoadDeliverableRepository = nonTeachingLoadDeliverableRepository;
        }

        public async Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId)
            => await _nonTeachingLoadDeliverableRepository.AnyByNonTeachingLoad(nonTeachingLoadId);

        public async Task Delete(NonTeachingLoadDeliverable entity)
            => await _nonTeachingLoadDeliverableRepository.Delete(entity);

        public async Task<NonTeachingLoadDeliverable> Get(Guid id)
            => await _nonTeachingLoadDeliverableRepository.Get(id);

        public async Task<List<NonTeachingLoadDeliverable>> GetNonTeachingLoadDeliverables(Guid nonTeachingLoadId)
            => await _nonTeachingLoadDeliverableRepository.GetNonTeachingLoadDeliverables(nonTeachingLoadId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadDeliverablesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId)
            => await _nonTeachingLoadDeliverableRepository.GetNonTeachingLoadDeliverablesDatatable(parameters, nonTeachingLoadId);

        public async Task Insert(NonTeachingLoadDeliverable entity)
            => await _nonTeachingLoadDeliverableRepository.Insert(entity);

        public async Task Update(NonTeachingLoadDeliverable entity)
            => await _nonTeachingLoadDeliverableRepository.Update(entity);
    }
}
