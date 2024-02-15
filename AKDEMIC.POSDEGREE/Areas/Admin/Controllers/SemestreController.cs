using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.AsignaturaViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.SemestreViewModel;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/semestre")]
    public class SemestreController : BaseController
    {
        public readonly ISemestreService _semestreService;
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
        public SemestreController(AkdemicContext context,ISemestreService semestreService, IDataTablesService dataTablesService)
        {
            _semestreService = semestreService;
            _dataTablesService = dataTablesService;
            _context= context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("getallsemestre")]
        public async Task<IActionResult> GetAllSemestre(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _semestreService.GetSemestreDataTable(parameters, search);
            return Ok(result);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AddPost(AddSemestreViewModel model)
        {
            var entity = new Semestre
            {
                Name = model.codigo,
                StartDate = model.fInicio,
                EndDate = model.fFinalizacion,
            };
            await _semestreService.Insert(entity);
            return RedirectToAction("Index");

        }
        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _semestreService.DeleteSemestre(id);
            return RedirectToAction("Index");
        }
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(AddSemestreViewModel model)
        {
            var entity = await _semestreService.Get(model.id);

            entity.StartDate = model.fInicio;
            entity.EndDate= model.fFinalizacion;
            entity.Name = model.codigo;
            await _context.SaveChangesAsync();
            return Ok();
        }
     
    }
}
