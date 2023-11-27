using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Student.Models.AbsenceJustificationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/justificacion-inasistencias-alumnos")]
    public class StudentAbsenceJustificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentAbsenceJustificationService _studentAbsenceJustificationService;
        private readonly IClassStudentService _classStudentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentAbsenceJustificationController(IUserService userService,
            IDataTablesService dataTablesService,
            ITermService termService,
            IStudentAbsenceJustificationService studentAbsenceJustificationService,
            IClassStudentService classStudentService, IStudentSectionService studentSectionService,
            IOptions<CloudStorageCredentials> storageCredentials, UserManager<ApplicationUser> userManager) : base(userService, termService)
        {
            _storageCredentials = storageCredentials;
            _studentAbsenceJustificationService = studentAbsenceJustificationService;
            _classStudentService = classStudentService;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _studentSectionService = studentSectionService;
        }

        public IActionResult Index() => View();

        /// <summary>
        /// Vista principal donde se gestiona las justificaciones de inasistencias de los alumnos
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetJustifications(byte? status, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentAbsenceJustificationService.GetDatatable(sentParameters, User, search, status);
            return Ok(result);
        }

        /// <summary>
        /// Método para aceptar o rechazar la solicitud de justificación de inasistencia.
        /// </summary>
        /// <param name="Id">Identificador de la justificación de inasistencia</param>
        /// <param name="Observation">Observación</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("validar")]
        public async Task<IActionResult> ValidateJustification(Guid Id, string Observation)
        {
            var absenceJustification = await _studentAbsenceJustificationService.Get(Id);
            if (absenceJustification.Status != CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.REQUESTED)
                return BadRequest("La solicitud ya fue atendida");

            var userId = _userManager.GetUserId(User);
            absenceJustification.TeacherId = userId;
            if (string.IsNullOrEmpty(Observation))
            {
                absenceJustification.Status = CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.TEACHER_APPROVED;
            }
            else
            {
                absenceJustification.Status = CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.OBSERVED;
                absenceJustification.Observation = Observation;
            }
            await _studentAbsenceJustificationService.Update(absenceJustification);
            return Ok();
        }

        /// <summary>
        /// Método para registrar una solicitud de justificación de inasistencia
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva justificación de inasistencia</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> Create(AbsenceJustificationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Revise la información ingresada");

            var classStudent = await _classStudentService.Get(model.ClassStudentId);
            if (!classStudent.IsAbsent) return BadRequest("El alumno no presenta inasistencia en la clase seleccionada");

            if (await _studentAbsenceJustificationService.Any(model.ClassStudentId)) return BadRequest("Ya se realizó una solicitud para la misma inasistencia");
            var userId = _userManager.GetUserId(User);

            var absenceJustification = new StudentAbsenceJustification
            {
                ClassStudentId = model.ClassStudentId,
                Justification = model.Justification,
                Status = string.IsNullOrEmpty(model.Observation) ? CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.TEACHER_APPROVED
                    : CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.OBSERVED,
                RegisterDate = DateTime.UtcNow,
                TeacherId = userId,
                Observation = model.Observation
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                var fileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.STUDENT_ABSENCE_JUSTIFICATION_FILE,
                    Path.GetExtension(model.File.FileName),
                    CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                absenceJustification.File = fileUrl;
            }

            await _studentAbsenceJustificationService.Insert(absenceJustification);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de alumnos matriculados en una sección
        /// </summary>
        /// <param name="id">Identificador de la sección</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("alumnos/{id}/get")]
        public async Task<IActionResult> GetSectionStudents(Guid id)
        {
            var students = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(id);
            var result = new
            {
                Items = students.Select(x => new
                {
                    id = x.Student.UserId,
                    text = x.Student.User.FullName
                }).OrderBy(x => x.text).ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de faltas del alumno
        /// </summary>
        /// <param name="userId">Identificador del docente</param>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Listado de faltas</returns>
        [HttpGet("alumno/{userId}/inasistencias/{sectionId}/get")]
        public async Task<IActionResult> GetStudentAbsences(string userId, Guid sectionId)
        {
            var result = await _classStudentService.GetClassStudentSelect2ClientSide(userId, sectionId, true);
            return Ok(new { items = result });
        }
    }
}
