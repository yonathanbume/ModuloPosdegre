using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectRubricService
    {
        Task<IEnumerable<Select2Structs.Result>> GetProjectRubricsSelect2ClientSide(byte? type = null);
        Task<ProjectRubric> GetByType(byte type);
        Task<ProjectRubric> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<ProjectRubric>> GetProjectRubricDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task Insert(ProjectRubric projectRubric);
        Task<IEnumerable<ProjectRubric>> GetProjectRubricsByType(byte type, bool? status = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task UpdateRange(IEnumerable<ProjectRubric> entites);
        Task Update(ProjectRubric entity);
        Task Delete(ProjectRubric entity);
    }
}
