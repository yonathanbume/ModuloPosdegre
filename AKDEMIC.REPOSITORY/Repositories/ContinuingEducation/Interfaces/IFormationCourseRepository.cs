using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces
{
    public interface IFormationCourseRepository:IRepository<ENTITIES.Models.ContinuingEducation.Course>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<FormationCourseTemplate> GetInformation(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
