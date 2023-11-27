using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVocationalTestAnswerCareerService
    {
        Task<DataTablesStructs.ReturnedData<object>> VocationalTestAnswerCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid vocationalTestQuestionId, string searchValue = null);
        Task Insert(VocationalTestAnswerCareer vocationalTestAnswerCareer);
        Task<VocationalTestAnswerCareer> Get(Guid vocationalTestAnswerCareerId);
        Task Update(VocationalTestAnswerCareer vocationalTestAnswerCareer);
        Task DeleteById(Guid id);
        Task DeleteRange(List<VocationalTestAnswerCareer>  vocationalTestAnswerCareers);
        Task<List<Guid>> GetCareersByAnswers(Guid vocationalTestAnswerCareerId);
        Task<List<VocationalTestAnswerCareer>> GetVocationalTestAnswerCareersFiltered(Guid vocationalTestAnswerId);
        Task<VocationalTestAnswerCareer> GetVocationalTestAnswerCareerFirstOrDefault(Guid vocationalTestAnswerId);
    }
}
