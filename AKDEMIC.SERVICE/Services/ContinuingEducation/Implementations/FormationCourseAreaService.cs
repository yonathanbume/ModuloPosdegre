using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ContinuingEducation;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationCourseAreaService : IFormationCourseAreaService
    {
        private readonly IFormationCourseAreaRepository _formationCourseAreaRepository;

        public FormationCourseAreaService(IFormationCourseAreaRepository formationCourseAreaRepository)
        {
            _formationCourseAreaRepository = formationCourseAreaRepository;
        }
        public Task<bool> AnyByName(string name, Guid? id = null)
            => _formationCourseAreaRepository.AnyByName(name, id);

        public Task Delete(CourseArea formationCourseArea)
            => _formationCourseAreaRepository.Delete(formationCourseArea);

        public Task<CourseArea> Get(Guid id)
            => _formationCourseAreaRepository.Get(id);

        public Task<IEnumerable<CourseArea>> GetAll()
            => _formationCourseAreaRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationCourseAreaRepository.GetAllFormationCourseAreasDatatable(sentParameters, searchValue);

        public Task Insert(CourseArea formationCourseArea)
            => _formationCourseAreaRepository.Insert(formationCourseArea);

        public Task Update(CourseArea formationCourseArea)
            => _formationCourseAreaRepository.Update(formationCourseArea);
    }
}
