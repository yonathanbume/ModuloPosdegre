using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularActivitiesViewModel;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/actividades-extracurriculares")]
    public class ExtracurricularActivitiesController : BaseController
    {
        private readonly IExtracurricularActivityService _extracurricularActivityService;

        public ExtracurricularActivitiesController(IDataTablesService dataTablesService,
            IExtracurricularActivityService extracurricularActivityService) : base(dataTablesService)
        {
            _extracurricularActivityService = extracurricularActivityService;
        }

        /// <summary>
        /// Vista donde se gestionan las actividades extracurriculares
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de actividades extracurriculares
        /// </summary>
        /// <returns>Objeto que contiene el listaado de actividades extracurriculares</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetActivities(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _extracurricularActivityService.GetDataDatatable(sentParameters, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar una actividad extracurricular
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva actividad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateExtracurricularActivity([Bind(Prefix = "Add")] ExtracurricularActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var anyActivityByName = await _extracurricularActivityService.GetByName(model.Name);
            if (anyActivityByName != null)
                return BadRequest("Ya existe una actividad extracurricular con el mismo nombre.");

            var anyActivityByCode = await _extracurricularActivityService.GetByCode(model.Code);
            if (anyActivityByCode != null)
                return BadRequest("Ya existe una actividad extracurricular con el mismo código.");

            var extracurricularActivity = new ExtracurricularActivity
            {
                Code = model.Code,
                Credits = model.Credits,
                Description = model.Description,
                Name = model.Name,
                TermId = model.TermId,
                ExtracurricularAreaId = model.AreaId
            };

            await _extracurricularActivityService.Insert(extracurricularActivity);
            return Ok();
        }

        /// <summary>
        /// Método para editar una actividad extracurricular
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la actividad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> EditExtracurricularActivity([Bind(Prefix = "Edit")] ExtracurricularActivityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var anyActivityByName = await _extracurricularActivityService.GetByName(model.Name);
            if (anyActivityByName != null && anyActivityByName.Id != model.Id)
                return BadRequest("Ya existe una actividad extracurricular con el mismo nombre.");

            var anyActivityByCode = await _extracurricularActivityService.GetByCode(model.Code);
            if (anyActivityByCode != null && anyActivityByCode.Id != model.Id)
                return BadRequest("Ya existe una actividad extracurricular con el mismo código.");

            var extracurricularActivity = await _extracurricularActivityService.Get(model.Id.Value);
            if (extracurricularActivity == null)
                return BadRequest("Actividad extracurricular no encontrada.");

            extracurricularActivity.Code = model.Code;
            extracurricularActivity.Name = model.Name;
            extracurricularActivity.Credits = model.Credits;
            extracurricularActivity.Description = model.Description;
            extracurricularActivity.TermId = model.TermId;
            extracurricularActivity.ExtracurricularAreaId = model.AreaId;

            await _extracurricularActivityService.Update(extracurricularActivity);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar una actividad extracurricular
        /// </summary>
        /// <param name="id">Identificador de la activdad extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> DeleteExtracurricularActivity(Guid id)
        {
            await _extracurricularActivityService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene los detalles de la actividad extracurricular
        /// </summary>
        /// <param name="id">Identificador de la actividad extracurricular</param>
        /// <returns>Objeto que contiene los datos de la actividad extracurricular</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var activity = await _extracurricularActivityService.Get(id);
            return Ok(new
            {
                id = activity.Id,
                code = activity.Code,
                name = activity.Name,
                description = activity.Description,
                credits = activity.Credits,
                termId = activity.Credits,
                areaId = activity.Credits,
            });
        }
    }
}
