using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IVocationalTestAnswerCareerRepository: IRepository<VocationalTestAnswerCareer>
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue = null);
        Task<List<Guid>> GetCareersByAnswers(Guid vocationalTestAnswerCareerId);
        Task<List<VocationalTestAnswerCareer>> GetVocationalTestAnswerCareersFiltered(Guid vocationalTestAnswerId);
        Task<VocationalTestAnswerCareer> GetVocationalTestAnswerCareerFirstOrDefault(Guid vocationalTestAnswerId);
    }
}
