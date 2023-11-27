using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces
{
    public interface IFormationCourseTypeRepository:IRepository<ENTITIES.Models.ContinuingEducation.CourseType>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
