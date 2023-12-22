using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.TeacherViewModel;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/Teacher")]
    // GET: TeacherController
    public class TeacherController : BaseController
    {
        public readonly ITeacherPService _teacherService;
        private readonly IDataTablesService _dataTablesService;
        public TeacherController(IDataTablesService dataTablesService, ITeacherPService teacherService)
        {
            _teacherService= teacherService;
            _dataTablesService= dataTablesService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("getallteacher")]
        public async Task<IActionResult> GetAllTeacher(string search)
        {
           var parameters = _dataTablesService.GetSentParameters();
            var result = await _teacherService.GetTeacherDataTable(parameters,search);
           return Ok(result);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AddPost(AddTeacherViewModel model)
        {
            var entity = new PosdegreeTeacher
            {
                name = model.Nombre,
                PaternalSurName= model.APaterno,
                Maternalsurname=model.AMaterno,
                Email=model.Email,
                PhoneNumber=model.Telefono,
                Departament=model.Departamento,
                Especiality=model.Especialidad
            };
            await _teacherService.Insert(entity);
            return RedirectToAction("Index");

        }
        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _teacherService.DeleteTeacher(id);
            return RedirectToAction("Index");
        }
    }
}
