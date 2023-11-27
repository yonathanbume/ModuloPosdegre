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
    public class TeachingLoadSubTypeService : ITeachingLoadSubTypeService
    {
        private readonly ITeachingLoadSubTypeRepository _teachingLoadSubTypeRepository;

        public TeachingLoadSubTypeService(ITeachingLoadSubTypeRepository teachingLoadSubTypeRepository)
        {
            _teachingLoadSubTypeRepository = teachingLoadSubTypeRepository;
        }

        public async Task Delete(TeachingLoadSubType entity)
            => await _teachingLoadSubTypeRepository.Delete(entity);

        public async Task<TeachingLoadSubType> Get(Guid id)
            => await _teachingLoadSubTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadSubTypeDatatable(DataTablesStructs.SentParameters parameters, Guid? teachingLoadTypeId)
            => await _teachingLoadSubTypeRepository.GetTeachingLoadSubTypeDatatable(parameters, teachingLoadTypeId);

        public async Task<object> GetTeachingLoadSubTypeSelect2(Guid? teachingLoadtypeId, bool? enabled = null)
            => await _teachingLoadSubTypeRepository.GetTeachingLoadSubTypeSelect2(teachingLoadtypeId, enabled);

        public async Task Insert(TeachingLoadSubType entity)
            => await _teachingLoadSubTypeRepository.Insert(entity);

        public async Task Update(TeachingLoadSubType entity)
            => await _teachingLoadSubTypeRepository.Update(entity);
    }
}
