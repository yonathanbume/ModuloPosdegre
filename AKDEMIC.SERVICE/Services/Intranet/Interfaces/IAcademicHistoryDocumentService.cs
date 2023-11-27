using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAcademicHistoryDocumentService
    {
        Task<AcademicHistoryDocument> Get(Guid id);
        Task<IEnumerable<AcademicHistoryDocument>> GetAll();
        Task Insert(AcademicHistoryDocument academicHistoryDocument);
        Task Update(AcademicHistoryDocument academicHistoryDocument);
        Task Delete(AcademicHistoryDocument academicHistoryDocument);
        Task<AcademicHistoryDocument> GetByStudentId(Guid studentId);
        Task<int> GetLastCorrelative();
    }
}
