using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPortalGeneralFileService : ITransparencyPortalGeneralFileService
    {
        private readonly ITransparencyPortalGeneralFileRepository _transparencyPortalGeneralFileRepository;
        public TransparencyPortalGeneralFileService(ITransparencyPortalGeneralFileRepository transparencyPortalGeneralFileRepository)
        {
            _transparencyPortalGeneralFileRepository = transparencyPortalGeneralFileRepository;
        }

        public async Task DeleteById(Guid id) => await _transparencyPortalGeneralFileRepository.DeleteById(id);

        public async Task DeleteRange(IEnumerable<TransparencyPortalGeneralFile> files)
            => await _transparencyPortalGeneralFileRepository.DeleteRange(files);

        public async Task<TransparencyPortalGeneralFile> Get(Guid id) => await _transparencyPortalGeneralFileRepository.Get(id);

        public async Task<IEnumerable<TransparencyPortalGeneralFile>> GetAll() => await _transparencyPortalGeneralFileRepository.GetAll();

        public async Task<IEnumerable<TransparencyPortalGeneralFile>> GetAll(Guid generalId) => await _transparencyPortalGeneralFileRepository.GetAll(generalId);

        public async Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters, Guid id)
             => await _transparencyPortalGeneralFileRepository.GetDataTable(sentParameters, id);

        public async Task Insert(TransparencyPortalGeneralFile file) => await _transparencyPortalGeneralFileRepository.Insert(file);

        public async Task Update(TransparencyPortalGeneralFile file) => await _transparencyPortalGeneralFileRepository.Update(file);
    }
}
