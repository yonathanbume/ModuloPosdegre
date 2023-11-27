using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
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
        private readonly AkdemicContext _context;
        public RegistrationControlStudentsEnrolledController(IDataTablesService dataTablesService, IUserService userService, AkdemicContext context)
        {
            _userService = userService;
            _dataTablesService = dataTablesService;
            _context=context;
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
        [HttpPost("regitrar")]
        public async Task<IActionResult> AddPost(AddMasterViewModel model)
        {
           
            return RedirectToAction("Index");

        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
