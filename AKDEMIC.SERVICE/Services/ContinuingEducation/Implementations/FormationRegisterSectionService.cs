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
    public class FormationRegisterSectionService : IFormationRegisterSectionService
    {
        private readonly IFormationRegisterSectionRepository _formationRegisterSectionRepository;

        public FormationRegisterSectionService(IFormationRegisterSectionRepository formationRegisterSectionRepository)
        {
            _formationRegisterSectionRepository = formationRegisterSectionRepository;
        }

        public Task<bool> AnyBySectionDni(Guid sectionId, string dni)
            => _formationRegisterSectionRepository.AnyBySectionDni(sectionId, dni);

        public Task<bool> AnyBySectionUser(Guid sectionId, string userId)
            => _formationRegisterSectionRepository.AnyBySectionUser(sectionId, userId); 

        public Task Delete(RegisterSection formationRegisterSection)
            => _formationRegisterSectionRepository.Delete(formationRegisterSection);

        public Task<RegisterSection> Get(Guid id)
            => _formationRegisterSectionRepository.Get(id);

        public Task<IEnumerable<RegisterSection>> GetAll()
            => _formationRegisterSectionRepository.GetAll();

        public Task Insert(RegisterSection formationRegisterSection)
            => _formationRegisterSectionRepository.Insert(formationRegisterSection);

        public Task Update(RegisterSection formationRegisterSection)
            => _formationRegisterSectionRepository.Update(formationRegisterSection);
    }
}
