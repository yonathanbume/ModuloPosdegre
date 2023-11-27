using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class ImageCompanyRepository : Repository<ImageCompany>, IImageCompanyRepository
    {
        public ImageCompanyRepository(AkdemicContext context) : base(context) { }

        public async Task<ImageCompany> GetFirstOrDefaultById(Guid id)
        {
            var result = _context.ImageCompanies.Where(x => x.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<object> GetImageCompanies(Guid companyId)
        {
            var result = _context.ImageCompanies.Where(x => x.CompanyId == companyId).Select(x => new
            {
                x.Id,
                Src = "/imagenes/" + x.Picture

            });
            return await result.ToListAsync(); 
        }
      
    }
}
