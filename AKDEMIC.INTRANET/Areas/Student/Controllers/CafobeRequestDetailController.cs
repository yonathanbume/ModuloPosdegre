// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.INTRANET.Areas.Student.Models.CafobeRequestDetailViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenXmlPowerTools;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("estudiante/rendicion-cuenta")]
    public class CafobeRequestDetailController : BaseController
    {
        private readonly ICafobeRequestDetailService _cafobeRequestDetailService;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;

        public CafobeRequestDetailController(
    UserManager<ApplicationUser> userManager,
    ICloudStorageService cloudStorageService,
    IUserService userService,
    ICafobeRequestDetailService cafobeRequestDetailService,
    ITermService termService, IStudentService studentService,
    IDataTablesService dataTablesService) : base(userManager, userService, dataTablesService, cloudStorageService)
        {
            _cafobeRequestDetailService = cafobeRequestDetailService;
            _termService = termService;
            _studentService = studentService;

        }

        public IActionResult Index()
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
            var result = await _cafobeRequestDetailService.GetStudentRequestDetailDatatable(sentParameters, currentstudent.Id, typeValue, statusValue, searchValue);
            return Ok(result);
        }

        [HttpGet("get/{cafobeRequestId}")]
        public async Task<IActionResult> Edit(Guid cafobeRequestId)
        {
            var cafobeRequestDetail = await _cafobeRequestDetailService.Get(cafobeRequestId);

            var model = new CafobeRequestDetailViewModel();

            model.CafobeRequestId = cafobeRequestId;

            if (cafobeRequestDetail != null)
            {
                model.StatusText = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES.ContainsKey(cafobeRequestDetail.Status) ?
                        ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.VALUES[cafobeRequestDetail.Status] : "";
                model.Comentary = cafobeRequestDetail.Comentary;
            }

            return Ok(model);
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> Add(CafobeRequestDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Revisa el formulario");
            }

            var cafobeRequestDetailPrev = await _cafobeRequestDetailService.Get(model.CafobeRequestId);


            if (cafobeRequestDetailPrev == null)
            {
                string fileDetailPath = "";

                if (model.FileDetailFile == null)
                    return BadRequest("Por favor adjunte un archivo");

                else
                {
                    if (model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                        || model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)
                        || model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.XLS))
                    {
                        string extension = Path.GetExtension(model.FileDetailFile.FileName);
                        fileDetailPath = await _cloudStorageService.UploadFile(model.FileDetailFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.CAFOBE_REQUESTDETAIL,
                            extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INSTITUTIONALWELFARE);
                    }

                    else
                        return BadRequest($"El contenido del archivo '{model.FileDetailFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word"); 

                }


                var cafobeRequestDetail = new CafobeRequestDetail
                {
                    CafobeRequestId = model.CafobeRequestId,
                    Status = AKDEMIC.CORE.Helpers.ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.PENDING,
                    RegisterDate = DateTime.UtcNow,
                    FileDetailUrl = fileDetailPath,
                };

                await _cafobeRequestDetailService.Insert(cafobeRequestDetail);
            }

            else
            {
                if (cafobeRequestDetailPrev.Status == ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.OBSERVED || cafobeRequestDetailPrev.Status == ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.PENDING)
                {
                    string fileDetailPath = "";

                    if (model.FileDetailFile == null)
                        return BadRequest("Por favor adjunte un archivo");

                    else
                    {
                        if (model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.PDF)
                            || model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.DOCX)
                            || model.FileDetailFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.XLS))
                        {
                            string extension = Path.GetExtension(model.FileDetailFile.FileName);
                            fileDetailPath = await _cloudStorageService.UploadFile(model.FileDetailFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.CAFOBE_REQUESTDETAIL,
                                extension, ConstantHelpers.FileStorage.SystemFolder.INSTITUTIONALWELFARE);
                        }

                        else
                            return BadRequest($"El contenido del archivo '{model.FileDetailFile.FileName}' es inválido, por favor adjunte un archuivo pdf o word");

                    }

                    if (cafobeRequestDetailPrev.Status == ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.PENDING)
                        cafobeRequestDetailPrev.Status = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.PENDING;
                    else
                        cafobeRequestDetailPrev.Status = ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS.CORRECTED;

                    cafobeRequestDetailPrev.FileDetailUrl = fileDetailPath;

                    await _cafobeRequestDetailService.Update(cafobeRequestDetailPrev);
                }
                else
                    return BadRequest("No puede editar la información");
            }
            return Ok();
        }

    }
}
