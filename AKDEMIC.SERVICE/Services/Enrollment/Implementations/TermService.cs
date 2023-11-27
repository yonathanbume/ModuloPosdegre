using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class TermService : ITermService
    {
        private readonly ITermRepository _termRepository;
        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public async Task<Select2Structs.ResponseParameters> GetTermsSelect2(Select2Structs.RequestParameters requestParameters)
        {
            return await _termRepository.GetTermsSelect2(requestParameters);
        }

        public async Task InsertTerm(Term term) =>
       await _termRepository.Insert(term);

        public async Task UpdateTerm(Term term) =>
            await _termRepository.Update(term);

        public async Task DeleteTerm(Term term) =>
            await _termRepository.Delete(term);
        
        public Task<bool> ExistActiveTerm()
            => _termRepository.ExistActiveTerm();

        public Task<Term> Get(Guid id)
            => _termRepository.Get(id);

        public Task<Term> GetActiveTerm()
            => _termRepository.GetActiveTerm();

        public Task<Term> GetLastTerm(bool? includeSummer = true)
            => _termRepository.GetLastTerm(includeSummer);

        public Task<IEnumerable<Term>> GetAll()
            => _termRepository.GetAll();
        
        public Task<Term> GetTermByDateTime(DateTime dateTime)
            => _termRepository.GetTermByDateTime(dateTime);
        
        public Task<IEnumerable<int>> GetTermYears()
            => _termRepository.GetTermYears();
        
        public Task<IEnumerable<Term>> GetTermsByStudentSections(Guid studentId)
            => _termRepository.GetTermsByStudentSections(studentId);

        public async Task<IEnumerable<Select2Structs.Result>> GetTermsSelect2ClientSide(bool OnlyTermWithInstitutionalWelfareSurvey = false)
            => await _termRepository.GetTermsSelect2ClientSide(OnlyTermWithInstitutionalWelfareSurvey);

        public Task<bool> IsActiveTermInTime()
            => _termRepository.IsActiveTermInTime();

        public async Task<Term> GetReservation()
            => await _termRepository.GetReservation();

        public async Task<object> GetSelect2Terms(int? year = null)
            => await _termRepository.GetSelect2Terms(year);

        public async Task<Select2Structs.ResponseParameters> GetTermsByCourseIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? courseId = null, string searchValue = null)
            => await _termRepository.GetTermsByCourseIdSelect2(requestParameters, courseId, searchValue);

        public async Task<object> GetTermsFinishedSelect2ClientSide()
            => await _termRepository.GetTermsFinishedSelect2ClientSide();

        public async Task<object> GetTermsFinishedSelect2ClientSideByClassStartDate()
            => await _termRepository.GetTermsFinishedSelect2ClientSideByClassStartDate();

        public async Task Update(Term term) => await _termRepository.Update(term);

        public async Task<object> GetTermsWithStatus(int status)
            => await _termRepository.GetTermsWithStatus(status);

        public async Task<List<Term>> GetUserCodePrefixByTermId(Guid termId)
            => await _termRepository.GetUserCodePrefixByTermId(termId);

        public async Task<List<Term>> GetTermsStatus(int status)
            => await _termRepository.GetTermsStatus(status);
        public async Task<Guid> GetIdFirst()
            => await _termRepository.GetIdFirst();

        public async Task<bool> GetTermByName(string name)
            => await _termRepository.GetTermByName(name);

        public async Task<List<Term>> GetTermByIdList(Guid termId)
            => await _termRepository.GetTermByIdList(termId);

        public async Task<Term> GetFirst()
            => await _termRepository.GetFirst();

        public async Task<object> GetTermsByStudentIdSelect2ClientSide(Guid studentId)
            => await _termRepository.GetTermsByStudentIdSelect2ClientSide(studentId);
        public async Task<Term> GetByStatus(int status)
            => await _termRepository.GetByStatus(status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _termRepository.GetTermsDatatable(sentParameters, search);

        public async Task<bool> IsInvalidDateRange(Term term)
        {
            return await _termRepository.IsInvalidDateRange(term);
        }

        public async Task<object> GetPastTermsSelect2ClientSide()
            => await _termRepository.GetPastTermsSelect2ClientSide();

        public async Task<IEnumerable<Select2Structs.Result>> GetLastTermsSelect2ClientSide(int? yearDifference)
            => await _termRepository.GetLastTermsSelect2ClientSide(yearDifference);

        public async Task<int> GetTotalStudentsEnrolled(Guid termId, Guid? campusId = null, ClaimsPrincipal user = null)
            => await _termRepository.GetTotalStudentsEnrolled(termId, campusId, user);

        public async Task<IEnumerable<Term>> GetTermsByStudentSectionsAndAcademicHistories(Guid studentId)
            => await _termRepository.GetTermsByStudentSectionsAndAcademicHistories(studentId);
    }
}