using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularCourseViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/cursos-extracurriculares")]
    public class ExtracurricularCourseController : BaseController
    {
        private readonly IExtracurricularCourseService _extracurricularCourseService;

        public ExtracurricularCourseController(IDataTablesService dataTablesService,
            IExtracurricularCourseService extracurricularCourseService) : base(dataTablesService)
        {
            _extracurricularCourseService = extracurricularCourseService;
        }

        /// <summary>
        /// Vista donde se gestionan los cursos extracurriculares
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> GetExtracurricularCourses(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _extracurricularCourseService.GetDataDatatable(sentParameters, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para crear un curso extracurricular
        /// </summary>
        /// <param name="model">Objeto con los datos del nuevo curso extracurricular.</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateExtracurricularCourse([Bind(Prefix = "Add")] ExtracurricularCourseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var anyCourse = await _extracurricularCourseService.GetByCode(model.Code);
            if (anyCourse != null)
                return BadRequest("Ya existe un curso con el mismo código.");

            var extracurricularCourse = new ExtracurricularCourse
            {
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Credits = model.Credits,
                ExtracurricularAreaId = model.AreaId
            };

            await _extracurricularCourseService.Insert(extracurricularCourse);
            return Ok();
        }

        /// <summary>
        /// Retorna el detalle de un curso extracurricular.
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular.</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetExtracurricularCourse(Guid id)
        {
            var course = await _extracurricularCourseService.Get(id);
            var result = new
            {
                id = course.Id,
                name = course.Name,
                code = course.Code,
                price = course.Price,
                credits = course.Credits,
                description = course.Description
            };
            return Ok(result);
        }

        /// <summary>
        /// Método para actualizar un curso extracurricular.
        /// </summary>
        /// <param name="model">Objeto con los datos actualizados del curso extracurricular.</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> EditExtracurricularCourse([Bind(Prefix = "Edit")] ExtracurricularCourseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            var anyCourse = await _extracurricularCourseService.GetByCode(model.Code);
            if (anyCourse != null && anyCourse.Id != model.Id)
                return BadRequest("Ya existe un curso con el mismo código.");

            var extracurricularCourse = await _extracurricularCourseService.Get(model.Id.Value);
            extracurricularCourse.Name = model.Name;
            extracurricularCourse.Code = model.Code;
            extracurricularCourse.Description = model.Description;
            extracurricularCourse.Price = model.Price;
            extracurricularCourse.Credits = model.Credits;
            extracurricularCourse.ExtracurricularAreaId = model.AreaId;

            await _extracurricularCourseService.Update(extracurricularCourse);
            return Ok(extracurricularCourse);
        }

        /// <summary>
        /// Método para eliminar un curso extracurricular.
        /// </summary>
        /// <param name="id">identificador del curso extracurricular</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<ActionResult> DeleteExtracurricularCourse(Guid id)
        {
            //Se elimina el categoria
            var extracurricularCourse = await _extracurricularCourseService.Get(id);
            extracurricularCourse.DeletedAt = DateTime.Now;
            await _extracurricularCourseService.Update(extracurricularCourse);
            return Ok();
        }
    }
}
