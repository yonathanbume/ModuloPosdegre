using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchSubAreaService : IResearchSubAreaService
    {
        private readonly IResearchSubAreaRepository _researchSubAreaRepository;
        public ResearchSubAreaService(IResearchSubAreaRepository researchSubAreaRepository)
        {
            _researchSubAreaRepository = researchSubAreaRepository;
        }
        public async Task<bool> AnyResearchSubAreaByName(string name, Guid? id)
        {
            return await _researchSubAreaRepository.AnyResearchSubAreaByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _researchSubAreaRepository.Count();
        }
        public async Task<ResearchSubArea> Get(Guid id)
        {
            return await _researchSubAreaRepository.Get(id);
        }
        public async Task<object> GetResearchSubArea(Guid id)
        {
            return await _researchSubAreaRepository.GetResearchSubArea(id);
        }
        public async Task<IEnumerable<ResearchSubArea>> GetAll()
        {
            return await _researchSubAreaRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetResearchSubAreas()
        {
            return await _researchSubAreaRepository.GetResearchSubAreas();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchSubAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _researchSubAreaRepository.GetResearchSubAreasDatatable(sentParameters, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchSubAreaRepository.DeleteById(id);
        }
        public async Task Insert(ResearchSubArea researchSubArea)
        {
            await _researchSubAreaRepository.Insert(researchSubArea);
        }
        public async Task Update(ResearchSubArea researchSubArea)
        {
            await _researchSubAreaRepository.Update(researchSubArea);
        }
    }
}
