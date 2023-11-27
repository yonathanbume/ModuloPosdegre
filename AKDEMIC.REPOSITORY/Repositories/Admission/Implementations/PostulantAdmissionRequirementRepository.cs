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
    public class PostulantAdmissionRequirementRepository : Repository<PostulantAdmissionRequirement>, IPostulantAdmissionRequirementRepository
    {

        public PostulantAdmissionRequirementRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<PostulantAdmissionRequirement>> GetAllByPostulant(Guid postulantId)
        {
            var requirements = await _context.PostulantAdmissionRequirements
                .Where(x => x.PostulantId == postulantId)
                .Include(x => x.AdmissionRequirement)
                .ToListAsync();
            return requirements;
        }

        public async Task<PostulantAdmissionRequirement> GetPostulantAndAdmissionRequirement(Guid postulantId, Guid requirementId)
        {
            var postulantRequirement = await _context.PostulantAdmissionRequirements
                .Include(x => x.AdmissionRequirement)
                .FirstOrDefaultAsync(x => x.PostulantId == postulantId && x.AdmissionRequirementId == requirementId);
            return postulantRequirement;
        }

        public async Task RemoveByPostulantId (Guid postulantId)
        {
            var requirements = await _context.PostulantAdmissionRequirements.Where(x => x.PostulantId == postulantId).ToListAsync();
            _context.PostulantAdmissionRequirements.RemoveRange(requirements);
            await _context.SaveChangesAsync();
        }
    }
}
