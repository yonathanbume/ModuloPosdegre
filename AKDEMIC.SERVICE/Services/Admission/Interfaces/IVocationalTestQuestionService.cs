using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVocationalTestQuestionService
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestQuestionDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestId, string searchValue = null);
        Task Insert(VocationalTestQuestion vocationalTestQuestion);
        Task<VocationalTestQuestion> Get(Guid vocacionalTestQuestionId);
        Task Update(VocationalTestQuestion vocationalTestQuestion);
        Task DeleteById(Guid id);
        Task<List<VocationalTestQuestion>> GetActiveVocationalTestQuestions();
    }
}
