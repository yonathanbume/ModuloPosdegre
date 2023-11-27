using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectRubricRepository : IRepository<ProjectRubric>
    {
        Task<IEnumerable<Select2Structs.Result>> GetProjectRubricsSelect2ClientSide(byte? type = null);
        Task<ProjectRubric> GetByType(byte type);
        Task<DataTablesStructs.ReturnedData<ProjectRubric>> GetProjectRubricDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<ProjectRubric>> GetProjectRubricsByType(byte type, bool? status = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
