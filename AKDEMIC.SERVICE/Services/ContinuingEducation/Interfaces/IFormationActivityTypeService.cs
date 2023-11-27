using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationActivityTypeService
    {
        Task<ENTITIES.Models.ContinuingEducation.ActivityType> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.ActivityType>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.ActivityType formationActivityType);
        Task Update(ENTITIES.Models.ContinuingEducation.ActivityType formationActivityType);
        Task Delete(ENTITIES.Models.ContinuingEducation.ActivityType formationActivityType);

        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationActivitiesTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
