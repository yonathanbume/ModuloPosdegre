using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.SpecialCase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class SpecialCaseRepository : Repository<SpecialCase>, ISpecialCaseRepository
    {
        public SpecialCaseRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByAdmissiontypeIdAndCareerId(Guid admissionTypeId, Guid careerId, Guid? specialCaseId = null)
        {
            if (specialCaseId.HasValue&&specialCaseId != Guid.Empty)
            {
                return await _context.SpecialCases.AnyAsync(x => specialCaseId.Value != x.Id && x.AdmissionTypeId == admissionTypeId && x.CareerId == careerId);
            }
            return await _context.SpecialCases.AnyAsync(x => x.AdmissionTypeId == admissionTypeId && x.CareerId == careerId);
        }

        public async Task<List<SpecialCaseTemplate>> GetSpecialCasesByAdmissionTypeId(Guid admissionTypeId)
        {
            return await _context.SpecialCases
                .Where(x => x.AdmissionTypeId == admissionTypeId)
                .Select(x => new SpecialCaseTemplate
                {
                    Id= x.Id,
                    Career = x.Career.Name,
                    Cost = x.IsExonerated ? "Exonerado" : $"Pub: S/. {x.PublicConcept.Amount:0.00}<br /> Priv: S/. {x.PrivateConcept.Amount:0.00}<br /> Ext: S/. {x.ForeignConcept.Amount:0.00}",
                })
                .ToListAsync();
        }


        public async Task<List<SpecialCase>> GetAllByAdmissionTypeId(Guid admissionTypeId)
        {
            return await _context.SpecialCases
                .Where(x => x.AdmissionTypeId == admissionTypeId)
                .Include(x => x.PublicConcept)
                .Include(x => x.PrivateConcept)
                .Include(x => x.ForeignConcept)
                .ToListAsync();
        }
    }
}
