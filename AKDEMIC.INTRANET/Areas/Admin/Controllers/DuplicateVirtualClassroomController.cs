using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-duplicado-aula")]
    public class DuplicateVirtualClassroomController : BaseController
    {
        public readonly ISectionsDuplicateContentService _sectionsDuplicateContentService;
        public DuplicateVirtualClassroomController(
            ISectionsDuplicateContentService sectionsDuplicateContentService,
            IUserService userService,
            IDataTablesService dataTablesService) : base(userService, dataTablesService)
        {
            _sectionsDuplicateContentService = sectionsDuplicateContentService;
        }

        /// <summary>
        /// Vista donde se gestiona el contenido duplicado en aula virtual
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de contenido duplicado en las secciones aperturadas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Listado de contenido</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetSectionsDuplicateContent(Guid? termId, Guid? careerId)
        {
            if (termId == Guid.Empty)
            {
                var term = await _termService.GetActiveTerm();
                if (term == null) await _termService.GetLastTerm();
                termId = term.Id;
            }
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _sectionsDuplicateContentService.GetSectionsDuplicateContentDatatable(sentParameters, termId, careerId);

            return Ok(result);
        }

        /// <summary>
        /// Método para crear un duplicado de contenido
        /// </summary>
        /// <param name="sectionAid">identificador de la seccion A</param>
        /// <param name="sectionBid">Identificador de la sección B</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar")]
        public async Task<IActionResult> SaveDeanFaculty(Guid sectionAid, Guid sectionBid)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Hubo un problema al momento de agregar el decano");
            }
            if (await _sectionsDuplicateContentService.AnySectionASectionB(sectionAid, sectionBid))
            {
                return BadRequest("Ya existe la sección registrada");
            }
            var newEntity = new SectionsDuplicateContent
            {
                SectionAId = sectionAid,
                SectionBId = sectionBid
            };
            await _sectionsDuplicateContentService.InsertSectionsDuplicateContent(newEntity);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar un duplicado de contenido
        /// </summary>
        /// <param name="sectionAid">identificador de la seccion A</param>
        /// <param name="sectionBid">Identificador de la sección B</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/{sectionAid}/{sectionBid}")]
        public async Task<IActionResult> DeleteDeanFaculty(Guid sectionAid, Guid sectionBid)
        {
            var entity = await _sectionsDuplicateContentService.GetBySectionAandB(sectionAid, sectionBid);
            await _sectionsDuplicateContentService.DeleteSectionsDuplicateContent(entity);
            return Ok();
        }
    }
}
