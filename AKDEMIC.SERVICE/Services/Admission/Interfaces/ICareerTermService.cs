using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface ICareerTermService
    {
        Task InsertCareerTerm(CareerApplicationTerm careerTerm);
        Task AddAsync(CareerApplicationTerm careerTerm);
        Task UpdateCareerTerm(CareerApplicationTerm careerTerm);
        Task DeleteCareerTerm(CareerApplicationTerm careerTerm);
        Task<CareerApplicationTerm> GetCareerTermById(Guid id);
        Task<IEnumerable<CareerApplicationTerm>> GetAllCareerTerms();
        Task<CareerApplicationTerm> GetCareerTermByCareerIdAndApplicationTermId(Guid careerId, Guid applicationTermId);
        Task<List<Career>> GetCareersToAdd(Guid applicationTermId, Guid campusId);
        Task<List<CareerApplicationTerm>> GetCareersByTermInclude(Guid applicationTermId, Guid? campusId = null);
        Task<List<CareerApplicationTerm>> GetCareersInclude(Guid termId);
        Task DeleteCareerTermById(Guid id);
        Task SaveChanges();
    }
}
