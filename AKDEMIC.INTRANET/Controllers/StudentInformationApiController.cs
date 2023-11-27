using System;
using System.Threading.Tasks;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Produces("application/json")]
    [Route("api/ficha-estudiante")]
    public class StudentInformationApiController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentInformationApiController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Valida si el estudiante tiene ficha socioeconomica
        /// </summary>
        /// <param name="code">UserName</param>
        /// <returns>Retorna un true o false en caso de tenga o no ficha socioeconomica</returns>
        [AllowAnonymous, HttpGet("valida")]
        public async Task<IActionResult> Updated(string code)
        {
            try
            {
                var result = await _studentService.GetStudentToUpdateByCode(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }
    }
}
