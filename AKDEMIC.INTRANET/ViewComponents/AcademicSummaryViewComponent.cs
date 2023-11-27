using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSummary")]
    public class AcademicSummaryViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly AkdemicContext _context;

        public AcademicSummaryViewComponent(
            IUserService userService,
            ITermService termService,
            IStudentService studentService,
            IStudentSectionService studentSectionService,
            AkdemicContext context,
            IAcademicSummariesService academicSummariesService)
        {
            _userService = userService;
            _termService = termService;
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _context = context;
            _academicSummariesService = academicSummariesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? studentId = null)
        {
            var student = studentId.HasValue
                ? await _studentService.GetWithIncludes(studentId.Value)
                : await _studentService.GetStudentByUser(_userService.GetUserIdByClaim(UserClaimsPrincipal));

            var terms = (await _termService.GetTermsByStudentSections(student.Id)).ToList();
            if (!terms.Any(y => y.Status == ConstantHelpers.TERM_STATES.ACTIVE))
            {
                var term = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                    && (x.Type == ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.SPECIAL || x.Type == ConstantHelpers.AcademicHistory.Types.REEVALUATION || x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION)
                )
                    .Select(x => x.Term).FirstOrDefaultAsync();

                if (term != null)
                    terms.Add(term);
            }

            terms = terms.OrderByDescending(x => x.Year).ThenBy(x => x.Number).ToList();

            //var cumulative = 0.00M;
            //var credits = 0.0M;

            var model = new List<StudentTermViewModel>();

            var summaries = await _academicSummariesService.GetAllByStudent(student.Id);
            foreach (var t in terms)
            {
                //var ac = await _academicSummariesService.GetByStudentAndTerm(student.Id, t.Id); // Si es activo será NULL
                var ac = summaries.FirstOrDefault(x => x.TermId == t.Id); // Si es activo será NULL

                //if (ac != null)
                //    cumulative = cumulative == 0 ? ac.WeightedAverageGrade
                //        : (credits + ac.TotalCredits != 0
                //            ? ((cumulative * credits) + (ac.WeightedAverageGrade * ac.TotalCredits)) / (credits + ac.TotalCredits) : cumulative);

                var studentTerm = new StudentTermViewModel
                {
                    TermId = ac?.TermId ?? t.Id,
                    TermName = ac?.Term?.Name ?? t.Name,
                    CareerName = ac?.Career?.Name ?? student.Career.Name,
                };

                studentTerm.AcademicSummary = new SummaryViewModel
                {
                    TotalCredits = (ac?.TotalCredits
                        ?? (await _studentSectionService.GetAll(student.Id, t.Id))
                        .Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Sum(x => x.Section.CourseTerm.Course.Credits)).ToString("0.0"),
                    ApprovedCredits = ac?.ApprovedCredits ?? 0.0M,
                    WeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageGrade, 2).ToString(),
                    //CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(cumulative, 2).ToString(),
                    CumulativeWeightedFinalGrade = !ac?.TermHasFinished ?? true ? "-" : Math.Round(ac.WeightedAverageCumulative, 2).ToString(),
                    Order = !ac?.TermHasFinished ?? true ? "-" : ac.MeritOrder.ToString(),
                    MeritOrder = !ac?.TermHasFinished ?? true ? "-" : ac.MeritType >= 0 ? ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                    Observations = !ac?.TermHasFinished ?? true ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                    StudentAcademicYear = ac?.StudentAcademicYear ?? student.CurrentAcademicYear,
                    Condition = ac != null ? ConstantHelpers.Student.Condition.VALUES[ac.StudentCondition] : "---",
                    Status = ac != null ? ConstantHelpers.Student.States.VALUES[ac.StudentStatus] : "---",
                };

                //credits += ac?.TotalCredits ?? 0;
                model.Add(studentTerm);
            }

            return View(model.OrderByDescending(m => m.TermName).ToList());
        }
    }
}
