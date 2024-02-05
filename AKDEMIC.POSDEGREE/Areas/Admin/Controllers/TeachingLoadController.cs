using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/TeachingLoad")]
    public class TeachingLoadController : BaseController
    {
        public readonly IAsignaturaService _asignaturaService;
        public readonly IMasterService _masterService;
        public readonly ISemestreService _semestreService;
        public readonly ITeacherPService _teacherPService;
        public TeachingLoadController(IAsignaturaService asignaturaService, IMasterService masterService, ISemestreService semestreService,
         ITeacherPService teacherPService)
        {
            _asignaturaService = asignaturaService;
            _masterService = masterService;
            _semestreService = semestreService;
            _teacherPService = teacherPService;
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Obtiene el listado de asignaturas
        /// </summary>
        /// <returns>Retorna un Ok con la información de las asignaturas para ser usado en select</returns>
        [HttpGet("Asignatura/get")]
        public async Task<IActionResult> GetAsignatura()
        {
            var result = await _asignaturaService.GetAsignaturaAllJson();
            return Ok(new { items = result });

        }
        [HttpGet("Docentes/get")]
        public async Task<IActionResult> GetDocente()
        {
            var result = await _teacherPService.GetDocenteAllJson();
            return Ok(new { items = result });

        }
        [HttpGet("Semestre/get")]
        public async Task<IActionResult> GetSemestre()
        {
            var result = await _semestreService.GetSemestreAllJson();
            return Ok(new { items = result });

        }
        [HttpGet("Master/get")]
        public async Task<IActionResult> GetMaestria()
        {
            var result = await _masterService.GetMasterAllJson();
            return Ok(new { items = result });

        }
    }
}

