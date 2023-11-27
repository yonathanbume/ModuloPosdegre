using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSituationViewModels;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "StudentSituation")]
    public class StudentSituationViewComponent : ViewComponent
    {
        private readonly ITermService _termService;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly ICurriculumService _curriculumService;
        private readonly IAcademicHistoryService _academicHistoryService;

        public StudentSituationViewComponent(IUserService userService, 
            ITermService termService, 
            IStudentService studentService,
            IAcademicYearCourseService academicYearCourseService,
            IAcademicSummariesService academicSummariesService,
            ICurriculumService curriculumService,
            IAcademicHistoryService academicHistoryService)
        {
            _userService = userService;
            _termService = termService;
            _studentService = studentService;
            _academicYearCourseService = academicYearCourseService;
            _academicSummariesService = academicSummariesService;
            _curriculumService = curriculumService;
            _academicHistoryService = academicHistoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? studentId = null)
        {
            var term = await _termService.GetActiveTerm();
            var student = studentId.HasValue
                ? await _studentService.GetStudentWithCareerAndUser(studentId.Value)
                : await _studentService.GetStudentByUser(_userService.GetUserIdByClaim(UserClaimsPrincipal));

            var studentSummaries = await _academicSummariesService.GetAllByStudent(student.Id);
            var approvedCredits = await _studentService.GetApprovedCreditsByStudentId(student.Id);
            var lastStudentTerm = studentSummaries.OrderBy(x => x.Term.StartDate).LastOrDefault();

            var electiveApprovedCredits = await _studentService.GetElectiveApprovedCredits(student.Id);
            var requiredApprovedCredits = await _studentService.GetRequiredApprovedCredits(student.Id);
            
            var curriculum = await _curriculumService.Get(student.CurriculumId);

            var model = new StudentInfoViewModel()
            {
                Id = student.Id,
                AcademicYear = student.CurrentAcademicYear,
                Dni = student.User.Dni,
                FullName = student.User.FullName,
                UserName = student.User.UserName,
                CareerName = student.Career.Name,
                Order = lastStudentTerm?.MeritOrder ?? 0,
                Term = lastStudentTerm?.Term.Name ?? "---",
                Observations = lastStudentTerm == null ? "-" : lastStudentTerm.MeritType > 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[lastStudentTerm.MeritType] : "-",
                Status = student.Status > 0 ? CORE.Helpers.ConstantHelpers.Student.States.VALUES[student.Status] : "-",
                WeightedFinalGrade = lastStudentTerm?.WeightedAverageGrade ?? 0.00M,
                CumulativeWeightedFinalGrade = (studentSummaries.Sum(t => t.TotalCredits) != 0)
                    ? (studentSummaries.Sum(t => t.TotalCredits * t.WeightedAverageGrade) /
                       studentSummaries.Sum(t => t.TotalCredits))
                    : 0,
                ApprovedCredits = approvedCredits,
                RequiredApprovedCredits = requiredApprovedCredits,
                ElectiveApprovedCredits = electiveApprovedCredits,
                ElectiveCredits = curriculum.ElectiveCredits,
                RequiredCredits = curriculum.RequiredCredits
            };
            model.CumulativeWeightedFinalGrade = Math.Round(model.CumulativeWeightedFinalGrade, 2);

            return View(model);
        }
    }
}
