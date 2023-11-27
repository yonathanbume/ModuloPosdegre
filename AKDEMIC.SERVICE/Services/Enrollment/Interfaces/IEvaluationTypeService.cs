using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEvaluationTypeService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task Delete(EvaluationType evaluationType);
        Task Insert(EvaluationType evaluationType);
        Task Update(EvaluationType evaluationType);
        Task<EvaluationType> Get(Guid id);
        Task<bool> AnyEvaluation(Guid id, Guid? termId = null);
        Task<object> GetEvaluationTypeJson();
    }
}
