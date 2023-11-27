using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchDisciplineService : IResearchDisciplineService
    {
        private readonly IResearchDisciplineRepository _researchDisciplineRepository;
        public ResearchDisciplineService(IResearchDisciplineRepository researchDisciplineRepository)
        {
            _researchDisciplineRepository = researchDisciplineRepository;
        }
        public async Task<bool> AnyResearchDisciplineByName(string name, Guid? id)
        {
            return await _researchDisciplineRepository.AnyResearchDisciplineByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _researchDisciplineRepository.Count();
        }
        public async Task<ResearchDiscipline> Get(Guid id)
        {
            return await _researchDisciplineRepository.Get(id);
        }
        public async Task<object> GetResearchDiscipline(Guid id)
        {
            return await _researchDisciplineRepository.GetResearchDiscipline(id);
        }
        public async Task<IEnumerable<ResearchDiscipline>> GetAll()
        {
            return await _researchDisciplineRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetResearchDisciplines()
        {
            return await _researchDisciplineRepository.GetResearchDisciplines();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchDisciplinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? areaId, Guid? subareaId, string searchValue = null)
        {
            return await _researchDisciplineRepository.GetResearchDisciplinesDatatable(sentParameters, areaId, subareaId, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchDisciplineRepository.DeleteById(id);
        }
        public async Task Insert(ResearchDiscipline researchDiscipline)
        {
            await _researchDisciplineRepository.Insert(researchDiscipline);
        }
        public async Task Update(ResearchDiscipline researchDiscipline)
        {
            await _researchDisciplineRepository.Update(researchDiscipline);
        }
    }
}
