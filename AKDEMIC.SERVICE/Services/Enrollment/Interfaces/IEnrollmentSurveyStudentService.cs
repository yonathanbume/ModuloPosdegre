using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentSurveyStudentService
    {
        Task Insert(EnrollmentSurveyStudent entity);
        Task<bool> HasSurveyCompleted(Guid studentId, Guid termId);
        Task<ChartJSTemplate> GetSurveyChart(Guid? termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user = null);
        Task<ChartPieJSTemplate> GetSurveyStudentsByCareerChart(Guid? termId, ClaimsPrincipal user = null);
        Task<DetailedReportTemplate> GetDetailedReport(Guid? termId);
    }
}
