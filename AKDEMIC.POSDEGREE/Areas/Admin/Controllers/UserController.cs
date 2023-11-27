using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/user")]

    public class UserController : BaseController
    {
      
        private readonly IUserService _userService;
        private readonly IDataTablesService _dataTablesService;
        public UserController( IUserService userService, IDataTablesService dataTablesService)
        {
          
            _userService = userService;
            _dataTablesService = dataTablesService;
        }
        [HttpGet("getalluser")]
        public async Task<IActionResult> GetAllUser(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetUserDataTable(parameters, search);
            return Ok(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
