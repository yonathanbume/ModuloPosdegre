using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces
{
    public interface IFormationActivityTypeRepository:IRepository<ENTITIES.Models.ContinuingEducation.ActivityType>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationActivitiesTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
