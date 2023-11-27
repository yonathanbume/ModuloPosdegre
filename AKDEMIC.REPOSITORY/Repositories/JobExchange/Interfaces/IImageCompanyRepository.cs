
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IImageCompanyRepository : IRepository<ImageCompany>
    {    
        Task<object> GetImageCompanies(Guid companyId);
        Task<ImageCompany> GetFirstOrDefaultById(Guid id);
    }
}
