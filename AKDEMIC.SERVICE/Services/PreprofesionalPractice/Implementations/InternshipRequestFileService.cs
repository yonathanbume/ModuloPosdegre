using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Implementations
{
    public class InternshipRequestFileService : IInternshipRequestFileService
    {
        public IInternshipRequestFileRepository _internshipRequestFileRepository { get; }

        public InternshipRequestFileService(
            IInternshipRequestFileRepository internshipRequestFileRepository
            )
        {
            _internshipRequestFileRepository = internshipRequestFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid internshipRequestId, byte type)
            => await _internshipRequestFileRepository.GetDatatable(sentParameters, internshipRequestId, type);

        public async Task Insert(InternshipRequestFile entity)
            => await _internshipRequestFileRepository.Insert(entity);

        public async Task<InternshipRequestFile> Get(Guid id)
            => await _internshipRequestFileRepository.Get(id);

        public async Task Delete(InternshipRequestFile entity)
            => await _internshipRequestFileRepository.Delete(entity);
    }
}
