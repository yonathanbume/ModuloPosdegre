using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Interfaces
{
    public interface IDegreeRequirementService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDegreeRequirementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(DegreeRequirement entity);
        Task<DegreeRequirement> Get(Guid id);
        Task Update(DegreeRequirement entity);
        Task Delete(DegreeRequirement entity);
        Task<IEnumerable<DegreeRequirement>> GetDegreeRequirementsByType(int type);
        
    }
}
