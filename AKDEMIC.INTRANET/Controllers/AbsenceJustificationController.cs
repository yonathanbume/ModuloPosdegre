using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.ViewModels.AbsenceJustificationViewModels;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{ 
    [Authorize]
    [Route("justificacion-inasistencias")]
    public class AbsenceJustificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IUserAbsenceJustificationService _userAbsenceJustificationService;
        private readonly IWorkingDayService _workingDayService;

        public AbsenceJustificationController(IUserService userService,
            ITermService termService,
            IWorkingDayService workingDayService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IUserAbsenceJustificationService userAbsenceJustificationService) : base(userService, termService)
        {
            _storageCredentials = storageCredentials;
            _workingDayService = workingDayService;
            _userAbsenceJustificationService = userAbsenceJustificationService;
        }

        /// <summary>
        /// Obtiene la vista inicial de justificación de inasistencias
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la lista de justificación de inasistencias
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetAbsenceJustifications()
        {
            var term = await GetActiveTerm();
            if (term==null)
                term = await _termService.GetLastTerm();
            if (term==null)
                term = new ENTITIES.Models.Enrollment.Term();

            var userId = _userService.GetUserIdByClaim(User);
            var data = await _userAbsenceJustificationService.GetAll(term.Id, userId);
            var result = data.Select(x => new
            {
                id = x.Id,
                date = x.WorkingDay.RegisterDate.ToDateFormat(),
                registerDate = x.RegisterDate.ToLocalDateTimeFormat(),
                status = x.Status,
                canDelete = x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS
            }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la justificación de inasistencias
        /// </summary>
        /// <param name="id">Identificador de justificación de inasistencia</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetAbsenceJustification(Guid id)
        {
            var userAbsenceJustification = await _userAbsenceJustificationService.Get(id);
            var result = new
            {
                id = userAbsenceJustification.Id,
                date = userAbsenceJustification.WorkingDay.RegisterDate.ToDateFormat(),
                registerDate = userAbsenceJustification.RegisterDate.ToLocalDateTimeFormat(),
                status = userAbsenceJustification.Status,
                canDelete = userAbsenceJustification.Status.Equals(ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS),
                justification = userAbsenceJustification.Justification,
                file = !string.IsNullOrEmpty(userAbsenceJustification.File)
            };
            return Ok(result);
        }

        /// <summary>
        /// Registra la justificación de inasistencia
        /// </summary>
        /// <param name="model">Modelo que contiene la justificación de inasistencia</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("registrar/post")]
        public async Task<IActionResult> AddAbsenceJustification(AbsenceJustificationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var workingDay = await _workingDayService.Get(model.WorkingDayId);
            //if (!workingDay.IsAbsent)
            //    return BadRequest("Usted no presenta una inasistencia en el día seleccionado.");
            if (await _userAbsenceJustificationService.Any(model.WorkingDayId, ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS))
                return BadRequest("Ya existe una solicitud en proceso para el mismo día de trabajo.");
            if (model.File?.Length > 1024 * 1024 * 20)
                return BadRequest("El archivo no debe pesar más de 20MB.");

            var absenceJustification = new UserAbsenceJustification
            {
                WorkingDayId = model.WorkingDayId,
                Justification = model.Justification,
                Status = ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS,
                RegisterDate = DateTime.UtcNow
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (model.File.ContentType.Contains("image"))
                    absenceJustification.File = await storage.UploadFile(model.File.OpenReadStream(),
                        CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_ABSENCE_JUSTIFICATION_FILE, Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                else
                    absenceJustification.File = await storage.UploadProductBinary(model.File.OpenReadStream(),
                        CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_ABSENCE_JUSTIFICATION_FILE, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            await _userAbsenceJustificationService.Insert(absenceJustification);
            return Ok();
        }

        /// <summary>
        /// Descarga un archivo de justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la justificación de inasistencia</param>
        /// <returns>Retorna un archivo</returns>
        [HttpGet("{id}/archivo/descargar")]
        public async Task DownloadFile(Guid id)
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
        /// Elimina la justificación de inasistencia
        /// </summary>
        /// <param name="id">Identificador de la justificación de inasistencia </param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> DeleteAbsenceJustification(Guid id)
        {
            var absenceJustifiction = await _userAbsenceJustificationService.Get(id);
            if (!absenceJustifiction.Status.Equals(ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS))
                return BadRequest("No puede eliminar una solicitud ya respondida.");
            if (!string.IsNullOrEmpty(absenceJustifiction.File))
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(absenceJustifiction.File.Split('/').Last(),
                    ConstantHelpers.CONTAINER_NAMES.USER_ABSENCE_JUSTIFICATION_FILE);
            }
            await _userAbsenceJustificationService.Delete(absenceJustifiction);
            return Ok();
        }
    }
}
