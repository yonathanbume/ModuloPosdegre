using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Student.Models.AbsenceJustificationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/justificacion-inasistencias")]
    public class AbsenceJustificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentAbsenceJustificationService _studentAbsenceJustificationService;
        private readonly IClassStudentService _classStudentService;
        private readonly IDataTablesService _dataTablesService;

        public AbsenceJustificationController(IUserService userService,
            ITermService termService,
            IClassStudentService classStudentService,
            IStudentAbsenceJustificationService studentAbsenceJustificationService, IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials) : base(userService, termService)
        {
            _storageCredentials = storageCredentials;
            _studentAbsenceJustificationService = studentAbsenceJustificationService;
            _classStudentService = classStudentService;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista donde se generan las solicitudes de justificación de inasistencia
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de justificación de inasistencia creadas por el alumno logueado
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de solicitudes</returns>
        [Route("get")]
        public async Task<IActionResult> GetAbsenceJustifications(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentAbsenceJustificationService.GetDatatable(sentParameters, User, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la solicitud de justificación de inasistencia
        /// </summary>
        /// <param name="id">identificador de la solicitud</param>
        /// <returns>Datos de la solicitud</returns>
        [Route("{id}/get")]
        public async Task<IActionResult> GetJustification(Guid id)
        {
            var studentAbsenceJustification = await _studentAbsenceJustificationService.Get(id);
            var result = new
            {
                id = studentAbsenceJustification.Id,
                course = studentAbsenceJustification.ClassStudent.Class.Section.CourseTerm.Course.FullName,
                section = studentAbsenceJustification.ClassStudent.Class.Section.Code,
                date = "Clase del " + studentAbsenceJustification.ClassStudent.Class.StartTime.ToLocalDateFormat(),
                registerDate = studentAbsenceJustification.RegisterDate.ToLocalDateTimeFormat(),
                status = studentAbsenceJustification.Status,
                justification = studentAbsenceJustification.Justification,
                file = !string.IsNullOrEmpty(studentAbsenceJustification.File)
            };
            return Ok(result);
        }

        /// <summary>
        /// Método para descargar el archivo adjunto de la solicitud de justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la solicitud de justificación de inasistencia</param>
        [HttpGet("{id}/archivo/descargar")]
        public async Task DownloadFile(Guid id)
        {
            var absenceJustification = await _studentAbsenceJustificationService.Get(id);
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDownload(mem, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.STUDENT_ABSENCE_JUSTIFICATION_FILE, absenceJustification.File);

                // Download file
                var fileName = Path.GetFileName(absenceJustification.File);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Método para registrar una solicitud de justificación de inasistencia
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la justificación de inasistencia</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> Create(AbsenceJustificationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Revise la información ingresada");

            var classStudent = await _classStudentService.Get(model.ClassStudentId);
            if (!classStudent.IsAbsent) return BadRequest("No presenta inasistencia en la clase seleccionada");

            if (await _studentAbsenceJustificationService.Any(model.ClassStudentId)) return BadRequest("Ya realizó una solicitud para la misma inasistencia");

            //if (model.File?.Length > 1024 * 1024 * 20) return BadRequest("El archivo no debe pesar más de 20MB.");

            var absenceJustification = new StudentAbsenceJustification
            {
                ClassStudentId = model.ClassStudentId,
                Justification = model.Justification,
                Status = CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.REQUESTED,
                RegisterDate = DateTime.UtcNow
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
        /// Mëtodo para eliminar una solicitud de justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la justificación de inasistencia</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var absenceJustifiction = await _studentAbsenceJustificationService.Get(id);

            if (absenceJustifiction.Status != CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.REQUESTED)
                return BadRequest("No puede eliminar una solicitud ya atendida");

            if (!string.IsNullOrEmpty(absenceJustifiction.File))
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(absenceJustifiction.File, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.STUDENT_ABSENCE_JUSTIFICATION_FILE);
            }

            await _studentAbsenceJustificationService.Delete(absenceJustifiction);
            return Ok();
        }
    }
}
