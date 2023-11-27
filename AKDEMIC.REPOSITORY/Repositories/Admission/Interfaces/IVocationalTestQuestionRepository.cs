using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IVocationalTestQuestionRepository : IRepository<VocationalTestQuestion>
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestQuestionDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestId, string searchValue = null);
        Task<List<VocationalTestQuestion>> GetActiveVocationalTestQuestions();
    }
}
