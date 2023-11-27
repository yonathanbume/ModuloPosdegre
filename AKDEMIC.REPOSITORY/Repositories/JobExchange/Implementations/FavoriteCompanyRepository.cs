using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class FavoriteCompanyRepository:Repository<FavoriteCompany>, IFavoriteCompanyRepository
    {
        public FavoriteCompanyRepository(AkdemicContext context):base(context) { }

        public async Task<FavoriteCompany> GetByCompanyAndUser(Guid companyId, string userId)
        {
            var result = await _context.FavoriteCompanies
                .Where(x => x.CompanyId == companyId && x.UserId == userId)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
