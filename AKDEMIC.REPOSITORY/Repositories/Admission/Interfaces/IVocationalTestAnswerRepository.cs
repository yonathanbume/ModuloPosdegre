using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IVocationalTestAnswerRepository: IRepository<VocationalTestAnswer>
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswersDatatable(DataTablesStructs.SentParameters sentParameters, Guid VocationalTestQuestionId, string searchValue = null);
    }
}
