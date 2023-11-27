using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.DegreeRequerimentViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-de-requerimientos-de-grado")]
    public class DegreeRequirementController : BaseController
    {
        private readonly IDegreeRequirementService _degreeRequirementService;
        private readonly IDataTablesService _dataTablesService;

        public DegreeRequirementController(IDegreeRequirementService degreeRequirementService,
            IDataTablesService dataTablesService)
        {
            _degreeRequirementService = degreeRequirementService;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista principal donde se listan los requerimientos
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de requerimientos 
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Retorna un OK con los datos para ser usado en tablas</returns>
        [HttpGet("obtener-requerimientos")]
        public async Task<IActionResult> GetDatatable(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _degreeRequirementService.GetDegreeRequirementDatatable(sentParameters, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Mëtodo para agregar un requerimiento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo requerimiento</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> Add(DegreeRequirementeGradeViewModel model)
        {
            var entity = new DegreeRequirement()
            {
                Name = model.Name,
                Type = model.Type
            };
            await _degreeRequirementService.Insert(entity);
            return Ok();
        }

        /// <summary>
        /// Obtiene los detalles del requerimiento
        /// </summary>
        /// <param name="id">Identificador del requerimiento</param>
        /// <returns>Retorna un OK</returns>
        [HttpGet("obtener-requerimiento/{id}")]
        public async Task<IActionResult> GetDegreeRequirement(Guid id)
        {
            var entity =  await _degreeRequirementService.Get(id);
            return Ok(entity);
        }

        /// <summary>
        /// Método para editar el requerimiento
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del requerimiento</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(DegreeRequirementeGradeViewModel model)
        {
            var entity = await _degreeRequirementService.Get(model.Id);
            entity.Name = model.Name;
            entity.Type = model.Type;
            await _degreeRequirementService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar el requerimiento
        /// </summary>
        /// <param name="id">Identificador del requerimiento</param>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _degreeRequirementService.Get(id);        
            await _degreeRequirementService.Delete(entity);
            return Ok();
        }



    }
}
