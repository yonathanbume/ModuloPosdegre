using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ITermRepository : IRepository<Term>
    {
        Task<bool> ExistActiveTerm();
        Task<bool> IsActiveTermInTime();
        Task<Term> GetActiveTerm();
        Task<Term> GetLastTerm(bool? includeSummer = true);
        Task<Term> GetTermByDateTime(DateTime dateTime);
        Task<IEnumerable<int>> GetTermYears();
        Task<IEnumerable<Term>> GetTermsByStudentSections(Guid studentId);
        Task<Select2Structs.ResponseParameters> GetTermsSelect2(Select2Structs.RequestParameters requestParameters);
        Task<IEnumerable<Select2Structs.Result>> GetTermsSelect2ClientSide(bool OnlyTermWithInstitutionalWelfareSurvey);
        Task<Term> GetReservation();
        Task<object> GetSelect2Terms(int? year = null);
        Task<Select2Structs.ResponseParameters> GetTermsByCourseIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? courseId = null, string searchValue = null);
        Task<object> GetTermsFinishedSelect2ClientSide();
        Task<object> GetTermsFinishedSelect2ClientSideByClassStartDate();
        Task<object> GetTermsWithStatus(int status);
        Task<List<Term>> GetUserCodePrefixByTermId(Guid termId);
        Task<List<Term>> GetTermsStatus(int status);
        Task<Guid> GetIdFirst();
        Task<bool> GetTermByName(string name);
        Task<List<Term>> GetTermByIdList(Guid termId);
        Task<Term> GetFirst();
        Task<object> GetTermsByStudentIdSelect2ClientSide(Guid studentId);
        Task<Term> GetByStatus(int status);
        Task<DataTablesStructs.ReturnedData<object>> GetTermsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetPastTermsSelect2ClientSide();
        Task<bool> IsInvalidDateRange(Term term);
        Task<int> GetTotalStudentsEnrolled(Guid termId, Guid? campusId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<Select2Structs.Result>> GetLastTermsSelect2ClientSide(int? yearDifference);
        Task<IEnumerable<Term>> GetTermsByStudentSectionsAndAcademicHistories(Guid studentId);
    }
}