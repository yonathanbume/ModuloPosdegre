using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IJobOfferLanguageService
    {
        Task<IEnumerable<JobOfferLanguage>> GetAllByJobOfferId(Guid jobOfferId);
        Task DeleteRange(IEnumerable<JobOfferLanguage> jobOfferLanguages);
        Task<bool> ExistByLanguage(Guid id);
    }
}
