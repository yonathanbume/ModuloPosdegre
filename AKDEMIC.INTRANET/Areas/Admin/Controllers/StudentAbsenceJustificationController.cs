using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + ","
        + CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.TEACHERS)]
    [Area("Admin")]
    [Route("admin/justificacion-inasistencias/alumnos")]
    public class StudentAbsenceJustificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentAbsenceJustificationService _studentAbsenceJustificationService;
        private readonly IClassStudentService _classStudentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IClassService _classService;

        public StudentAbsenceJustificationController(IUserService userService,
            IDataTablesService dataTablesService,
            ITermService termService,
            IClassService classService,
            IStudentAbsenceJustificationService studentAbsenceJustificationService,
            IClassStudentService classStudentService,
            IOptions<CloudStorageCredentials> storageCredentials) : base(userService, termService)
        {
            _storageCredentials = storageCredentials;
            _studentAbsenceJustificationService = studentAbsenceJustificationService;
            _classStudentService = classStudentService;
            _dataTablesService = dataTablesService;
            _classService = classService;
        }

        /// <summary>
        /// Vista donde se listan las solicitudes realizadas por estudiantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de justificación de inasistencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="status">Estado</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetJustifications(Guid? termId, byte? status, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentAbsenceJustificationService.GetAdminDatatable(sentParameters, User, search, termId, status);
            return Ok(result);
        }

        /// <summary>
        /// Método para descargar el archivo adjunto en la justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la justificación de inasistencia</param>
        /// <returns>Archivo</returns>
        [HttpGet("{id}/archivo/descargar")]
        public async Task DownloadStudentFile(Guid id)
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
        /// Método para aceptar o rechazar la justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la solicitud de justificación de inasistencia</param>
        /// <param name="approved">¿Aceptada?</param>
        /// <returns></returns>
        [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
        [HttpPost("post")]
        public async Task<IActionResult> ChangeStudentState(Guid id, bool approved)
        {
            var absenceJustification = await _studentAbsenceJustificationService.Get(id);

            if (absenceJustification.Status == CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.ACCEPTED || absenceJustification.Status == CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.DENIED)
                return BadRequest("La solicitud ya fue atendida");

            if (approved)
            {
                absenceJustification.Status = CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.ACCEPTED;
                var absenceToModify = await _classStudentService.Get(absenceJustification.ClassStudentId);
                var @class = await _classService.Get(absenceToModify.ClassId);
                absenceToModify.IsAbsent = false;
                await _classStudentService.Update(absenceToModify);
                await _classStudentService.AssignDPI(@class.SectionId);
            }
            else
            {
                absenceJustification.Status = CORE.Helpers.ConstantHelpers.Intranet.StudentAbsenceJustification.Status.DENIED;
            }
            await _studentAbsenceJustificationService.Update(absenceJustification);
            return Ok();
        }
    }
}
