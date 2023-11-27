using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface ICareerTermRepository : IRepository<CareerApplicationTerm>
    {
        Task<CareerApplicationTerm> GetCareerTermById(Guid careerApplicationTermId);
        Task<CareerApplicationTerm> GetCareerTermByCareerIdAndApplicationTermId(Guid careerId, Guid applicationTermId);
        Task<List<Career>> GetCareersToAdd(Guid applicationTermId, Guid campusId);
        Task<List<CareerApplicationTerm>> GetCareersByTermInclude(Guid applicationTermId, Guid? campusId = null);
        Task<List<CareerApplicationTerm>> GetCareersInclude(Guid termId);
        Task SaveChanges();
        Task DeleteCareerTermById(Guid id);
    }
}
