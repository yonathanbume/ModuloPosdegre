using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "StudentSummary")]
    public class StudentSummaryViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;

        public StudentSummaryViewComponent(IUserService userService, IStudentService studentService)
        {
            _userService = userService;
            _studentService = studentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? studentId = null)
        {
            var student = studentId.HasValue
                ? await _studentService.GetStudentWithCareerAndUser(studentId.Value)
                : await _studentService.GetStudentByUser(_userService.GetUserIdByClaim(UserClaimsPrincipal));
            var model = new StudentInfoViewModel
            {
                FullName = student.User.FullName,
                UserName = student.User.UserName,
                Dni = student.User.Dni
            };
            return View(model);
        }
    }
}
