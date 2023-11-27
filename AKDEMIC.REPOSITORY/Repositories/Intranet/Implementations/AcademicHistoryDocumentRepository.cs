using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AcademicHistoryDocumentRepository:Repository<AcademicHistoryDocument> , IAcademicHistoryDocumentRepository
    {
        public AcademicHistoryDocumentRepository(AkdemicContext context):base(context) { }


        public async Task<AcademicHistoryDocument> GetByStudentId(Guid studentId)
            => await _context.AcademicHistoryDocuments.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();

        public async Task<int> GetLastCorrelative()
           => await _context.AcademicHistoryDocuments.IgnoreQueryFilters().CountAsync();
        
    }
}
