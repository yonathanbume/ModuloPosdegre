using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermService
    {
        Task InsertApplicationTerm(ApplicationTerm applicationTerm);
        Task UpdateApplicationTerm(ApplicationTerm applicationTerm);
        Task DeleteApplicationTerm(ApplicationTerm applicationTerm);
        Task<ApplicationTerm> GetApplicationTermById(Guid id);
        Task<IEnumerable<ApplicationTerm>> GetAllApplicationTerms();
        Task<string> GetApplicationTermName(int status);
        Task<object> GetTermsVacant();
        Task<ApplicationTerm> GetWithTerm(Guid id);
        Task<IEnumerable<ApplicationTerm>> GetAllApplicationTermsWithData(Guid? termId=null);
        Task<List<Career>> GetCareers(Guid applicationTermId);
        Task<object> GetAdmissionTerms();
        Task<ApplicationTerm> GetActiveTerm();
        Task<ApplicationTerm> GetLastTerm();
        Task<object> GetPostulantsApplication(Guid termId);
        Task<object> GetApplicationTermWithTerm();
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<object> GetApplicationTermObject(Guid id);
        Task<bool> AnyAsync(Guid termId);
        Task<ApplicationTerm> AddApp();
        Task<bool> AnyAsyncByFieldAndId(Guid termId, Guid id);
        Task<bool> AnyAsyncByTermDate(DateTime startDate, DateTime endDate);
        Task<bool> AnyAsyncByTermDateById(Guid termId, DateTime startDate, DateTime endDate);
        Task<object> GetActiveApplicationTerms();
        Task<object> GetAvailableInscriptionApplicationTerms();
    }
}
