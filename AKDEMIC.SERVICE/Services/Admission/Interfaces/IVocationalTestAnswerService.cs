using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVocationalTestAnswerService
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue);
        Task Insert(VocationalTestAnswer vocationalTestAnswer);
        Task<VocationalTestAnswer> Get(Guid id);
        Task Update(VocationalTestAnswer vocationalTestAnswer);
        Task DeleteById(Guid id);
    }
}
