using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamApplicationTermRepository : Repository<AdmissionExamApplicationTerm>, IAdmissionExamApplicationTermRepository
    {
        public AdmissionExamApplicationTermRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByExamIdAndAppTermId(Guid examId, Guid applicationTermId)
        {
            return await _context.AdmissionExamApplicationTerms.AnyAsync(x => x.AdmissionExamId == examId && x.ApplicationTermId==applicationTermId);
        }
        public async Task<bool> HasPrincipal(Guid applicationTermId, Guid? admissionExamId = null)
        {
            if (admissionExamId.HasValue)
                return await _context.AdmissionExamApplicationTerms.AnyAsync(x => x.AdmissionExamId!= admissionExamId.Value && x.ApplicationTermId== applicationTermId&& x.AdmissionExam.IsPrincipal);
            return await _context.AdmissionExamApplicationTerms.AnyAsync(x => x.ApplicationTermId== applicationTermId&& x.AdmissionExam.IsPrincipal);
        }
        public async Task<object> GetApplicationTermsByAdmissionExamId(Guid admissionExamId)
        {
            return await _context.AdmissionExamApplicationTerms
                                    .Where(x => x.AdmissionExamId == admissionExamId)
                                    .Select(x => new
                                    {
                                        text = x.ApplicationTerm.Name,
                                        id = x.ApplicationTermId
                                    })
                                    .OrderBy(x => x.text)
                                    .ToListAsync();
        }


        public async Task<List<AdmissionExamApplicationTerm>> GetApplicationTermsByExamId(Guid admissionExamId)
        {
            return await _context.AdmissionExamApplicationTerms
                .Where(x => x.AdmissionExamId == admissionExamId)
                .ToListAsync();
        }

        public async Task DeleteApplicationTermsByAdmissionExamId(Guid admissionExamId)
        {
            var admissionExamAppTerm=  await _context.AdmissionExamApplicationTerms
                                    .Where(x => x.AdmissionExamId == admissionExamId)
                                    .ToListAsync();

            _context.AdmissionExamApplicationTerms.RemoveRange(admissionExamAppTerm);
            await _context.SaveChangesAsync();
        }
    }
}
