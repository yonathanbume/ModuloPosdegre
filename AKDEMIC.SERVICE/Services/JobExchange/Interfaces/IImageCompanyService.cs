using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IImageCompanyService
    {
        Task<object> GetImageCompanies(Guid companyId);
        Task InsertRange(IEnumerable<ImageCompany> imageCompanies);
        Task<ImageCompany> GetFirstOrDefaultById(Guid id);
        Task Delete(ImageCompany imageCompany);
    }
}
