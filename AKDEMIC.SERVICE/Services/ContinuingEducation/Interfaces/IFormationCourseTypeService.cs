using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationCourseTypeService
    {
        Task<ENTITIES.Models.ContinuingEducation.CourseType> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.CourseType>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.CourseType formationCourseType);
        Task Update(ENTITIES.Models.ContinuingEducation.CourseType formationCourseType);
        Task Delete(ENTITIES.Models.ContinuingEducation.CourseType formationCourseType);

        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
