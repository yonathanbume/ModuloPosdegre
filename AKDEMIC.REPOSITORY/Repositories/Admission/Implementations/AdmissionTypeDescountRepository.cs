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
    public class AdmissionTypeDescountRepository : Repository<AdmissionTypeDescount>, IAdmissionTypeDescountRepository
    {

        public AdmissionTypeDescountRepository(AkdemicContext context): base(context) { }

        public async Task<List<AdmissionTypeDescount>> GetAdmissionTypeDescountByTermId(Guid termId)
        {
            var existingDescounts = await _context.AdmissionTypeDescounts.Where(a => a.TermId == termId).ToListAsync();

            return existingDescounts;
        }
        public async Task<AdmissionTypeDescount> GetAdmissionTypeDescountByAdmissionTypeIdAndTermId(Guid admissionTypeId, Guid id)
            => await _context.AdmissionTypeDescounts.Where(x => x.AdmissionTypeId == admissionTypeId && x.TermId == id).FirstOrDefaultAsync();
    }
}
