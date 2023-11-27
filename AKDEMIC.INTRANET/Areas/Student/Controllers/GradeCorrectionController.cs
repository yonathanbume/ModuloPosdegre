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
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Student.Models.GradeCorrectionViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/correccion-notas")]
    public class GradeCorrectionController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;
        private readonly IGradeService _gradeService;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IGradeCorrectionService _gradeCorrectionService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICloudStorageService _cloudStorageService;

        public GradeCorrectionController(
            IStudentService studentService,
            ITermService termService,
            IGradeService gradeService,
            IConfigurationService configurationService,
            IUserService userService,
            IGradeCorrectionService gradeCorrectionService,
            IDataTablesService dataTablesService,
            ICloudStorageService cloudStorageService
            )
        {
            _studentService = studentService;
            _termService = termService;
            _gradeService = gradeService;
            _configurationService = configurationService;
            _userService = userService;
            _gradeCorrectionService = gradeCorrectionService;
            _dataTablesService = dataTablesService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var enableGradeCorreciontionRequest = Convert.ToBoolean(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST));

            if (!enableGradeCorreciontionRequest)
                RedirectToAction(nameof(HomeController.Index), "Home");

            var maxDaysBeforeRequest = Convert.ToInt16(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS));
            var activeTerm = await _termService.GetActiveTerm();

            ViewBag.HasActiveTerm = activeTerm != null;
            ViewBag.TermId = activeTerm?.Id;
            ViewBag.MaxDaysBeforeRequest = maxDaysBeforeRequest;

            return View();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetDatatable(Guid termId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var user = await _userService.GetUserByClaim(User);
            var student = await _studentService.GetStudentByUser(user.Id);
            var result = await _gradeCorrectionService.GetGradeCorrectionsRequestedByStudentDatatable(parameters, termId, student.Id, null);
            return Ok(result);
        }

        [HttpGet("get-notas")]
        public async Task<IActionResult> GetGrades(Guid studentSectionId)
        {
            var grades = await _gradeService.GetAll(studentSectionId);

            var result = grades
                .Select(x => new
                {
                    x.Id,
                    Text = $"{x.Evaluation.Name} - Nota : {x.Value}"
                })
                .ToList();

            return Ok(result);
        }

        [HttpPost("solicitar")]
        public async Task<IActionResult> RequestGradeCorrection(GradeCorrectionViewModel model)
        {
            var maxDaysBeforeRequest = Convert.ToInt16(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.STUDENT_GRADE_CORRECTION_REQUEST_MAX_DAYS));

            var enableGradeCorreciontionRequest = Convert.ToBoolean(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST));

            if (!enableGradeCorreciontionRequest)
                return BadRequest("La funcionalidad no se encuentra habilitada.");

            var grade = await _gradeService.Get(model.GradeId);

            if (await _gradeCorrectionService.AnyGradeCorrectionByFilters(grade.Id, ConstantHelpers.GRADECORRECTION_STATES.STUDENT_REQUEST))
                return BadRequest("La evaluación seleccionada tiene una solicitud con estado pendiente.");

            if (grade.CreatedAt.Value.ToDefaultTimeZone().AddDays(maxDaysBeforeRequest).Date < DateTime.UtcNow.ToDefaultTimeZone().Date)
                return BadRequest($"No se puede solicitar la correción de esta nota ya que fue publicada el {grade.CreatedAt.Value.ToLocalDateFormat()}.");

            var entity = new GradeCorrection
            {
                GradeId = model.GradeId,
                OldGrade = grade.Value,
                Observations = model.Observations,
                RequestedByStudent = true,
                State = ConstantHelpers.GRADECORRECTION_STATES.STUDENT_REQUEST
            };

            if (model.File != null)
            {
                var url = await _cloudStorageService.UploadFile(
                    model.File.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.GRADE_CORRECTION,
                    Path.GetExtension(model.File.FileName)
                    );

                entity.FilePath = url;
            }

            await _gradeCorrectionService.Insert(entity);

            return Ok();
        }


    }
}
