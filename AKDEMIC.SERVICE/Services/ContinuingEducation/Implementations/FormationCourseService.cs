using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationCourse;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationCourseService : IFormationCourseService
    {
        private readonly IFormationCourseRepository _formationCourseRepository;
        public FormationCourseService(IFormationCourseRepository formationCourseRepository)
        {
            _formationCourseRepository = formationCourseRepository;
        }
        public Task<bool> AnyByName(string name, Guid? id = null)
            => _formationCourseRepository.AnyByName(name, id);
        public Task Delete(ENTITIES.Models.ContinuingEducation.Course formationCourse)
            => _formationCourseRepository.Delete(formationCourse);

        public Task<ENTITIES.Models.ContinuingEducation.Course> Get(Guid id)
            => _formationCourseRepository.Get(id);

        public Task<IEnumerable<ENTITIES.Models.ContinuingEducation.Course>> GetAll()
            => _formationCourseRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationCourseRepository.GetAllFormationCoursesDatatable(sentParameters, searchValue);

        public Task<FormationCourseTemplate> GetInformation(Guid id)
            => _formationCourseRepository.GetInformation(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationCourseRepository.GetReportDatatable(sentParameters, searchValue);

        public Task Insert(ENTITIES.Models.ContinuingEducation.Course formationCourse)
            => _formationCourseRepository.Insert(formationCourse);

        public Task Update(ENTITIES.Models.ContinuingEducation.Course formationCourse)
            => _formationCourseRepository.Update(formationCourse);
    }
}
