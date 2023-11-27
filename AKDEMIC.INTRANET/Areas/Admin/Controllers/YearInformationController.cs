using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.YearInformationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Route("admin/informacion-anual")]
    public class YearInformationController : BaseController
    {
        private readonly IYearInformationService _yearInformationService;

        public YearInformationController(
            IDataTablesService dataTablesService,
            IYearInformationService yearInformationService
        ) : base(dataTablesService)
        {
            _yearInformationService = yearInformationService;
        }

        /// <summary>
        /// Vistaa donde se gestiona la información anual
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de la información anual
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listaado de la información anual</returns>
        [HttpGet("datatable/get")]
        public async Task<IActionResult> GetDatatable(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _yearInformationService.GetAllYearInformationDatatable(sentParameters, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar la información anual
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos de la nueva información anual</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> Add(YearInformationViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Sucedio un error");

            if (await _yearInformationService.AnyByYear(viewModel.Year)) return BadRequest("Ya existe una información relacionada a ese año");

            var yearInformation = new YearInformation
            {
                Year = viewModel.Year,
                Name = viewModel.Name
            };

            await _yearInformationService.Insert(yearInformation);

            return Ok();
        }

        /// <summary>
        /// Obtiene los detalles de la información anual
        /// </summary>
        /// <param name="id">Identificador de la información anual</param>
        /// <returns>Objeto con los datos de la información anual</returns>
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> Get(Guid id)
        {
            var yearInformation = await _yearInformationService.Get(id);

            var data = new
            {
                yearInformation.Year,
                yearInformation.Name,
                yearInformation.Id,
            };

            return Ok(data);
        }

        /// <summary>
        /// Método para actualizar la información anual
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos actualizados de la información anual</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar")]
        public async Task<IActionResult> Update(YearInformationViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Sucedio un error");

            if (await _yearInformationService.AnyByYear(viewModel.Year, viewModel.Id)) return BadRequest("Ya existe una información relacionada a ese año");

            var yearInformation = await _yearInformationService.Get(viewModel.Id);

            yearInformation.Name = viewModel.Name;
            yearInformation.Year = viewModel.Year;

            await _yearInformationService.Update(yearInformation);

            return Ok();
        }

        /// <summary>
        /// Método para eliminar la información anual
        /// </summary>
        /// <param name="id">Identificador de la información anual</param>
        /// <returns>Código de estaado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var yearInformation = await _yearInformationService.Get(id);
            await _yearInformationService.Delete(yearInformation);
            return Ok();
        }
    }
}
