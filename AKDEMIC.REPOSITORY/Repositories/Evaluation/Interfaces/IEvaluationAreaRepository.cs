using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IEvaluationAreaRepository : IRepository<EvaluationArea>
    {
        Task<DataTablesStructs.ReturnedData<EvaluationArea>> GetEvaluationDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<bool> AnyByAreaId(Guid areaId);
        Task<bool> AnyByName(string name,Guid? ignoredId = null);
        Task<IEnumerable<Select2Structs.Result>> GetAreasSelect2ClientSide();
    }
}
