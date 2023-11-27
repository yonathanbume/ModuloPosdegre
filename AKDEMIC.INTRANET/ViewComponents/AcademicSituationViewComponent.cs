using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSituationViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSituation")]
    public class AcademicSituationViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ICurriculumService _curriculumService;

        public AcademicSituationViewComponent(IUserService userService, IStudentService studentService, ICurriculumService curriculumService)
        {
            _userService = userService;
            _studentService = studentService;
            _curriculumService = curriculumService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? studentId = null)
        {
            var student = studentId.HasValue
                ? await _studentService.Get(studentId.Value)
                : await _studentService.GetStudentByUser(_userService.GetUserIdByClaim(UserClaimsPrincipal));
            var academicYears = await _curriculumService.GetAllAcademicYears(student.CurriculumId);

            var model = academicYears
                .Select(x => new AcademicYearViewModel()
                {
                    Id = x,
                    Number = x
                }).ToList();
            return View(model);
        }
    }
}
