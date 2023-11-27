using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermService : IApplicationTermService
    {
        private readonly IApplicationTermRepository _applicationTermRepository;

        public ApplicationTermService(IApplicationTermRepository applicationTermRepository)
        {
            _applicationTermRepository = applicationTermRepository;
        }

        public async Task InsertApplicationTerm(ApplicationTerm applicationTerm) =>
            await _applicationTermRepository.Insert(applicationTerm);

        public async Task UpdateApplicationTerm(ApplicationTerm applicationTerm) =>
            await _applicationTermRepository.Update(applicationTerm);

        public async Task DeleteApplicationTerm(ApplicationTerm applicationTerm) =>
            await _applicationTermRepository.Delete(applicationTerm);

        public async Task<ApplicationTerm> GetApplicationTermById(Guid id) =>
            await _applicationTermRepository.Get(id);

        public async Task<IEnumerable<ApplicationTerm>> GetAllApplicationTerms() =>
            await _applicationTermRepository.GetAll();

        public async Task<string> GetApplicationTermName(int status)
            => await _applicationTermRepository.GetApplicationTermName(status);

        public async Task<object> GetTermsVacant()
            => await _applicationTermRepository.GetTermsVacant();

        public async Task<ApplicationTerm> GetWithTerm(Guid id)
            => await _applicationTermRepository.GetWithTerm(id);

        public async Task<IEnumerable<ApplicationTerm>> GetAllApplicationTermsWithData(Guid? termId=null)
            => await _applicationTermRepository.GetAllApplicationTermsWithData(termId);

        public async Task<object> GetAdmissionTerms()
            => await _applicationTermRepository.GetAdmissionTerms();

        public async Task<ApplicationTerm> GetActiveTerm()
            => await _applicationTermRepository.GetActiveTerm();

        public async Task<object> GetPostulantsApplication(Guid termId)
            => await _applicationTermRepository.GetPostulantsApplication(termId);

        public async Task<object> GetApplicationTermWithTerm()
            => await _applicationTermRepository.GetApplicationTermWithTerm();
        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _applicationTermRepository.GetDatatable(sentParameters, search);
        public async Task<object> GetApplicationTermObject(Guid id)
            => await _applicationTermRepository.GetApplicationTermObject(id);
        public async Task<bool> AnyAsync(Guid termId)
            => await _applicationTermRepository.AnyAsync(termId);
        public async Task<ApplicationTerm> AddApp()
            => await _applicationTermRepository.AddApp();
        public async Task<bool> AnyAsyncByFieldAndId(Guid termId, Guid id)
            => await _applicationTermRepository.AnyAsyncByFieldAndId(termId, id);
        public async Task<bool> AnyAsyncByTermDate(DateTime startDate, DateTime endDate)
            => await _applicationTermRepository.AnyAsyncByTermDate(startDate, endDate);
        public async Task<bool> AnyAsyncByTermDateById(Guid termId, DateTime startDate, DateTime endDate)
            => await _applicationTermRepository.AnyAsyncByTermDateById(termId, startDate, endDate);

        public async Task<object> GetActiveApplicationTerms() => await _applicationTermRepository.GetActiveApplicationTerms();
        public async Task<object> GetAvailableInscriptionApplicationTerms() => await _applicationTermRepository.GetAvailableInscriptionApplicationTerms();

        public async Task<List<Career>> GetCareers(Guid applicationTermId) => await _applicationTermRepository.GetCareers(applicationTermId);

        public async Task<ApplicationTerm> GetLastTerm()
        {
            return await _applicationTermRepository.GetLastTerm();
        }
    }
}
