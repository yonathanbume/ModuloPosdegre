using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class SedeRepository : Repository<Sede>, ISedeRepository
    {
        public SedeRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Sede>> GetListSedesByCompany(Guid companyId)
        {
           var result =  _context.Sedes.Where(x => x.CompanyId == companyId);
           return await result.ToListAsync();
        }

        public async Task<object> GetSedesByCompany(Guid companyId)
        {
            return await _context.Sedes
                .Where(x => x.CompanyId == companyId)
                .Select(x => new
                {
                    Description = x.Name,
                    Address = x.Description,
                    Department = x.District.Province.Department.Name,
                    Province = x.District.Province.Name,
                    District = x.District.Name,
                    x.DistrictId
                }).ToListAsync(); ;
        }

        public async Task<object> GetSedesByUser(string userId)
        {
            Guid companyId = Guid.Empty;
            var company = await _context.Companies.Where(x => x.UserId == userId && x.IsExternalRequest == false).FirstOrDefaultAsync();

            if (company != null)
                companyId = company.Id;

            var result = await _context.Sedes
                .Where(x => x.CompanyId == companyId)
                .Select(x => new
                {
                    Description = x.Name,
                    Address = x.Description,
                    Department = x.District.Province.Department.Name,
                    Province = x.District.Province.Name,
                    District = x.District.Name,
                    x.DistrictId
                })
                .ToListAsync();

            return result;
        }
    }
}
