using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class CareerTermService : ICareerTermService
    {
        private readonly ICareerTermRepository _careerTermRepository;

        public CareerTermService(ICareerTermRepository careerTermRepository)
        {
            _careerTermRepository = careerTermRepository;
        }

        public async Task InsertCareerTerm(CareerApplicationTerm careerTerm) =>
            await _careerTermRepository.Insert(careerTerm);

        public async Task AddAsync(CareerApplicationTerm careerTerm)
            => await _careerTermRepository.Add(careerTerm);

        public async Task UpdateCareerTerm(CareerApplicationTerm careerTerm) =>
            await _careerTermRepository.Update(careerTerm);

        public async Task DeleteCareerTerm(CareerApplicationTerm careerTerm) =>
            await _careerTermRepository.Delete(careerTerm);

        public async Task<CareerApplicationTerm> GetCareerTermById(Guid id) =>
            await _careerTermRepository.GetCareerTermById(id);

        public async Task<IEnumerable<CareerApplicationTerm>> GetAllCareerTerms() =>
            await _careerTermRepository.GetAll();

        public async Task<CareerApplicationTerm> GetCareerTermByCareerIdAndApplicationTermId(Guid careerId, Guid applicationTermId)
            => await _careerTermRepository.GetCareerTermByCareerIdAndApplicationTermId(careerId, applicationTermId);

        public async Task<List<Career>> GetCareersToAdd(Guid applicationTermId, Guid campusId)
            => await _careerTermRepository.GetCareersToAdd(applicationTermId, campusId);

        public async Task<List<CareerApplicationTerm>> GetCareersByTermInclude(Guid applicationTermId, Guid? campusId = null)
            => await _careerTermRepository.GetCareersByTermInclude(applicationTermId, campusId);

        public async Task<List<CareerApplicationTerm>> GetCareersInclude(Guid termId)
            => await _careerTermRepository.GetCareersInclude(termId);

        public async Task DeleteCareerTermById(Guid id)
        {
            await _careerTermRepository.DeleteCareerTermById(id);
        }

        public async Task SaveChanges()
        {
            await _careerTermRepository.SaveChanges();
        }
    }
}
