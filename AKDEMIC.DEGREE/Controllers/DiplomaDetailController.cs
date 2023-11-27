using System;
using System.Threading.Tasks;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Controllers
{
    [AllowAnonymous]
    [Route("detalle-diploma")]
    public class DiplomaDetailController : BaseController
    {
        private readonly IRegistryPatternService _registryPatternService;
        public DiplomaDetailController(IRegistryPatternService registryPatternService)
        {
            _registryPatternService = registryPatternService;
        }

        /// <summary>
        /// VIsta donde se muestra el detalle del diploma
        /// </summary>
        /// <param name="id">Identificador del diploma</param>
        /// <returns>Retorna un OK</returns>
        public async Task<IActionResult> Index(Guid id)
        {
            var viewModel = await _registryPatternService.GetPdfReport(id);
            return View(viewModel);
        }
    }

}
