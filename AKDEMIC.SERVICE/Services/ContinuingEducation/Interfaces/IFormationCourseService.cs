using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces
{
    public interface IFormationCourseService
    {
        Task<ENTITIES.Models.ContinuingEducation.Course> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<FormationCourseTemplate> GetInformation(Guid id);
        Task<IEnumerable<ENTITIES.Models.ContinuingEducation.Course>> GetAll();
        Task Insert(ENTITIES.Models.ContinuingEducation.Course formationCourse);
        Task Update(ENTITIES.Models.ContinuingEducation.Course formationCourse);
        Task Delete(ENTITIES.Models.ContinuingEducation.Course formationCourse);

        Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
