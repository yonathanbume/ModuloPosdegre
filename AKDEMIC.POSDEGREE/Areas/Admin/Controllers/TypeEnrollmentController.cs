using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.AsignaturaViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.TeacherViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.TypeEnrollmentViewModel;
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
    [Route("admin/typeenrollment")]
    public class TypeEnrollmentController:BaseController
    {
        public readonly ITypeEnrollmentService _typeEnrollmentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
        public TypeEnrollmentController(AkdemicContext context, ITypeEnrollmentService typeenrollmentService, IDataTablesService dataTablesService)
        {
            _typeEnrollmentService = typeenrollmentService;
            _dataTablesService = dataTablesService;
            _context = context;
        }
        [HttpGet("getallTypeEnrollment")]
        public async Task<IActionResult> GetAllTypeEnrollment(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _typeEnrollmentService.GetTypeEnrollmentDataTable(parameters, search);
            return Ok(result);
        }
        [HttpPost("Agregar")]
        public async  Task<IActionResult> AddPost(AddTypeEnrollmentViewModel model)
        {
            var entity = new TypeEnrollment
            {
              Name=model.name,
              Costo=model.costo,
              Description=model.Description
                
            };
            await _typeEnrollmentService.Insert(entity);
            return RedirectToAction("Index");

        }
        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _typeEnrollmentService.DeleteTypeEnrollment(id);
            return RedirectToAction("Index");
        }
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(AddTypeEnrollmentViewModel model)
        { 
            var entity = await _typeEnrollmentService.Get(model.Id);

            entity.Name = model.name;
            entity.Description = model.Description;
            entity.Costo = model.costo;
       
            await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}

