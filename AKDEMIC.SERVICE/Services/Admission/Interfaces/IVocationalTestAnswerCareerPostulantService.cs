using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVocationalTestAnswerCareerPostulantService
    {
        Task Insert(VocationalTestAnswerCareerPostulant vocationalTestAnswerCareerPostulant);
        Task<IEnumerable<VocationalTestAnswerCareerPostulant>> GetAll();
        Task<object> GetVocationalTestAnswerCareerPostulantsFiltered(Guid postulantId);
        Task<DataTablesStructs.ReturnedData<object>> GetVocationalTestAnswerCareerDataTable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null);
        Task<object> GetChart(Guid applicationTermId);
    }
}
