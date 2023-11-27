using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class ConceptTypeService : IConceptTypeService
    {
        private readonly IConceptTypeRepository _conceptTypeRepository;
        public ConceptTypeService(IConceptTypeRepository conceptTypeRepository)
        {
            _conceptTypeRepository = conceptTypeRepository;
        }
        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _conceptTypeRepository.AnyByCode(code, id);

        public Task Delete(ConceptType conceptType)
            => _conceptTypeRepository.Delete(conceptType);

        public Task<ConceptType> Get(Guid id)
            => _conceptTypeRepository.Get(id);

        public Task<IEnumerable<ConceptType>> GetAll()
            => _conceptTypeRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _conceptTypeRepository.GetAllDatatable(sentParameters, searchValue);

        public Task Insert(ConceptType conceptType)
            => _conceptTypeRepository.Insert(conceptType);

        public Task Update(ConceptType conceptType)
            => _conceptTypeRepository.Update(conceptType);
    }
}
