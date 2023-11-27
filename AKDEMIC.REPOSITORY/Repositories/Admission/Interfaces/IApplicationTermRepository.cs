using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IApplicationTermRepository : IRepository<ApplicationTerm>
    {
        Task<string> GetApplicationTermName(int status);
        Task<object> GetTermsVacant();
        Task<ApplicationTerm> GetWithTerm(Guid id);
        Task<IEnumerable<ApplicationTerm>> GetAllApplicationTermsWithData(Guid? termId=null);
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
        Task<List<Career>> GetCareers(Guid applicationTermId);
    }
}
