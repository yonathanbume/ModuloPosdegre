﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ITermService
    {
        Task InsertTerm(Term term);
        Task UpdateTerm(Term term);
        Task DeleteTerm(Term term);
        Task<bool> ExistActiveTerm();
        Task<bool> IsActiveTermInTime();
        Task<Term> GetActiveTerm();
        Task<Term> GetLastTerm(bool? includeSummer = true);
        Task<Term> GetTermByDateTime(DateTime dateTime);
        Task<IEnumerable<Term>> GetAll();
        Task<IEnumerable<int>> GetTermYears();
        Task<IEnumerable<Term>> GetTermsByStudentSections(Guid studentId);
        Task<IEnumerable<Term>> GetTermsByStudentSectionsAndAcademicHistories(Guid studentId);
        Task<Term> Get(Guid id);
        Task<Select2Structs.ResponseParameters> GetTermsSelect2(Select2Structs.RequestParameters requestParameters);
        Task<IEnumerable<Select2Structs.Result>> GetTermsSelect2ClientSide(bool OnlyTermWithInstitutionalWelfareSurvey = false);
        Task<Term> GetReservation();
        Task<object> GetSelect2Terms(int? year = null);
        Task<Select2Structs.ResponseParameters> GetTermsByCourseIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? courseId = null, string searchValue = null);
        Task<object> GetTermsFinishedSelect2ClientSide();
        Task<object> GetTermsFinishedSelect2ClientSideByClassStartDate();
        Task<object> GetTermsByStudentIdSelect2ClientSide(Guid studentId);
        Task Update(Term term);
        Task<object> GetTermsWithStatus(int status);
        Task<List<Term>> GetUserCodePrefixByTermId(Guid termId);
        Task<List<Term>> GetTermsStatus(int status);
        Task<Guid> GetIdFirst();
        Task<DataTablesStructs.ReturnedData<object>> GetTermsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<bool> GetTermByName(string name);
        Task<List<Term>> GetTermByIdList(Guid termId);
        Task<Term> GetFirst();
        Task<Term> GetByStatus(int status);
        Task<object> GetPastTermsSelect2ClientSide();
        Task<bool> IsInvalidDateRange(Term term);
        Task<IEnumerable<Select2Structs.Result>> GetLastTermsSelect2ClientSide(int? yearDifference);
        Task<int> GetTotalStudentsEnrolled(Guid termId, Guid? campusId = null, ClaimsPrincipal user = null);
    }
}