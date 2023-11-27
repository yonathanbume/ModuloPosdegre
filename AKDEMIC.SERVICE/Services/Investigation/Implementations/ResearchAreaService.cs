using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchAreaService : IResearchAreaService
    {
        private readonly IResearchAreaRepository _researchAreaRepository;
        public ResearchAreaService(IResearchAreaRepository researchAreaRepository)
        {
            _researchAreaRepository = researchAreaRepository;
        }
        public async Task<bool> AnyResearchAreaByName(string name, Guid? id)
        {
            return await _researchAreaRepository.AnyResearchAreaByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _researchAreaRepository.Count();
        }
        public async Task<ResearchArea> Get(Guid id)
        {
            return await _researchAreaRepository.Get(id);
        }
        public async Task<object> GetResearchArea(Guid id)
        {
            return await _researchAreaRepository.GetResearchArea(id);
        }
        public async Task<IEnumerable<ResearchArea>> GetAll()
        {
            return await _researchAreaRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetResearchAreas()
        {
            return await _researchAreaRepository.GetResearchAreas();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _researchAreaRepository.GetResearchAreasDatatable(sentParameters, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchAreaRepository.DeleteById(id);
        }
        public async Task Insert(ResearchArea researchArea)
        {
            await _researchAreaRepository.Insert(researchArea);
        }
        public async Task Update(ResearchArea researchArea)
        {
            await _researchAreaRepository.Update(researchArea);
        }
    }
}
