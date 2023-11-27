// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.INTRANET.Areas.Student.Models.CafobeRequestViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("estudiante/apoyo-economico")]
    public class CafobeRequestController : BaseController
    {
        private readonly ICafobeRequestService _cafobeRequestService;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;

        public CafobeRequestController(
            UserManager<ApplicationUser> userManager,
            ICloudStorageService cloudStorageService,
            IUserService userService,
            ICafobeRequestService cafobeRequestService,
            ITermService termService, IStudentService studentService,
            IDataTablesService dataTablesService) : base(userManager, userService, dataTablesService, cloudStorageService)
        {
            _cafobeRequestService = cafobeRequestService;
            _termService = termService;
            _studentService = studentService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("solicitudes")]
        public IActionResult CafobeRequestMenu()
        {
            return View();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetInfo(int? type = -1, int? status = -1, string searchValue = null)
        {
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);

            int? typeValue = null;
            int? statusValue = null;

            if (type != -1) typeValue = type;
            if (status != -1) statusValue = status;

            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _cafobeRequestService.GetStudentRequestDatatable(sentParameters, currentstudent.Id, typeValue, statusValue, searchValue);
            return Ok(result);
        }

        [Route("get/{id}")]
        public async Task<IActionResult> GetCafobeRequest(Guid id)
        {
            var result = await _cafobeRequestService.GetDataById(id);
            return Ok(result);
        }

        #region Alto Rendimiento

        [HttpGet("alto-rendimiento")]
        public async Task<IActionResult> HighPerformance()
        {

            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.HIGH_PERFORMANCE);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("alto-rendimiento/editar/{id}")]
        public async Task<IActionResult> HighPerformanceEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new HighPerformanceViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.LastTermHistoriesUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                ConstancyHigherFifthUrl = cafobeRequest.ConstancyHigherFifthUrl,
                MeritChartHigherFifthUrl = cafobeRequest.MeritChartHigherFifthUrl
            };

            

            return View(model);
        }

        [HttpPost("alto-rendimiento/agregar")]
        public async Task<IActionResult> AddHighPerformance(HighPerformanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.HIGH_PERFORMANCE);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
    || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
            || model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string constancyHigherFifthPath = "";

            if (model.ConstancyHigherFifthFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.ConstancyHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.ConstancyHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                constancyHigherFifthPath = await _cloudStorageService.UploadFile(model.ConstancyHigherFifthFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.ConstancyHigherFifthFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.ConstancyHigherFifthFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string meritChartHigherFiftPath = "";

            if (model.MeritChartHigherFifthFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.MeritChartHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.MeritChartHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                meritChartHigherFiftPath = await _cloudStorageService.UploadFile(model.MeritChartHigherFifthFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.MeritChartHigherFifthFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.MeritChartHigherFifthFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.HIGH_PERFORMANCE,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                EnrollmentFormUrl = enrollmentFormPath,
                ConstancyHigherFifthUrl = constancyHigherFifthPath,
                MeritChartHigherFifthUrl = meritChartHigherFiftPath
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("alto-rendimiento/editar")]
        public async Task<IActionResult> UpdateHighPerformance(HighPerformanceViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");


            bool hasUpdated = false;

            if (model.DirectorRequestFile != null &&
                (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF) ||
                model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;

            }

            if (model.DocumentaryProcedureVoucherFile != null
                && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {

                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }


            if (model.LastTermHistoriesFile != null
                && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {

                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }

            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {

                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }

            if (model.ConstancyHigherFifthFile != null && (model.ConstancyHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.ConstancyHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.ConstancyHigherFifthUrl = await _cloudStorageService.UploadFile(model.ConstancyHigherFifthFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.ConstancyHigherFifthFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }

            if (model.MeritChartHigherFifthFile != null && (model.MeritChartHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.MeritChartHigherFifthFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.MeritChartHigherFifthUrl = await _cloudStorageService.UploadFile(model.MeritChartHigherFifthFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HIGHPERFORMANCE,
                    Path.GetExtension(model.MeritChartHigherFifthFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                hasUpdated = true;
            }

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }
        #endregion

        #region Maternidad

        [HttpGet("maternidad")]
        public async Task<IActionResult> Maternity()
        {
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.MATERNITY);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("maternidad/editar/{id}")]
        public async Task<IActionResult> MaternityEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new MaternityViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.LastTermHistoriesUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                BabyBirhtCertificateUrl = cafobeRequest.BabyBirhtCertificateUrl,
                BabyControlCardUrl = cafobeRequest.BabyControlCardUrl
            };

            

            return View(model);
        }

        [HttpPost("maternidad/agregar")]
        public async Task<IActionResult> AddMaternity(MaternityViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();

            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.MATERNITY);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string babyBirhtCertificatePath = "";

            if (model.BabyBirhtCertificateFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.BabyBirhtCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.BabyBirhtCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                babyBirhtCertificatePath = await _cloudStorageService.UploadFile(model.BabyBirhtCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.BabyBirhtCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.BabyBirhtCertificateFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string babyControlCardPath = "";

            if (model.BabyControlCardFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.BabyControlCardFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.BabyControlCardFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                babyControlCardPath = await _cloudStorageService.UploadFile(model.BabyControlCardFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.BabyControlCardFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.BabyControlCardFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.MATERNITY,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                EnrollmentFormUrl = enrollmentFormPath,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                BabyBirhtCertificateUrl = babyBirhtCertificatePath,
                BabyControlCardUrl = babyControlCardPath
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("maternidad/editar")]
        public async Task<IActionResult> UpdateMaternity(MaternityViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");

            bool hasUpdated = false;

            if (model.DirectorRequestFile != null && (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DocumentaryProcedureVoucherFile != null && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {

                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.LastTermHistoriesFile != null && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

            }

            if (model.BabyBirhtCertificateFile != null && (model.BabyBirhtCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.BabyBirhtCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.BabyBirhtCertificateUrl = await _cloudStorageService.UploadFile(model.BabyBirhtCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.BabyBirhtCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.BabyControlCardFile != null && (model.BabyControlCardFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.BabyControlCardFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.BabyControlCardUrl = await _cloudStorageService.UploadFile(model.BabyControlCardFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_MATERNITY,
                    Path.GetExtension(model.BabyControlCardFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }
        #endregion

        #region Oftalmológico

        [HttpGet("oftalmologico")]
        public async Task<IActionResult> Ophthalmological()
        {
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.OPHTHALMOLOGICAL);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("oftalmologico/editar/{id}")]
        public async Task<IActionResult> OphthalmologicalEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new OphthalmologicalViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.DniUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                OpthicalMedicalDiagnosticUrl = cafobeRequest.OpthicalMedicalDiagnosticUrl,
                OpthicalProformUrl = cafobeRequest.OpthicalProformUrl
            };

            

            return View(model);
        }

        [HttpPost("oftalmologico/agregar")]
        public async Task<IActionResult> AddOphthalmological(OphthalmologicalViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();

            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.OPHTHALMOLOGICAL);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string opthicalMedicalDiagnosticPath = "";

            if (model.OpthicalMedicalDiagnosticFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.OpthicalMedicalDiagnosticFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.OpthicalMedicalDiagnosticFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                opthicalMedicalDiagnosticPath = await _cloudStorageService.UploadFile(model.OpthicalMedicalDiagnosticFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.OpthicalMedicalDiagnosticFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.OpthicalMedicalDiagnosticFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string opthicalProformPath = "";

            if (model.OpthicalProformFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.OpthicalProformFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.OpthicalProformFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                opthicalProformPath = await _cloudStorageService.UploadFile(model.OpthicalProformFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.OpthicalProformFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.OpthicalProformFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.OPHTHALMOLOGICAL,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                Observation = model.Observation,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                OpthicalMedicalDiagnosticUrl = opthicalMedicalDiagnosticPath,
                OpthicalProformUrl = opthicalProformPath,
                EnrollmentFormUrl = enrollmentFormPath
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("oftalmologico/editar")]
        public async Task<IActionResult> UpdateOphthalmological(OphthalmologicalViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");

            bool hasUpdated = false;


            if (model.DirectorRequestFile != null && (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DocumentaryProcedureVoucherFile != null && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.LastTermHistoriesFile != null && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }


            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.OpthicalMedicalDiagnosticFile != null && (model.OpthicalMedicalDiagnosticFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.OpthicalMedicalDiagnosticFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.OpthicalMedicalDiagnosticUrl = await _cloudStorageService.UploadFile(model.OpthicalMedicalDiagnosticFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.OpthicalMedicalDiagnosticFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.OpthicalProformFile != null && (model.OpthicalProformFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.OpthicalProformFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.OpthicalProformUrl = await _cloudStorageService.UploadFile(model.OpthicalProformFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_OPHTHALMOLOGICAL,
                    Path.GetExtension(model.OpthicalProformFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }

        #endregion

        #region Defunción

        [HttpGet("defuncion")]
        public async Task<IActionResult> FamilyDeath()
        {
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.FAMILY_DEATH);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("defuncion/editar/{id}")]
        public async Task<IActionResult> FamilyDeathEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new FamilyDeathViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.LastTermHistoriesUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                DeathCertificateUrl = cafobeRequest.DeathCertificateUrl,
                StudentBirthCertificateUrl = cafobeRequest.StudentBirthCertificateUrl
            };

            

            return View(model);
        }

        [HttpPost("defuncion/agregar")]
        public async Task<IActionResult> AddFamilyDeath(FamilyDeathViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.FAMILY_DEATH);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string deathCertificatePath = "";

            if (model.DeathCertificateFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DeathCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DeathCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                deathCertificatePath = await _cloudStorageService.UploadFile(model.DeathCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DeathCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DeathCertificateFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string studentBirthCertificatePath = "";

            if (model.StudentBirthCertificateFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.StudentBirthCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentBirthCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                studentBirthCertificatePath = await _cloudStorageService.UploadFile(model.StudentBirthCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.StudentBirthCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.StudentBirthCertificateFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.FAMILY_DEATH,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                Observation = model.Observation,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                DeathCertificateUrl = deathCertificatePath,
                StudentBirthCertificateUrl = studentBirthCertificatePath,
                EnrollmentFormUrl = enrollmentFormPath
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("defuncion/editar")]
        public async Task<IActionResult> UpdateFamilyDeath(FamilyDeathViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");

            bool hasUpdated = false;

            if (model.DirectorRequestFile != null && (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DocumentaryProcedureVoucherFile != null && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.LastTermHistoriesFile != null && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DeathCertificateFile != null && (model.DeathCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DeathCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DeathCertificateUrl = await _cloudStorageService.UploadFile(model.DeathCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.DeathCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.StudentBirthCertificateFile != null && (model.StudentBirthCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentBirthCertificateFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.StudentBirthCertificateUrl = await _cloudStorageService.UploadFile(model.StudentBirthCertificateFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_FAMILYDEATH,
                    Path.GetExtension(model.StudentBirthCertificateFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }


            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }
        #endregion

        #region Salud

        [HttpGet("salud")]
        public async Task<IActionResult> Health()
        {
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.HEALTH);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("salud/editar/{id}")]
        public async Task<IActionResult> HealthEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new HealthViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.LastTermHistoriesUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                MedicalRecordUrl = cafobeRequest.MedicalRecordUrl,
                TreatmentRecordUrl = cafobeRequest.TreatmentRecordUrl
            };

            

            return View(model);
        }

        [HttpPost("salud/agregar")]
        public async Task<IActionResult> AddHealth(HealthViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();

            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.HEALTH);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string medicalRecordPath = "";

            if (model.MedicalRecordFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.MedicalRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.MedicalRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                medicalRecordPath = await _cloudStorageService.UploadFile(model.MedicalRecordFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.MedicalRecordFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.MedicalRecordFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            string treatmentRecordPath = "";

            if (model.TreatmentRecordFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.TreatmentRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.TreatmentRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                string extension = Path.GetExtension(model.TreatmentRecordFile.FileName);
                treatmentRecordPath = await _cloudStorageService.UploadFile(model.TreatmentRecordFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.TreatmentRecordFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.TreatmentRecordFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");


            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.HEALTH,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                Observation = model.Observation,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                MedicalRecordUrl = medicalRecordPath,
                TreatmentRecordUrl = treatmentRecordPath,
                EnrollmentFormUrl = enrollmentFormPath,
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("salud/editar")]
        public async Task<IActionResult> UpdateHealth(HealthViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
    || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
    || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");

            bool hasUpdated = false;

            if (model.DirectorRequestFile != null && (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DocumentaryProcedureVoucherFile != null && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.LastTermHistoriesFile != null && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.MedicalRecordFile != null && (model.MedicalRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.MedicalRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.MedicalRecordUrl = await _cloudStorageService.UploadFile(model.MedicalRecordFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.MedicalRecordFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.TreatmentRecordFile != null && (model.TreatmentRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.TreatmentRecordFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.TreatmentRecordUrl = await _cloudStorageService.UploadFile(model.TreatmentRecordFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.TreatmentRecordFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }

        #endregion

        #region Estímulo

        [HttpGet("estimulo")]
        public async Task<IActionResult> StimulusScholarshipOrOutstandingSportsman()
        {
            var searchterm = await _termService.GetActiveTerm();
            if (searchterm == null)
                return RedirectToAction(nameof(Index));
            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN);
            if (prev)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpGet("estimulo/editar/{id}")]
        public async Task<IActionResult> StimulusScholarshipOrOutstandingSportsmanEdit(Guid id)
        {
            var cafobeRequest = await _cafobeRequestService.Get(id);

            var model = new StimulusScholarshipOrOutstandingSportsmanViewModel
            {
                StudentId = cafobeRequest.Id,
                TermId = cafobeRequest.TermId,
                Type = cafobeRequest.Type,
                Status = cafobeRequest.Status,
                DirectorRequestUrl = cafobeRequest.DirectorRequestUrl,
                DocumentaryProcedureVoucherUrl = cafobeRequest.DocumentaryProcedureVoucherUrl,
                LastTermHistoriesUrl = cafobeRequest.LastTermHistoriesUrl,
                DniUrl = cafobeRequest.DniUrl,
                EnrollmentFormUrl = cafobeRequest.EnrollmentFormUrl,
                EventInvitationUrl = cafobeRequest.EventInvitationUrl,
                StudentHealthInsuranceUrl = cafobeRequest.StudentHealthInsuranceUrl,
                StudentSportParticipationUrl = cafobeRequest.StudentSportParticipationUrl

            };

            

            return View(model);
        }

        [HttpPost("estimulo/agregar")]
        public async Task<IActionResult> AddStimulusScholarshipOrOutstandingSportsman(StimulusScholarshipOrOutstandingSportsmanViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revisa el formulario");
            }

            //var previouscafoberequest = 

            var user = await GetCurrentUserAsync();
            var currentstudent = await _studentService.GetStudentByUser(user.Id);
            var searchterm = await _termService.GetActiveTerm();

            if (searchterm == null)
                return RedirectToAction(nameof(Index));

            var prev = await _cafobeRequestService.GetLastByStudent(currentstudent.Id, searchterm.Id, ConstantHelpers.CAFOBE_REQUEST.TYPE.STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN);
            if (prev)
                return BadRequest("Revisa el formulario");

            string directorRequestPath = "";

            if (model.DirectorRequestFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                || model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                directorRequestPath = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            else
                return BadRequest($"El contenido del archivo '{model.DirectorRequestFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string documentaryProcedureVoucherPath = "";

            if (model.DocumentaryProcedureVoucherFile == null)
                return BadRequest("Por favor adjunte un archivo");


            if (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                documentaryProcedureVoucherPath = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DocumentaryProcedureVoucherFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string lastTermHistoriesPath = "";

            if (model.LastTermHistoriesFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                lastTermHistoriesPath = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.LastTermHistoriesFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string dniPath = "";

            if (model.DniFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                dniPath = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                   Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.DniFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string enrollmentFormPath = "";

            if (model.EnrollmentFormFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                enrollmentFormPath = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string eventInvitationPath = "";

            if (model.EventInvitationFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.EventInvitationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EventInvitationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                eventInvitationPath = await _cloudStorageService.UploadFile(model.EventInvitationFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.EventInvitationFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.EnrollmentFormFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string studentHealthInsurancePath = "";

            if (model.StudentHealthInsuranceFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.StudentHealthInsuranceFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentHealthInsuranceFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                studentHealthInsurancePath = await _cloudStorageService.UploadFile(model.StudentHealthInsuranceFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.StudentHealthInsuranceFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.StudentHealthInsuranceFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            string studentSportParticipationPath = "";

            if (model.StudentSportParticipationFile == null)
                return BadRequest("Por favor adjunte un archivo");

            if (model.StudentSportParticipationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentSportParticipationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX))
            {
                studentSportParticipationPath = await _cloudStorageService.UploadFile(model.StudentSportParticipationFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.StudentSportParticipationFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            else
                return BadRequest($"El contenido del archivo '{model.StudentSportParticipationFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

            var cafobeRequest = new CafobeRequest
            {
                StudentId = currentstudent.Id,
                TermId = searchterm.Id,
                Type = ConstantHelpers.CAFOBE_REQUEST.TYPE.STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.PENDING,
                Observation = model.Observation,
                DirectorRequestUrl = directorRequestPath,
                DocumentaryProcedureVoucherUrl = documentaryProcedureVoucherPath,
                LastTermHistoriesUrl = lastTermHistoriesPath,
                DniUrl = dniPath,
                EventInvitationUrl = eventInvitationPath,
                StudentHealthInsuranceUrl = studentHealthInsurancePath,
                StudentSportParticipationUrl = studentSportParticipationPath,
                EnrollmentFormUrl = enrollmentFormPath
            };

            await _cafobeRequestService.Insert(cafobeRequest);

            return Ok();
        }

        [HttpPost("estimulo/editar")]
        public async Task<IActionResult> UpdateStimulusScholarshipOrOutstandingSportsman(StimulusScholarshipOrOutstandingSportsmanViewModel model)
        {
            var cafobeRequest = await _cafobeRequestService.Get(model.Id);

            if (cafobeRequest == null)
                return BadRequest();

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.APPROVED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED
                || cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.DENIED)
                return BadRequest("La Solicitud ya no puede ser modificada");

            bool hasUpdated = false;

            if (model.DirectorRequestFile != null && (model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DirectorRequestFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DirectorRequestUrl = await _cloudStorageService.UploadFile(model.DirectorRequestFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.DirectorRequestFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DocumentaryProcedureVoucherFile != null && (model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DocumentaryProcedureVoucherFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DocumentaryProcedureVoucherUrl = await _cloudStorageService.UploadFile(model.DocumentaryProcedureVoucherFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.DocumentaryProcedureVoucherFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.LastTermHistoriesFile != null && (model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.LastTermHistoriesFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.LastTermHistoriesUrl = await _cloudStorageService.UploadFile(model.LastTermHistoriesFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.LastTermHistoriesFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.DniFile != null && (model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.DniFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.DniUrl = await _cloudStorageService.UploadFile(model.DniFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.DniFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.EnrollmentFormFile != null && (model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EnrollmentFormFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EnrollmentFormUrl = await _cloudStorageService.UploadFile(model.EnrollmentFormFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN,
                    Path.GetExtension(model.EnrollmentFormFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.EventInvitationFile != null && (model.EventInvitationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.EventInvitationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.EventInvitationUrl = await _cloudStorageService.UploadFile(model.EventInvitationFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.EventInvitationFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.StudentHealthInsuranceFile != null && (model.StudentHealthInsuranceFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentHealthInsuranceFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.StudentHealthInsuranceUrl = await _cloudStorageService.UploadFile(model.StudentHealthInsuranceFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.StudentHealthInsuranceFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (model.StudentSportParticipationFile != null && (model.StudentSportParticipationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
|| model.StudentSportParticipationFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)))
            {
                cafobeRequest.StudentSportParticipationUrl = await _cloudStorageService.UploadFile(model.StudentSportParticipationFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.CAFOBE_HEALTH,
                    Path.GetExtension(model.StudentSportParticipationFile.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);

                hasUpdated = true;
            }

            if (cafobeRequest.Status == ConstantHelpers.CAFOBE_REQUEST.STATUS.OBSERVED && hasUpdated)
            {
                cafobeRequest.Status = ConstantHelpers.CAFOBE_REQUEST.STATUS.CORRECTED;
                await _cafobeRequestService.Update(cafobeRequest);
            }

            else if (hasUpdated)
            {
                await _cafobeRequestService.Update(cafobeRequest);
            }

            return Ok();

        }
        #endregion  
    }
}
