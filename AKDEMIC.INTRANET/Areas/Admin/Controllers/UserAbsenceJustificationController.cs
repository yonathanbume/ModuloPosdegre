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
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/justificacion-inasistencias/personal")]
    public class UserAbsenceJustificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IUserAbsenceJustificationService _userAbsenceJustificationService;
        private readonly IWorkingDayService _workingDayService;

        public UserAbsenceJustificationController(IUserService userService,
            ITermService termService,
            IUserAbsenceJustificationService userAbsenceJustificationService,
            IWorkingDayService workingDayService,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(userService, termService)
        {
            _storageCredentials = storageCredentials;
            _workingDayService = workingDayService;
            _userAbsenceJustificationService = userAbsenceJustificationService;
        }

        /// <summary>
        /// Vista donde se listan las solicitudes de justificación de inasistencias
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de justificación de inasistencias del personal
        /// </summary>
        /// <returns>Objeto que contiene el listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetUserAbsenceJustifications()
        {
            var term = await GetActiveTerm();
            var data = await _userAbsenceJustificationService.GetAll(term.Id);
            var result = data.Select(x => new
            {
                id = x.Id,
                user = x.WorkingDay.User.FullName,
                date = x.WorkingDay.RegisterDate.ToDateFormat(),
                registerDate = x.RegisterDate.ToLocalDateTimeFormat(),
                status = x.Status
            }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>objeto que contiene los datos de la solicitud</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetUserAbsenceJustification(Guid id)
        {
            var userAbsenceJustification = await _userAbsenceJustificationService.Get(id);
            var result = new
            {
                id = userAbsenceJustification.Id,
                user = userAbsenceJustification.WorkingDay.User.FullName,
                date = userAbsenceJustification.WorkingDay.RegisterDate.ToDateFormat(),
                registerDate = userAbsenceJustification.RegisterDate.ToLocalDateTimeFormat(),
                status = userAbsenceJustification.Status,
                justification = userAbsenceJustification.Justification,
                file = !string.IsNullOrEmpty(userAbsenceJustification.File)
            };
            return Ok(result);
        }

        /// <summary>
        /// Método para descargar el archivo adjunto de la solicitud
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Archivo</returns>
        [HttpGet("{id}/archivo/descargar")]
        public async Task DownloadUserFile(Guid id)
        {
            var absenceJustification = await _userAbsenceJustificationService.Get(id);
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);
                var fileId = absenceJustification.File.Split('/').Last();
                await storage.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.USER_ABSENCE_JUSTIFICATION_FILE, fileId);

                // Download file
                var fileName = fileId.Contains(".") ? fileId : $"{fileId}.pdf";
                var text = $"attachment;filename=\"{fileName}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                mem.CopyTo(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Método para aceptar o denegar la solicitud
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <param name="approved">¿Aprobado?</param>
        /// <returns>Código de estdo HTTP</returns>
        [HttpPost("post")]
        public async Task<IActionResult> ChangeUserState(Guid id, bool approved)
        {
            var absenceJustification = await _userAbsenceJustificationService.Get(id);
            if (!absenceJustification.Status.Equals(ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS))
                return BadRequest("La solicitud ya fue aprobada o desaprobada anteriormente.");
            if (approved)
            {
                absenceJustification.Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED;
            }
            else
            {
                absenceJustification.Status = ConstantHelpers.USER_PROCEDURES.STATUS.NOT_APPLICABLE;
            }
            await _userAbsenceJustificationService.Update(absenceJustification);
            return Ok();
        }
    }
}
