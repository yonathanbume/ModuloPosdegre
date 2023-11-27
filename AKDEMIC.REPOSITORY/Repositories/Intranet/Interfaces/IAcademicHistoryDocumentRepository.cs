using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAcademicHistoryDocumentRepository : IRepository<AcademicHistoryDocument>
    {
        Task<AcademicHistoryDocument> GetByStudentId(Guid studentId);
        Task<int> GetLastCorrelative();
    }
}
