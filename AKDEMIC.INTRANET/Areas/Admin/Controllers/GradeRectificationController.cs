using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.GradeRectificationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/correccion-rectificacion-notas")]
    public class GradeRectificationController : BaseController
    {
        private readonly IGradeRectificationService _gradeRectificationService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IGradeService _gradeService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly ISectionService _sectionService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public GradeRectificationController(
            IDataTablesService dataTablesService,
            IAcademicHistoryService academicHistoryService,
            IGradeRectificationService gradeRectificationService,
            ISubstituteExamService substituteExamService,
            ITeacherSectionService teacherSectionService,
            ISectionService sectionService,
            IGradeService gradeService,
            IOptions<CloudStorageCredentials> storageCredentials) : base(dataTablesService)
        {
            _gradeRectificationService = gradeRectificationService;
            _academicHistoryService = academicHistoryService;
            _substituteExamService = substituteExamService;
            _gradeService = gradeService;
            _teacherSectionService = teacherSectionService;
            _sectionService = sectionService;
            _storageCredentials = storageCredentials;
        }

        /// <summary>
        /// Vista donde se gestionan las rectificaciones de notas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de solicitudes de rectificaciones de notas antiguas
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de rectificaciones de notas antiguas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetGradeRectification(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeRectificationService.GetAllDatatable(sentParameters, null, null, searchValue);

            return Ok(result);
        }

        /// <summary>
        /// Método para crear una solicitud de rectificación de nota
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la solicitud</param>
        /// <returns>Método de estado HTTP</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> CreateGradeRectification(GradeRectificationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var storage = new CloudStorageService(_storageCredentials);

            if (model.Type == ConstantHelpers.GRADERECTIFICATION.TYPE.GRADECORRECTION)
            {
                var lstGradeRectification = new List<GradeRectification>();
                foreach (var item in model.StudentIds)
                {
                    if (await _gradeRectificationService.AnyByEvaluation(item))
                        return BadRequest($"Ya existe una solicitud para ese periodo .");

                    var grade = await _gradeService.Get(item);
                    var gradeRectification = new GradeRectification()
                    {
                        GradeId = item,
                        EvaluationId = model.EvaluationId,
                        State = ConstantHelpers.GRADERECTIFICATION.STATE.CREATED,
                        Type = model.Type,
                        TeacherId = model.TeacherId,
                        GradePrevious = grade.Value
                    };

                    if (model.ReasonFile != null)
                    {
                        gradeRectification.ReasonFile = await storage.UploadFile(model.ReasonFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.GRADE_RECTIFICATION,
                            Path.GetExtension(model.ReasonFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                    }

                    lstGradeRectification.Add(gradeRectification);
                }
                await _gradeRectificationService.InsertRange(lstGradeRectification);
            }
            else
            {
                var lstGradeRectification = new List<GradeRectification>();
                foreach (var item in model.StudentIds)
                {
                    var substituteExam = await _substituteExamService.GetAsync(item);


                    var gradeRectification = new GradeRectification()
                    {
                        State = ConstantHelpers.GRADERECTIFICATION.STATE.CREATED,
                        Type = model.Type,
                        TeacherId = model.TeacherId,
                        SubstituteExamId = substituteExam.Id,
                        GradePrevious = substituteExam.ExamScore.HasValue ? substituteExam.ExamScore.Value : 0
                    };

                    if (model.ReasonFile != null)
                    {
                        gradeRectification.ReasonFile = await storage.UploadFile(model.ReasonFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.GRADE_RECTIFICATION,
                            Path.GetExtension(model.ReasonFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                    }

                    lstGradeRectification.Add(gradeRectification);
                }

                await _gradeRectificationService.InsertRange(lstGradeRectification);
            }

            return Ok();

        }
    }
}
