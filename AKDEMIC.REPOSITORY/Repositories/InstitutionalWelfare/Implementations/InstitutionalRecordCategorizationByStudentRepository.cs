using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalRecordCategorizationByStudentRepository: Repository<InstitutionalRecordCategorizationByStudent>, IInstitutionalRecordCategorizationByStudentRepository
    {
        public InstitutionalRecordCategorizationByStudentRepository(AkdemicContext akdemicContext) : base(akdemicContext)
        {

        }

        public async Task<InstitutionalRecordCategorizationByStudent> GetByStudentAndRecord(Guid recordId, Guid studentId)
        {
            return await _context.InstitutionalRecordCategorizationByStudents.Where(x => x.InstitutionalWelfareRecordId == recordId && x.StudentId== studentId).FirstOrDefaultAsync();
        }

        public async Task<object> GetStudentReport(Guid id, Guid termId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
            var query = _context.InstitutionalRecordCategorizationByStudents
                .Where(x => x.InstitutionalWelfareRecordId == id && x.TermId == termId)
                .AsNoTracking();

            if (sisfohClasification != null && sisfohClasification != 0)
                query = query.Where(x => x.SisfohClasification == sisfohClasification);

            if (categorizationLevelId != null)
                query = query.Where(x => x.CategorizationLevelId == categorizationLevelId);

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            var result = await query
                .Select(x => new
                {
                    x.CategorizationLevelId,
                    x.CategorizationLevel.Name              
                })
                .GroupBy(x => new { x.CategorizationLevelId, x.Name, })
                .Select(x=> new { 
                    x.Key.Name,
                    counter = x.Count()
                })
                .ToListAsync();
            
            return result;
        }
    }
}
