using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/cursos-extracurriculares")]
    public class ExtracurricularCourseController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IPaymentService _paymentService;
        private readonly IExtracurricularCourseGroupService _extracurricularCourseGroupService;
        private readonly IExtracurricularCourseGroupStudentService _extracurricularCourseGroupStudentService;

        public ExtracurricularCourseController(IUserService userService,
            IStudentService studentService,
            IPaymentService paymentService,
            IExtracurricularCourseGroupService extracurricularCourseGroupService,
            IExtracurricularCourseGroupStudentService extracurricularCourseGroupStudentService)
            : base(userService)
        {
            _studentService = studentService;
            _paymentService = paymentService;
            _extracurricularCourseGroupService = extracurricularCourseGroupService;
            _extracurricularCourseGroupStudentService = extracurricularCourseGroupStudentService;
        }

        /// <summary>
        /// Vista donde se muestran los cursos extracurriculares
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de cursos extracurriculares habilitados
        /// </summary>
        /// <returns>Listado de cursos extracurriculares</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExtracurricularCourses()
        {
            var extracurricularCourseGroups = await _extracurricularCourseGroupService.GetAll();
            var result = extracurricularCourseGroups.Select(x => new
            {
                id = x.Id,
                group = $"{x.Code}",
                course = $"{x.ExtracurricularCourse.Code} - {x.ExtracurricularCourse.Name}",
                price = $"S/. {x.ExtracurricularCourse.Price.ToString("0.00")}",
                credits = x.ExtracurricularCourse.Credits
            }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Método para inscribirse en un curso extracurricular
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("enrollment")]
        public async Task<IActionResult> EnrollmentExtracurricularCourse(Guid id)
        {
            var courseGroup = await _extracurricularCourseGroupService.Get(id);
            var userId = GetUserId();
            var student = await _studentService.GetStudentByUser(userId);
            if (await _extracurricularCourseGroupStudentService.AnyStudentInCourse(student.Id, courseGroup.ExtracurricularCourseId))
                return BadRequest("Ya se encuentra inscrito en este curso extracurricular.");

            var courseGroupStudent = new ExtracurricularCourseGroupStudent
            {
                StudentId = student.Id,
                GroupId = id,
                Score = 0,
                Approved = false
            };

            await _extracurricularCourseGroupStudentService.Insert(courseGroupStudent);

            var total = courseGroup.ExtracurricularCourse.Price;
            var subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
            var igv = total - subtotal;

            var payment = new Payment()
            {
                Description = $"Pago por inscripción en el grupo {courseGroup.Code} del curso extracurricular {courseGroup.ExtracurricularCourse.Code} - {courseGroup.ExtracurricularCourse.Name}",
                UserId = userId,
                SubTotal = subtotal,
                IgvAmount = igv,
                Discount = 0.00M,
                Total = total,
                EntityId = courseGroupStudent.Id,
                Type = ConstantHelpers.PAYMENT.TYPES.EXTRACURRICULAR_COURSE_PAYMENT,
                Status = ConstantHelpers.PAYMENT.STATUS.PENDING
            };

            await _paymentService.Insert(payment);
            courseGroupStudent.PaymentId = payment.Id;
            await _extracurricularCourseGroupStudentService.Update(courseGroupStudent);

            return Ok();
        }
    }
}
