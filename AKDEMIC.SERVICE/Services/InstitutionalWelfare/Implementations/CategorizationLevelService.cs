using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class CategorizationLevelService : ICategorizationLevelService
    {
        private readonly ICategorizationLevelRepository _studentRubricItemRepository;
        public CategorizationLevelService(ICategorizationLevelRepository studentRubricItemRepository)
        {
            _studentRubricItemRepository = studentRubricItemRepository;
        }

        public async Task Delete(CategorizationLevel studentRubricItem)
        {
            await _studentRubricItemRepository.Delete(studentRubricItem);
        }

        public async Task<CategorizationLevel> FirstElementWithMax(Guid categorizationLevelHeaderId, Guid? avoidId = null)
        {
            return await _studentRubricItemRepository.FirstElementWithMax(categorizationLevelHeaderId, avoidId);
        }

        public async Task<CategorizationLevel> Get(Guid id)
        {
            return await _studentRubricItemRepository.Get(id);
        }

        public async Task<IEnumerable<CategorizationLevel>> GetAll()
        {
            return await _studentRubricItemRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCategorizationLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid categorizationLevelHeaderId, string searchValue = null)
        {
            return await _studentRubricItemRepository.GetCategorizationLevelDatatable(sentParameters, categorizationLevelHeaderId,searchValue);
        }

        public async Task Insert(CategorizationLevel studentRubricItem)
        {
            await _studentRubricItemRepository.Insert(studentRubricItem);
        }

        public async Task Update(CategorizationLevel studentRubricItem)
        {
            await _studentRubricItemRepository.Update(studentRubricItem);
        }

        public async Task<bool> ValidateValues(Guid categorizationLevelHeaderId, int min, int max, Guid? categorizationLevelId)
        {
            return await _studentRubricItemRepository.ValidateValues(categorizationLevelHeaderId, min, max, categorizationLevelId);
        }
    }
}
