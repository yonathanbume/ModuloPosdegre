using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class PostulationInformationService : IPostulationInformationService
    {
        private readonly IPostulationInformationRepository _postulationInformationRepository;

        public PostulationInformationService(IPostulationInformationRepository postulationInformationRepository)
        {
            _postulationInformationRepository = postulationInformationRepository;
        }

        public async Task<PostulationInformation> Get(Guid id)
            => await _postulationInformationRepository.Get(id);
    }
}
