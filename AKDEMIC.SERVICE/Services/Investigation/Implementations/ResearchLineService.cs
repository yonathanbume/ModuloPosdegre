using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ResearchLineService : IResearchLineService
    {
        private readonly IResearchLineRepository _researchLineRepository;
        public ResearchLineService(IResearchLineRepository researchLineRepository)
        {
            _researchLineRepository = researchLineRepository;
        }
        public async Task<bool> AnyResearchLineByName(string name, Guid? id)
        {
            return await _researchLineRepository.AnyResearchLineByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _researchLineRepository.Count();
        }
        public async Task<ResearchLine> Get(Guid id)
        {
            return await _researchLineRepository.Get(id);
        }
        public async Task<object> GetResearchLine(Guid id)
        {
            return await _researchLineRepository.GetResearchLine(id);
        }
        public async Task<IEnumerable<ResearchLine>> GetAll()
        {
            return await _researchLineRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetResearchLines()
        {
            return await _researchLineRepository.GetResearchLines();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null)
        {
            return await _researchLineRepository.GetResearchLinesDatatable(sentParameters, careerId, categoryId, disciplineId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCompanyResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            return await _researchLineRepository.GetCompanyResearchLinesDatatable(sentParameters, userId);
        }
        public async Task DeleteById(Guid id)
        {
            await _researchLineRepository.DeleteById(id);
        }
        public async Task Insert(ResearchLine researchLine)
        {
            await _researchLineRepository.Insert(researchLine);
        }
        public async Task Update(ResearchLine researchLine)
        {
            await _researchLineRepository.Update(researchLine);
        }
    }
}
