using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class NonTeachingLoadService : INonTeachingLoadService
    {
        private readonly INonTeachingLoadRepository _nonTeachingLoadRepository;

        public NonTeachingLoadService(INonTeachingLoadRepository nonTeachingLoadRepository)
        {
            _nonTeachingLoadRepository = nonTeachingLoadRepository;
        }

        public async Task<bool> AnyByTeachingLoadType(Guid? teachingLoadTypeId, Guid? teachingLoadSubTypeId)
            => await _nonTeachingLoadRepository.AnyByTeachingLoadType(teachingLoadTypeId, teachingLoadSubTypeId);

        public Task Delete(NonTeachingLoad nonTeachingLoad)
            => _nonTeachingLoadRepository.Delete(nonTeachingLoad);

        public Task DeleteById(Guid id)
            => _nonTeachingLoadRepository.DeleteById(id);

        public Task<NonTeachingLoad> Get(Guid id)
            => _nonTeachingLoadRepository.Get(id);

        public Task<IEnumerable<NonTeachingLoad>> GetAll(string userId,int? category = null, int? minHours = null, int? maxHours = null, Guid? teachingLoadTypeId = null)
            => _nonTeachingLoadRepository.GetAll(userId,category, minHours, maxHours, teachingLoadTypeId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadByTeacher(DataTablesStructs.SentParameters parameters, string userId, Guid? termId)
            => await _nonTeachingLoadRepository.GetNonTeachingLoadByTeacher(parameters, userId, termId);

        public Task Insert(NonTeachingLoad nonTeachingLoad)
            => _nonTeachingLoadRepository.Insert(nonTeachingLoad);

        public Task Update(NonTeachingLoad nonTeachingLoad)
            => _nonTeachingLoadRepository.Update(nonTeachingLoad);
    }
}
