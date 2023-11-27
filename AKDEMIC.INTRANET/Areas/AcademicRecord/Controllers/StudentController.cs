using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = 
        ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + 
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/alumnos")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public StudentController(IStudentService studentService, IDataTablesService datatableService, IUserService userService) : base(datatableService)
        {
            _studentService = studentService;
            _userService = userService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de estudiantes para ser usado en tablas
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [Route("get")]
        public async Task<IActionResult> GetStudents(Guid? fid = null, Guid? cid = null, string search = null)
        {
            var sentParameters = GetSentParameters();
            
            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = _userService.GetUserIdByClaim(User);
                var result = await _studentService.GetStudentsDatatable(sentParameters, search, fid, cid, null, null, userId);
                return Ok(result);
            }
            else
            {
                var result = await _studentService.GetStudentsDatatable(sentParameters, search, fid, cid);
                return Ok(result);
            }
        }
    }
}
