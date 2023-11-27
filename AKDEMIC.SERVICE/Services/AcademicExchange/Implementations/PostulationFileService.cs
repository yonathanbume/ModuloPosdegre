using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class PostulationFileService : IPostulationFileService
    {
        private readonly IPostulationFileRepository _postulationFileRepository;

        public PostulationFileService(IPostulationFileRepository postulationFileRepository)
        {
            _postulationFileRepository = postulationFileRepository;
        }

        public async Task Delete(PostulationFile entity)
            => await _postulationFileRepository.Delete(entity);

        public async Task<PostulationFile> Get(Guid id)
            => await _postulationFileRepository.Get(id);

        public async Task<IEnumerable<PostulationFile>> GetAllByPostulationId(Guid postulationId)
            => await _postulationFileRepository.GetAllByPostulationId(postulationId);

        public async Task Insert(PostulationFile entity)
            => await _postulationFileRepository.Insert(entity);
    }
}
