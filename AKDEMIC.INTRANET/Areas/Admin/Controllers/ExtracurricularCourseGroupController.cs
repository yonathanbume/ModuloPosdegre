using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularCourseGroupViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/grupos-extracurriculares")]
    public class ExtracurricularCourseGroupController : BaseController
    {
        private readonly IExtracurricularCourseGroupService _extracurricularCourseGroupService;
        private readonly IExtracurricularCourseGroupStudentService _extracurricularCourseGroupStudentService;

        public ExtracurricularCourseGroupController(IDataTablesService dataTablesService,
            IExtracurricularCourseGroupService extracurricularCourseGroupService,
            IExtracurricularCourseGroupStudentService extracurricularCourseGroupStudentService) : base(dataTablesService)
        {
            _extracurricularCourseGroupService = extracurricularCourseGroupService;
            _extracurricularCourseGroupStudentService = extracurricularCourseGroupStudentService;
        }

        /// <summary>
        /// Vista donde se gestionan los grupos de curso extracurricular
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de grupos de cursos extracurriculares
        /// </summary>
        /// <returns>Objeto que contiene el listado de grupos de cursos extracurriculares</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExtracurricularCourseGroups(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _extracurricularCourseGroupService.GetDataDatatable(sentParameters, search);
            return Ok(result);
        }

        [HttpGet("alumnos/get")]
        public async Task<IActionResult> GetExtracurricularCourseGroups(Guid id)
        {
            var students = await _extracurricularCourseGroupStudentService.GetAllByGroup(id, ConstantHelpers.PAYMENT.STATUS.PAID);

            var results = students
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Student.User.FullName,
                    username = x.Student.UserId
                })
                .ToList();

            return Ok(results);
        }

        /// <summary>
        /// Método para crear un nuevo grupo de curso extracurricular.
        /// </summary>
        /// <param name="model">Objeto con los datos del categoria.</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> CreateExtracurricularCourseGroup([Bind(Prefix = "Add")] ExtracurricularCourseGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Verifique los  valores colocados.");

            var anyGroup = await _extracurricularCourseGroupService.GetByCode(model.Code);
            if (anyGroup != null)
                return BadRequest("Ya existe un grupo con el mismo código.");

            var extracurricularCourseGroup = new ExtracurricularCourseGroup
            {
                Code = model.Code,
                TeacherId = model.TeacherId,
                ExtracurricularCourseId = model.ExtracurricularCourseId,
                TermId = model.TermId,
            };

            await _extracurricularCourseGroupService.Insert(extracurricularCourseGroup);
            return Ok();
        }

        /// <summary>
        /// Retorna el detalle de un grupo en base a su Identificador.
        /// </summary>
        /// <param name="id">Identificador del grupo de curso extracurricular</param>
        /// <returns>Objeto que contiene los datos del grupo</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetExtracurricularCourseGroup(Guid id)
        {
            var course = await _extracurricularCourseGroupService.GetWithIncludes(id);
            var result = new
            {
                id = course.Id,
                code = course.Code,
                teacherid = course.TeacherId,
                teacherName = course.Teacher.User.FullName,
                extracurricularcourseid = course.ExtracurricularCourseId,
                termId = course.TermId
            };
            return Ok(result);
        }

        /// <summary>
        /// Método para actualizar un grupo de curso extracurricular.
        /// </summary>
        /// <param name="model">Objeto con los datos actualizados del grupo de curso extracurricular.</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("editar/post")]
        [HttpPost]
        public async Task<IActionResult> EditExtracurricularCourseGroup([Bind(Prefix = "Edit")] ExtracurricularCourseGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Verifique los  valores colocados.");

            var anyGroup = await _extracurricularCourseGroupService.GetByCode(model.Code);
            if (anyGroup != null && anyGroup.Id != model.Id)
                return BadRequest("Ya existe un curso con el mismo código.");

            var extracurricularCourseGroup = await _extracurricularCourseGroupService.Get(model.Id.Value);
            extracurricularCourseGroup.Code = model.Code;
            extracurricularCourseGroup.TeacherId = model.TeacherId;
            extracurricularCourseGroup.ExtracurricularCourseId = model.ExtracurricularCourseId;
            extracurricularCourseGroup.TermId = model.TermId;
            await _extracurricularCourseGroupService.Update(extracurricularCourseGroup);
            return Ok(extracurricularCourseGroup);
        }

        /// <summary>
        /// Método para eliminar un grupo
        /// </summary>
        /// <param name="id">identificador del grupo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<ActionResult> DeleteExtracurricularCourseGroup(Guid id)
        {
            //Se elimina el categoria
            var extracurricularCourseGroup = await _extracurricularCourseGroupService.Get(id);
            await _extracurricularCourseGroupService.DeleteById(extracurricularCourseGroup.Id);
            //await _extracurricularCourseGroupService.Update(extracurricularCourseGroup);
            return Ok();
        }
    }
}
