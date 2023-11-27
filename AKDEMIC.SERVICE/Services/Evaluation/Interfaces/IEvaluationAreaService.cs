using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IEvaluationAreaService
    {
        Task<bool> AnyByAreaId(Guid areaId);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Update(EvaluationArea entity);
        Task Insert(EvaluationArea entity);
        Task Delete(EvaluationArea entity);
        Task<EvaluationArea> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<EvaluationArea>> GetEvaluationDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide();
    }
}
