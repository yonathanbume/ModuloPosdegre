using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IJobOfferLanguageRepository :IRepository<JobOfferLanguage>
    {
        Task<IEnumerable<JobOfferLanguage>> GetAllByJobOfferId(Guid jobOfferId);
        Task<bool> ExistByLanguage(Guid id);
    }
}
