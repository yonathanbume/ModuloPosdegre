using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class FacultyCurriculumAreaRepository : Repository<FacultyCurriculumArea>, IFacultyCurriculumAreaRepository
    {
        public FacultyCurriculumAreaRepository(AkdemicContext context) : base(context)
        { }

        public async Task DeleteByCurriculumAreaId(Guid curriculumAreaId)
        {
            var toRemove = await _context.FacultyCurriculumAreas.Where(x => x.CurriculumAreaId == curriculumAreaId)
                .ToListAsync();
            _context.FacultyCurriculumAreas.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetByCurriculumAreaId(Guid id)
        {
            return await _context.FacultyCurriculumAreas.Where(x => x.CurriculumArea.Id == id)
                .Select(x => new 
                {
                    Name = x.Faculty.Name
                })
                .ToListAsync();
        }
    }
}
