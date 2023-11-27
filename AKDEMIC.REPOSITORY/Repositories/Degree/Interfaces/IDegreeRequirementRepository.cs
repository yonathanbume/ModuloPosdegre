using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces
{
    public interface IDegreeRequirementRepository : IRepository<DegreeRequirement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDegreeRequirementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<DegreeRequirement>> GetDegreeRequirementsByType(int type);
    
    }
}
