using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.INTRANET.Areas.Student.Controllers;

namespace AKDEMIC.INTRANET.Areas.AcademicAssistant.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_ASSISTANT)]
    [Area("AcademicAssistant")]
    [Route("academic-assistant/academic-assistant")]
    public class AcademicAssistantController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public IConverter _dinkConverter { get; }
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AcademicAssistantController(AkdemicContext context, IOptions<CloudStorageCredentials> storageCredentials,
           IConverter dinkConverter, IViewRenderService viewRenderService, IWebHostEnvironment environment) :
           base(context)
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _storageCredentials = storageCredentials;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de secciones aperturadas
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <returns>Listado de secciones</returns>
        [Route("sections")]
        public async Task<IActionResult> GetSections(Guid fid, Guid cid)
        {
            var query = _context.Sections.Include(x => x.CourseTerm.Course).Include(x => x.CourseTerm.Term).Where(x => x.CourseTerm.Term.Status == 1).AsQueryable();
            if (fid != Guid.Empty)
                query = query.Where(q => q.CourseTerm.Course.Career.FacultyId == fid);
            if (cid != Guid.Empty)
                query = query.Where(q => q.CourseTerm.Course.CareerId == cid);
            var result = await query.Select(x => new
            {
                x.Id,
                x.Code,
                Name = $"{x.CourseTerm.Course.Code} - {x.CourseTerm.Course.Name}",
                Career = x.CourseTerm.Course.Career == null ? "-" : x.CourseTerm.Course.Career.Name,
                Faculty = x.CourseTerm.Course.Career == null ? "-" : x.CourseTerm.Course.Career.Faculty == null ? "-" : x.CourseTerm.Course.Career.Faculty.Name
            }).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle de la sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("sectionScores/{sid}")]
        public IActionResult SectionScores(Guid sid)
        {
            return View(sid);
        }

        /// <summary>
        /// Obtiene el reporte de notas de la sección
        /// </summary>
        /// <param name="evaluations">Evaluaciones</param>
        /// <param name="grades">Notas</param>
        /// <param name="userId"></param>
        /// <returns>Identificador del estudiante</returns>
        private List<string> GetScores(List<Evaluation> evaluations, List<Grade> grades, String userId)
        {
            List<string> result = new List<string>();
            for (var j = 0; j < evaluations.Count; j++)
            {
                var grade = grades.FirstOrDefault(z => z.EvaluationId == evaluations[j].Id && z.StudentSection.Student.UserId == userId);
                if (grade == null)
                    result.Add("-");
                else
                    result.Add(grade.Value.ToString());
            }
            return result;
        }

        /// <summary>
        /// Obtiene el listado de notas de la sección
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <returns>Listado de notas</returns>
        [Route("scores/{sid}")]
        public JsonResult Scores(Guid sid)
        {
            var section = _context.Sections.FirstOrDefault(x => x.Id == sid);
            var evaluations = _context.Evaluations.Include(x => x.CourseTerm).Where(x => x.CourseTermId == section.CourseTermId).OrderBy(x => x.Percentage).ToList();
            var grades = _context.Grades.Include(x => x.StudentSection.Student).Include(x => x.Evaluation).Where(x => x.StudentSection.SectionId == sid).OrderBy(x => x.Evaluation.Percentage).ToList();

            var result = _context.StudentSections.Include(x => x.Student.User).Where(x => x.SectionId == sid).OrderBy(x => x.Student.User.FullName).Select(x => new
            {
                Estudiante = x.Student.User.FullName,
                Scores = GetScores(evaluations, grades, x.Student.UserId)
            }).ToList();
            return Json(new { Scores = result, Evaluations = evaluations.Select(x => x.Name).ToList() });
        }

        /// <summary>
        /// Método para enviar notificaciones al docente
        /// </summary>
        /// <param name="sid">identificador de la sección</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("notification/{sid}")]
        public async Task<IActionResult> SendNotification(Guid sid)
        {
            var section = await _context.Sections.Include(x => x.CourseTerm.Course).FirstAsync(x => x.Id == sid);
            var teacher = await _context.TeacherSections.Include(x => x.Teacher.User).FirstAsync(x => x.SectionId == sid);

            await SendNotificationToUsers(new string[] { teacher.Teacher.User.UserName }, "Estimado/a " + teacher.Teacher.User.FullName + ", se le informa que la sección " + section.Code + " del curso " + section.CourseTerm.Course.Code + " - " + section.CourseTerm.Course.Name + " tiene evaluaciones sin calificar. ", Url.GenerateLink(nameof(GradeController.Index), "Grade", Request.Scheme, new { area = "Teacher" }), "Urgente", Helpers.ConstantHelpers.NOTIFICATIONS.COLORS.BRAND);
            return Ok();
        }
    }
}
