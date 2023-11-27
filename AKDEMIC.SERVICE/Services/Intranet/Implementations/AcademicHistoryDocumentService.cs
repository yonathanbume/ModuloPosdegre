using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AcademicHistoryDocumentService : IAcademicHistoryDocumentService
    {
        private readonly IAcademicHistoryDocumentRepository _academicHistoryDocumentRepository;
        public AcademicHistoryDocumentService(IAcademicHistoryDocumentRepository academicHistoryDocumentRepository)
        {
            _academicHistoryDocumentRepository = academicHistoryDocumentRepository;
        }

        public async Task Delete(AcademicHistoryDocument academicHistoryDocument)
            => await _academicHistoryDocumentRepository.Delete(academicHistoryDocument);

        public async Task<AcademicHistoryDocument> Get(Guid id)
            => await _academicHistoryDocumentRepository.Get(id);

        public async Task<IEnumerable<AcademicHistoryDocument>> GetAll() => await _academicHistoryDocumentRepository.GetAll();

        public async Task<AcademicHistoryDocument> GetByStudentId(Guid studentId)
            => await _academicHistoryDocumentRepository.GetByStudentId(studentId);

        public async Task<int> GetLastCorrelative() => await _academicHistoryDocumentRepository.GetLastCorrelative();

        public async Task Insert(AcademicHistoryDocument academicHistoryDocument)
            => await _academicHistoryDocumentRepository.Insert(academicHistoryDocument);        

        public async Task Update(AcademicHistoryDocument academicHistoryDocument)
            => await _academicHistoryDocumentRepository.Update(academicHistoryDocument);
    }
}
