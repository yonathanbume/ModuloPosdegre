using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.ForeignUniversityViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.DEGREE.Helpers;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/universidad-extranjera")]
    public class ForeignUniversityOriginController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IForeignUniversityOriginService _foreignUniversityOriginService;

        public ForeignUniversityOriginController(
            IDataTablesService dataTablesService,
            IForeignUniversityOriginService foreignUniversityOriginService
        ) :base()
        {
            _dataTablesService = dataTablesService;
            _foreignUniversityOriginService = foreignUniversityOriginService;
        }

        /// <summary>
        /// Vista principal donde se lista las universidades extranjeras
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de universidades extranjeras
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("datatable/get")]
        public async Task<IActionResult> GetForeignUniversityOriginDatatable(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _foreignUniversityOriginService.GetForeignUniveristyOriginDatatable(sentParameters, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar una universidad extranjera
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos de la universidad extranjera</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> AddOrigin(ForeignUniversityOriginViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Sucedio un error");

            var foreignUniversityOrigin = new ForeignUniversityOrigin
            {
                Name = viewModel.Name
            };

            await _foreignUniversityOriginService.Insert(foreignUniversityOrigin);

            return Ok();
        }

        /// <summary>
        /// Método para obtener los detalles de la universidad extranjera
        /// </summary>
        /// <param name="id">Identificador de la universidad extanjera</param>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetOriginDetail(Guid id)
        {
            var foreignUniversityOrigin = await _foreignUniversityOriginService.Get(id);
            return Ok(foreignUniversityOrigin);
        }

        /// <summary>
        /// Método para actualizar una universidad extranjera
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos actualizados de la universidad extranjera</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("actualizar")]
        public async Task<IActionResult> Update(ForeignUniversityOriginViewModel viewModel)
        {
            if (!ModelState.IsValid) return BadRequest("Sucedio un error");

            var foreignUniversityOrigin = await _foreignUniversityOriginService.Get(viewModel.Id);

            foreignUniversityOrigin.Name = viewModel.Name;

            await _foreignUniversityOriginService.Update(foreignUniversityOrigin);

            return Ok();
        }

        /// <summary>
        /// Método para eliminar a la universidad extranjera
        /// </summary>
        /// <param name="id">Identificador de la universidad extranjera</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var foreignUniversityOrigin = await _foreignUniversityOriginService.Get(id);
            await _foreignUniversityOriginService.Delete(foreignUniversityOrigin);
            return Ok();
        }


    }
}
