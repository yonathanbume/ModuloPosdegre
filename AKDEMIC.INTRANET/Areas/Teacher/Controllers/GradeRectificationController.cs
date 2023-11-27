using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/correccion-rectificacion-notas")]
    public class GradeRectificationController : BaseController
    {
        private readonly IGradeRectificationService _gradeRectificationService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IGradeService _gradeService;
        public GradeRectificationController(IUserService userService,
            IDataTablesService dataTablesService,
            IAcademicHistoryService academicHistoryService,
            IGradeRectificationService gradeRectificationService,
            ISubstituteExamService substituteExamService,
            IGradeService gradeService) : base(userService, dataTablesService)
        {
            _gradeRectificationService = gradeRectificationService;
            _academicHistoryService = academicHistoryService;
            _substituteExamService = substituteExamService;
            _gradeService = gradeService;
        }

        /// <summary>
        /// Vista donde se listan las rectificaciones de notas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de rectificaciones de notas asignadas al docente logueado
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Listado de rectificaciones</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetGradeRectification(string searchValue)
        {
            var userId = GetUserId();
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeRectificationService.GetAllDatatable(sentParameters, userId, null, searchValue, ConstantHelpers.GRADERECTIFICATION.STATE.CREATED);

            return Ok(result);
        }
    }
}
