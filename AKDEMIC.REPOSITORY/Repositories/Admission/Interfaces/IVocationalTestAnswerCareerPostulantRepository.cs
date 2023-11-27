using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IVocationalTestAnswerCareerPostulantRepository:IRepository<VocationalTestAnswerCareerPostulant>
    {
        Task<object> GetVocationalTestAnswerCareerPostulantsFiltered(Guid postulantId);
        Task<DataTablesStructs.ReturnedData<object>> GetVocationalTestAnswerCareerDataTable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null);
        Task<object> GetChart(Guid applicationTermId);
    }
}
