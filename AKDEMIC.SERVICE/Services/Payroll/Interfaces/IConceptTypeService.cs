using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IConceptTypeService
    {
        Task<ConceptType> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<IEnumerable<ConceptType>> GetAll();
        Task Insert(ConceptType conceptType);
        Task Update(ConceptType conceptType);
        Task Delete(ConceptType conceptType);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
