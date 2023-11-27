using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ContinuingEducation;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationCourseTypeService: IFormationCourseTypeService
    {
        private readonly IFormationCourseTypeRepository _formationCourseTypeRepository;
        public FormationCourseTypeService(IFormationCourseTypeRepository formationCourseTypeRepository)
        {
            _formationCourseTypeRepository = formationCourseTypeRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _formationCourseTypeRepository.AnyByName(name, id);

        public Task Delete(CourseType formationCourseType)
            => _formationCourseTypeRepository.Delete(formationCourseType);

        public Task<CourseType> Get(Guid id)
            => _formationCourseTypeRepository.Get(id);

        public Task<IEnumerable<CourseType>> GetAll()
            => _formationCourseTypeRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationCourseTypeRepository.GetAllFormationCourseTypesDatatable(sentParameters, searchValue);

        public Task Insert(CourseType formationCourseType)
            => _formationCourseTypeRepository.Insert(formationCourseType);

        public Task Update(CourseType formationCourseType)
            => _formationCourseTypeRepository.Update(formationCourseType);
    }
}
