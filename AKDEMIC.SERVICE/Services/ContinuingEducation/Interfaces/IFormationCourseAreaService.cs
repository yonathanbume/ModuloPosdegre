using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationCourseAreaService
    {
        Task<ENTITIES.Models.ContinuingEducation.CourseArea> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.CourseArea>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.CourseArea formationCourseArea);
        Task Update(ENTITIES.Models.ContinuingEducation.CourseArea formationCourseArea);
        Task Delete(ENTITIES.Models.ContinuingEducation.CourseArea formationCourseArea);

        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
