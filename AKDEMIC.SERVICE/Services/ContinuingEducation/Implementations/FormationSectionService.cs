using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ContinuingEducation;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationSection;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationSectionService : IFormationSectionService
    {
        private readonly IFormationSectionRepository _formationSectionRepository;

        public FormationSectionService(IFormationSectionRepository formationSectionRepository)
        {
            _formationSectionRepository = formationSectionRepository;
        }

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _formationSectionRepository.AnyByCode(code, id);

        public Task Delete(Section formationSection)
            => _formationSectionRepository.Delete(formationSection);

        public Task<Section> Get(Guid id)
            => _formationSectionRepository.Get(id);

        public Task<IEnumerable<Section>> GetAll()
            => _formationSectionRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? courseId = null, string searchValue = null)
            => _formationSectionRepository.GetAllFormationSectionsDatatable(sentParameters, courseId, searchValue);

        public Task<List<FormationSectionTemplate>> GetAllSectionTemplateData(int skip = 0, int take = 0, Guid? courseTypeId = null, string searchValue = null)
            => _formationSectionRepository.GetAllSectionTemplateData(skip, take, courseTypeId, searchValue);

        public Task<FormationSectionTemplate> GetSectionTemplateData(Guid id)
            => _formationSectionRepository.GetSectionTemplateData(id);

        public Task Insert(Section formationSection)
            => _formationSectionRepository.Insert(formationSection);

        public Task Update(Section formationSection)
            => _formationSectionRepository.Update(formationSection);
    }
}
