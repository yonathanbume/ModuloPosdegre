using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class PostulantCardSectionService : IPostulantCardSectionService
    {
        private readonly IPostulantCardSectionRepository _postulantCardSectionRepository;
        public PostulantCardSectionService(IPostulantCardSectionRepository postulantCardSectionRepository)
        {
            _postulantCardSectionRepository = postulantCardSectionRepository;
        }
        public async Task<List<PostulantCardSection>> GetConfiguration(Guid admissionTypeId)
        {
            return await _postulantCardSectionRepository.GetConfiguration(admissionTypeId);
        }

        public async Task SaveConfiguration(Guid id, List<PostulantCardSection> sections)
        {
            await _postulantCardSectionRepository.SaveConfiguration(id, sections);
        }
    }
}
