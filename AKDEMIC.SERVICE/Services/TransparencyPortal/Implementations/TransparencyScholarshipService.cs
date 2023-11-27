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
    public class TransparencyScholarshipService : ITransparencyScholarshipService
    {
        private readonly ITransparencyScholarshipRepository _transparencyScholarshipRepository;

        public TransparencyScholarshipService(ITransparencyScholarshipRepository transparencyScholarshipRepository)
        {
            _transparencyScholarshipRepository = transparencyScholarshipRepository;
        }

        public async Task Delete(TransparencyScholarship entity)
            => await _transparencyScholarshipRepository.Delete(entity);

        public async Task<TransparencyScholarship> Get(Guid id)
            => await _transparencyScholarshipRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _transparencyScholarshipRepository.GetScholarshipDatatable(sentParameters, searchValue);

        public async Task Insert(TransparencyScholarship entity)
            => await _transparencyScholarshipRepository.Insert(entity);

        public async Task Update(TransparencyScholarship entity)
            => await _transparencyScholarshipRepository.Update(entity);
    }
}
