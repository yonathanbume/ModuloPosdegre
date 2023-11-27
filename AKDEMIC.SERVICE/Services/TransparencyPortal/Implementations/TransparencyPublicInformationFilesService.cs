using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPublicInformationFilesService : ITransparencyPublicInformationFilesService
    {
        private readonly ITransparencyPublicInformationFilesRepository _transparencyPublicInformationFilesRepository;
        public TransparencyPublicInformationFilesService(ITransparencyPublicInformationFilesRepository transparencyPublicInformationFilesRepository)
        {
            _transparencyPublicInformationFilesRepository = transparencyPublicInformationFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyPublicInformationFilesRepository.DeleteById(id);
        }

        public async  Task<TransparencyPublicInformationFile> Get(Guid id)
        {
            return await _transparencyPublicInformationFilesRepository.Get(id);
        }

        public async Task<List<TransparencyPublicInformationFile>> GetByTransparencyPublicInformationId(Guid id)
        {
            return await _transparencyPublicInformationFilesRepository.GetByTransparencyPublicInformationId(id);
        }

        public async Task Insert(TransparencyPublicInformationFile regulation)
        {
            await _transparencyPublicInformationFilesRepository.Insert(regulation);
        }

        public async Task Update(TransparencyPublicInformationFile regulation)
        {
            await _transparencyPublicInformationFilesRepository.Update(regulation);
        }
    }
}
