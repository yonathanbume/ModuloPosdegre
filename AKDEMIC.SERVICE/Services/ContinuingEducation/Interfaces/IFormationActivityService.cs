using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationActivityService
    {
        Task<ENTITIES.Models.ContinuingEducation.Activity> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.Activity>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.Activity formationActivity);
        Task Update(ENTITIES.Models.ContinuingEducation.Activity formationActivity);
        Task Delete(ENTITIES.Models.ContinuingEducation.Activity formationActivity);

        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
