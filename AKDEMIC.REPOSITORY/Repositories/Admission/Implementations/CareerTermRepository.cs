using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
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
    public class CareerTermRepository : Repository<CareerApplicationTerm>, ICareerTermRepository
    {
        public CareerTermRepository(AkdemicContext context) : base(context) { }

        public async Task<CareerApplicationTerm> GetCareerTermByCareerIdAndApplicationTermId(Guid careerId, Guid applicationTermId)
        {
            var result = await _context.CareerApplicationTerms.Where(x => x.CareerId == careerId && x.ApplicationTermId == applicationTermId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Career>> GetCareersToAdd(Guid applicationTermId, Guid campusId)
        {
            var actualCareers = await _context.CareerApplicationTerms
                .Where(x => x.ApplicationTermId == applicationTermId && x.CampusId == campusId)
                .Select(x=>x.CareerId)
                .ToListAsync();

            var careers = await _context.Careers.Where(x => !actualCareers.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();

            return careers;
        }
        public async Task<List<CareerApplicationTerm>> GetCareersByTermInclude(Guid applicationTermId, Guid? campusId = null)
        {
            var qry = _context.CareerApplicationTerms
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsQueryable();

            if (campusId.HasValue && campusId != Guid.Empty)
                qry = qry.Where(x => x.CampusId == campusId);

            var careers = await qry
                .Include(x => x.Career)
                .Include(x => x.ApplicationTerm)
                .ToListAsync();

            return careers;
        }

        public async Task<List<CareerApplicationTerm>> GetCareersInclude(Guid termId)
        {
            var result = await _context.CareerApplicationTerms.Include(x => x.Career)
                .Where(x => x.ApplicationTermId == termId).ToListAsync();

            return result;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCareerTermById(Guid id)
        {
            var careerTerm = await _context.CareerApplicationTerms.FindAsync(id);

            var vacants = await _context.Vacants
                .Where(x => x.CareerApplicationTermId == careerTerm.Id)
                .ToListAsync();

            _context.Vacants.RemoveRange(vacants);
            _context.CareerApplicationTerms.Remove(careerTerm);
            await _context.SaveChangesAsync();
        }

        public async Task<CareerApplicationTerm> GetCareerTermById(Guid careerApplicationTermId)
        {
            var result = await _context.CareerApplicationTerms
                .Where(x => x.Id == careerApplicationTermId)
                .Include(x => x.Campus)
                .Include(x => x.Career)
                .Include(x => x.ApplicationTerm)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
