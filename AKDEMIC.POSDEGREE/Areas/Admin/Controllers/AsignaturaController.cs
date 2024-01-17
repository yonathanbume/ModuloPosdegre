using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.POSDEGREE.Controllers;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.TeacherViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.AsignaturaViewModel;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/asignatura")]
    // GET: GestionController
    public class AsignaturaController : BaseController
    {
       public  readonly  IAsignaturaService _asignaturaService;
        private readonly IDataTablesService _dataTablesService;

        public AsignaturaController(IAsignaturaService asignaturaService, IDataTablesService dataTablesService )
        {
            _asignaturaService = asignaturaService;
            _dataTablesService = dataTablesService;
        }
        [HttpGet("getallasignatura")]
        public async Task<IActionResult> GetAllAsignatura(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _asignaturaService.GetAsignaturaDataTable(parameters, search);
            return Ok(result);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AddPost(AddAsignaturaViewModel model)
        {
            var entity = new Asignatura
            {
                Code = model.codigo,
                NameAsignatura = model.nameAsignatura,
                Credits = model.credito,
                TeoricasHours = model.hteoricas,
                PracticalHours = model.hpracticas,
                TotalHours = model.totalhoras,
                Requisito = model.requisito
            };
            await _asignaturaService.Insert(entity);
            return RedirectToAction("Index");

        }

    }
}
