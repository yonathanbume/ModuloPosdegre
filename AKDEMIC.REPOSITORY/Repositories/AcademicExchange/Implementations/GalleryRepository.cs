using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class GalleryRepository : Repository<Gallery>, IGalleryRepository
    {
        public GalleryRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> ExistName(string name, Guid? id)
        {
            var query = _context.Galleries.AsNoTracking();
            if (id.HasValue)
            {
                return await query.AnyAsync(x => x.Name == name && x.Id != id.Value);
            }
            return await query.AnyAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Gallery>> GetAllByPublicationId(Guid publicationid, string search)
        {
            return await _context.Galleries.Where(x => x.ScholarshipId == publicationid).ToListAsync();
        }
        public async Task<IEnumerable<Gallery>> GetAllByAcademicAgreementId(Guid academicAgreementId, string search)
        {
            return await _context.Galleries.Where(x => x.ScholarshipId  == academicAgreementId).ToListAsync();
        }

        public async Task<IEnumerable<Gallery>> GetAllByScholarshipId(Guid scholarshipId)
            => await _context.Galleries.Where(x => x.ScholarshipId == scholarshipId).ToArrayAsync();

        public async Task<GalleryTemplate> GetAllServerSideByScholarshipId(int page, Guid scholarshipId, string search)
        {
            var query = _context.Galleries.Where(x => x.ScholarshipId == scholarshipId).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            var total = await query.CountAsync();
            
            var result = await query.Skip(5 * page).Take(5)
                .Select(x => new Gallery
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    ScholarshipId = x.ScholarshipId
                }).ToArrayAsync();

            var data = new GalleryTemplate
            {
                Gallery = result,
                TotalRecords = total
            };

            return data;
        }
    }
}
