using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularActivitiesViewModel;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/actividades-extracurriculares-alumnos")]
    public class ExtracurricularActivitiesStudentController : BaseController
    {
        private readonly IExtracurricularActivityStudentService _extracurricularActivityStudentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IUserService _userService;
        private readonly ICloudStorageService _cloudStorageService;

        public ExtracurricularActivitiesStudentController(
            IExtracurricularActivityStudentService extracurricularActivityStudentService,
            IDataTablesService dataTablesService,
            IUserService userService,
            ICloudStorageService cloudStorageService
            )
            : base()
        {
            _extracurricularActivityStudentService = extracurricularActivityStudentService;
            _dataTablesService = dataTablesService;
            _userService = userService;
            _cloudStorageService = cloudStorageService;
        }

        /// <summary>
        /// Vista donde se gestionan las actividades extracurriculares de estudiantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _extracurricularActivityStudentService.GetDatatable(parameters, null, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de actividades extracurriculares de estudiantes
        /// </summary>
        /// <returns>Objeto que contiene el listado de actividades extracurriculares de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetRegisteredActivities()
        {
            var students = await _extracurricularActivityStudentService.GetAll();
            return Ok(students.Select(x => new
            {
                id = x.Id,
                credits = x.ExtracurricularActivity.Credits,
                activityCode = x.ExtracurricularActivity.Code,
                activity = x.ExtracurricularActivity.Name,
                name = x.Student.User.FullName,
                date = x.RegisterDateText
            }).ToList());
        }

        /// <summary>
        /// Método para agregar una actividad extracurricular
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva actividad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("add")]
        public async Task<IActionResult> CreateExtracurricularActivityStudent([Bind(Prefix = "Add")] ExtracurricularActivityStudentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Verifique los datos ingresados.");

            var extracurricularActivityStudent = new ExtracurricularActivityStudent
            {
                StudentId = model.StudentId,
                ExtracurricularActivityId = model.ExtracurricularActivityId,
                RegisterDate = DateTime.UtcNow,
                Grade = model.Grade,
                Resolution = model.Resolution,
            };

            if (!string.IsNullOrEmpty(model.EvaluationReportDate))
                extracurricularActivityStudent.EvaluationReportDate = ConvertHelpers.DatepickerToUtcDateTime(model.EvaluationReportDate);

            if (model.File != null)
            {
                extracurricularActivityStudent.UrlFile = await _cloudStorageService.UploadFile(
                    model.File.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.EXTRACURRICULAR_ACTIVITIES_STUDENTS,
                    Path.GetExtension(model.File.FileName)
                    );
            }

            await _extracurricularActivityStudentService.Insert(extracurricularActivityStudent);
            return Ok();
        }

        /// <summary>
        /// Método para editar una actividad extracurricular
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la actividad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("edit")]
        public async Task<IActionResult> EditExtracurricularActivityStudent([Bind(Prefix = "Edit")] ExtracurricularActivityStudentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Verifique los datos ingresados.");
            var extracurricularActivityStudent = await _extracurricularActivityStudentService.Get(model.Id.Value);
            if (extracurricularActivityStudent == null)
                return BadRequest("Actividad extracurricular no encontrada.");

            extracurricularActivityStudent.StudentId = model.StudentId;
            extracurricularActivityStudent.ExtracurricularActivityId = model.ExtracurricularActivityId;
            extracurricularActivityStudent.Grade = model.Grade;
            extracurricularActivityStudent.Resolution = model.Resolution;

            if (!string.IsNullOrEmpty(model.EvaluationReportDate))
                extracurricularActivityStudent.EvaluationReportDate = ConvertHelpers.DatepickerToUtcDateTime(model.EvaluationReportDate);

            if (model.File != null)
            {
                extracurricularActivityStudent.UrlFile = await _cloudStorageService.UploadFile(
                    model.File.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.EXTRACURRICULAR_ACTIVITIES_STUDENTS,
                    Path.GetExtension(model.File.FileName)
                    );
            }

            await _extracurricularActivityStudentService.Update(extracurricularActivityStudent);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar una actividad extracurricular
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la actividad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> DeleteExtracurricularActivityStudent(Guid id)
        {
            await _extracurricularActivityStudentService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene los datos de la actividad extracurricular
        /// </summary>
        /// <param name="id">Identificador de la actividad extracurricular</param>
        /// <returns>objeto que contiene los datos de la actividad extracurricular</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetActivityStudent(Guid id)
        {
            var extracurricularActivityStudent = await _extracurricularActivityStudentService.Get(id);
            var user = await _userService.GetUserByStudent(extracurricularActivityStudent.StudentId);

            return Ok(new
            {
                id = extracurricularActivityStudent.Id,
                extracurricularActivityId = extracurricularActivityStudent.ExtracurricularActivityId,
                studentId = extracurricularActivityStudent.StudentId,
                studentName = user.FullName,
                extracurricularActivityStudent.UrlFile,
                extracurricularActivityStudent.Resolution,
                extracurricularActivityStudent.Grade,
                evaluationReportDate =
                extracurricularActivityStudent.EvaluationReportDate.HasValue ?
                extracurricularActivityStudent.EvaluationReportDate.Value.ToLocalDateFormat() : null,

            });
        }
    }
}
