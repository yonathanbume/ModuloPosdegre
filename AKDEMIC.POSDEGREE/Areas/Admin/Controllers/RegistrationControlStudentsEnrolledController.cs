using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.PosdegreeStudentViewModel;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/Student")]
    public class RegistrationControlStudentsEnrolledController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IPosdegreeStudentService _posdegreeStudentService;
        private readonly AkdemicContext _context;
        
        public RegistrationControlStudentsEnrolledController(IDataTablesService dataTablesService, IUserService userService, IPosdegreeStudentService posdegreeStudentService,AkdemicContext context)
        {
            _userService = userService;
            _dataTablesService = dataTablesService;
            _posdegreeStudentService=posdegreeStudentService;
            _context=context;
        }
        [HttpGet("getallStudent")]
        public async Task<IActionResult> GetAllStudent(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _posdegreeStudentService.GetStudentDataTable(parameters, search);
            return Ok(result);
        }
        [HttpPost("getalluser/{dni}")]
        public async  Task<IActionResult> GetUserByDNI(string dni)
        {
            var user = _context.Users.FirstOrDefault(u => u.Dni == dni);
            if (user == null)
            {
             
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(AddPosdegreeStudentViewModel model)
        {
            var entity = await _posdegreeStudentService.Get(model.Id);

            entity.Dni = model.Dni;
            entity.Name = model.Nombre;
            entity.PaternalSurname = model.ApellidoP;
            entity.MaternalSurname = model.ApellidoM;
            entity.PhoneNumber = model.telefono;
            entity.Codigo= model.Codigo;
            entity.Address = model.direccion;
            entity.Email = model.email;
           
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _posdegreeStudentService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> AddPost(AddPosdegreeStudentViewModel model)
        {
            var entity = new PosdegreeStudent
            {
                Id = model.Id,
                Name = model.Nombre,
                Codigo = model.Codigo,
                PaternalSurname = model.ApellidoP,
                MaternalSurname = model.ApellidoM,
                Dni = model.Dni,
                Email = model.email,
                PhoneNumber = model.telefono,
                Address = model.direccion,
                File = model.File
            };
        
            await _posdegreeStudentService.Insert(entity);
            return RedirectToAction("Index");

        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
