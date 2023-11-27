using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentSurveyStudentService : IEnrollmentSurveyStudentService
    {
        private readonly IEnrollmentSurveyStudentRepository _enrollmentSurveyStudentRepository;

        public EnrollmentSurveyStudentService(IEnrollmentSurveyStudentRepository enrollmentSurveyStudentRepository)
        {
            _enrollmentSurveyStudentRepository = enrollmentSurveyStudentRepository;
        }

        public async Task<DetailedReportTemplate> GetDetailedReport(Guid? termId)
            => await _enrollmentSurveyStudentRepository.GetDetailedReport(termId);

        public async Task<ChartJSTemplate> GetSurveyChart(Guid? termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user = null)
            => await _enrollmentSurveyStudentRepository.GetSurveyChart(termId, facultyId, careerId, user);

        public async Task<ChartPieJSTemplate> GetSurveyStudentsByCareerChart(Guid? termId, ClaimsPrincipal user = null)
            => await _enrollmentSurveyStudentRepository.GetSurveyStudentsByCareerChart(termId, user);

        public async Task<bool> HasSurveyCompleted(Guid studentId, Guid termId)
            => await _enrollmentSurveyStudentRepository.HasSurveyCompleted(studentId, termId);

        public async Task Insert(EnrollmentSurveyStudent entity)
            => await _enrollmentSurveyStudentRepository.Insert(entity);
    }
}
