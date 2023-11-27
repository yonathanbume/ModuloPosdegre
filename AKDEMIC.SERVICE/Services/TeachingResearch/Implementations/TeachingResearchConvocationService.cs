using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingResearch;
using AKDEMIC.REPOSITORY.Repositories.TeachingResearch.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingResearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingResearch.Implementations
{
    public class TeachingResearchConvocationService : ITeachingResearchConvocationService
    {
        private readonly ITeachingResearchConvocationRepository _teachingResearchConvocationRepository;

        public TeachingResearchConvocationService(ITeachingResearchConvocationRepository teachingResearchConvocationRepository)
        {
            _teachingResearchConvocationRepository = teachingResearchConvocationRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _teachingResearchConvocationRepository.AnyByName(name, ignoredId);
        public async Task Delete(Convocation entity)
            => await _teachingResearchConvocationRepository.Delete(entity);

        public async Task<Convocation> Get(Guid id)
            => await _teachingResearchConvocationRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConvocationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _teachingResearchConvocationRepository.GetConvocationsDatatable(parameters, searchValue);

        public async Task Insert(Convocation entity)
            => await _teachingResearchConvocationRepository.Insert(entity);

        public async Task Update(Convocation entity)
            => await _teachingResearchConvocationRepository.Update(entity);
    }
}
