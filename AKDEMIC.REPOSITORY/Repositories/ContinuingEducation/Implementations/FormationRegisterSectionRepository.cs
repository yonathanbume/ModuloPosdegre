using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Implementations
{
    public class FormationRegisterSectionRepository : Repository<ENTITIES.Models.ContinuingEducation.RegisterSection>, IFormationRegisterSectionRepository
    {
        public FormationRegisterSectionRepository(AkdemicContext context) : base(context) { }


        public async Task<bool> AnyBySectionDni(Guid sectionId, string dni)
        {
            return await _context.FormationRegisterSections.AnyAsync(x => !x.IsInternal && x.SectionId == sectionId && x.Dni == dni);
        }

        public async Task<bool> AnyBySectionUser(Guid sectionId, string userId)
        {
            return await _context.FormationRegisterSections.AnyAsync(x => x.SectionId == sectionId && x.UserId == userId);
        }
    }
}
