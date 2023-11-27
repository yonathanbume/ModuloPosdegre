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
    public class NonTeachingLoadActivityService : INonTeachingLoadActivityService
    {
        private readonly INonTeachingLoadActivityRepository _nonTeachingLoadActivityRepository;

        public NonTeachingLoadActivityService(INonTeachingLoadActivityRepository nonTeachingLoadActivityRepository)
        {
            _nonTeachingLoadActivityRepository = nonTeachingLoadActivityRepository;
        }

        public async Task Insert(NonTeachingLoadActivity entity)
            => await _nonTeachingLoadActivityRepository.Insert(entity);

        public async Task Delete(NonTeachingLoadActivity entity)
            => await _nonTeachingLoadActivityRepository.Delete(entity);

        public async Task<NonTeachingLoadActivity> Get(Guid id)
            => await _nonTeachingLoadActivityRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadActivitiesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId)
            => await _nonTeachingLoadActivityRepository.GetNonTeachingLoadActivitiesDatatable(parameters, nonTeachingLoadId);

        public async Task Update(NonTeachingLoadActivity entity)
            => await _nonTeachingLoadActivityRepository.Update(entity);

        public async Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId)
            => await _nonTeachingLoadActivityRepository.AnyByNonTeachingLoad(nonTeachingLoadId);
    }
}
