using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSummaryReport")]
    public class AcademicSummaryReportViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicYearCourseService _academicYearCourseService;

        public AcademicSummaryReportViewComponent(IUserService userService, ITermService termService,
            IStudentService studentService, IStudentSectionService studentSectionService,
            IAcademicYearCourseService academicYearCourseService)
        {
            _userService = userService;
            _termService = termService;
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _academicYearCourseService = academicYearCourseService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? studentId = null)
        {
            var student = studentId.HasValue
                ? await _studentService.Get(studentId.Value)
                : await _studentService.GetStudentByUser(_userService.GetUserIdByClaim(UserClaimsPrincipal));
            var academicSummaries = await _academicSummariesService.GetAllByStudent(student.Id);
            var term = await _termService.GetActiveTerm();
            var studentTerms = new List<StudentTermReportViewModel>();
            var cumulative = 0m;
            var credits = 0.0M;

            foreach (var ac in academicSummaries)
            {
                if (cumulative == 0)
                    cumulative = ac.WeightedAverageGrade;
                else if ((credits + ac.TotalCredits) != 0)
                    cumulative = ((cumulative * credits) + (ac.WeightedAverageGrade * ac.TotalCredits)) / (credits + ac.TotalCredits);

                var studentTerm = new StudentTermReportViewModel()
                {
                    CareerName = ac.Career.Name,
                    TermName = ac.Term.Name,
                    StudentSections =  (await Task.WhenAll((await _studentSectionService.GetAll(student.Id, ac.TermId))
                                        .Select(async x => new StudentSectionsViewModel
                                        {
                                            CourseName = x.Section.CourseTerm.Course.FullName,
                                            Credits = x.Section.CourseTerm.Course.Credits,
                                            Try = x.Try,
                                            Status = x.Status,
                                            FinalGrade = x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS ? "-" : (x.Status == CORE.Helpers.ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "Retirado" : x.FinalGrade.ToString()),
                                            AcademicYear = await _academicYearCourseService.GetLevelByCourseAndCurriculum(x.Section.CourseTerm.CourseId, student.CurriculumId) ?? 0
                                        }).ToList())).ToList(),
                    AcademicSummary = new SummaryViewModel
                    {
                        TotalCredits = ac.TotalCredits.ToString("0.0"),
                        ApprovedCredits = ac.ApprovedCredits,
                        WeightedFinalGrade = !ac.TermHasFinished ? "-" : Math.Round(ac.WeightedAverageGrade, 2).ToString(),
                        CumulativeWeightedFinalGrade = !ac.TermHasFinished ? "-" : Math.Round(cumulative, 2).ToString("0.0"),
                        Order = !ac.TermHasFinished ? "-" : ac.MeritOrder.ToString(),
                        MeritOrder = !ac.TermHasFinished ? "-" : ac.MeritType >= 0 ? CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.VALUES[ac.MeritType] : "",
                        Observations = !ac.TermHasFinished ? "---" : string.IsNullOrEmpty(ac.Observations) ? "---" : ac.Observations,
                        StudentAcademicYear = ac.StudentAcademicYear
                    }
                };

                studentTerms.Add(studentTerm);
                credits += ac.TotalCredits;
            }

            var model = new RecordReportViewModel()
            {
                FacultyName = student.Career.Faculty.Name,
                CareerName = student.Career.Name,
                StudentCode = student.User.UserName,
                StudentFullName = student.User.FullName,
                CurrentTerm = term.Name,
                Terms = studentTerms
            };

            return View(model);
        }
    }
}
