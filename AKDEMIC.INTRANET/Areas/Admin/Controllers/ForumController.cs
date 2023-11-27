using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ForumsViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/foros")]
    public class ForumController : BaseController
    {
        private readonly IForumService _forumService;
        private readonly ICareerService _careerService;
        private readonly IMapper _mapper;
        public ForumController(
            IForumService forumService,
            ICareerService careerService,
            IMapper mapper) : base()
        {
            _forumService = forumService;
            _careerService = careerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Vista donde se gestionan los foros
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de foros
        /// </summary>
        /// <returns>Objeto que contiene el listado de foros</returns>
        [Route("get")]
        public async Task<IActionResult> GetForums()
        {
            var forums = await _forumService.GetAllBySystem(ConstantHelpers.Solution.Intranet);

            var result = forums.Select(s => new
            {
                state = s.Active,
                description = s.Description,
                name = s.Name,
                id = s.Id
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle del foro
        /// </summary>
        /// <param name="Id">identificador del foro</param>
        /// <returns>Objeto que contiene los datos del foro</returns>
        [Route("editar/{Id}")]
        public async Task<IActionResult> GetForumById(Guid Id)
        {
            var careerCount = await _careerService.Count();
            var forum = await _forumService.GetoForumById(Id, careerCount);
            return Ok(forum);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales
        /// </summary>
        /// <returns>Objeto que contiene el listado de escuelas profesionales</returns>
        [HttpGet("carreras")]
        public async Task<IActionResult> GetCareers()
        {
            var careers = await _careerService.GetCareersToForum();

            return Ok(careers);
        }

        /// <summary>
        /// Método para crear un foro
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo foro</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("crear/post")]
        [HttpPost]
        public async Task<IActionResult> CreateForum(ForumViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var modelTemplate = _mapper.Map<ForumTemplate>(model);
            var forumcareers = await _careerService.GetForumCareer(modelTemplate);

            var forum = new Forum
            {
                Active = true,
                Description = model.Description,
                Name = model.Name,
                System = ConstantHelpers.Solution.Intranet,
                ForumCareers = forumcareers
            };
            await _forumService.Insert(forum);
            return Ok();
        }

        /// <summary>
        /// Método para editar un foro
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del foro</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("editar/post")]
        [HttpPost]
        public async Task<IActionResult> UpdateForum(ForumViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var modelTemplate = _mapper.Map<ForumTemplate>(model);
            await _forumService.UpdateForumCompleted(modelTemplate);

            return Ok();

        }

        /// <summary>
        /// Método para eliminar un foro
        /// </summary>
        /// <param name="id">Identificador del foro</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> DeleteForum(Guid id)
        {
            await _forumService.DeleteForum(id);
            return Ok();
        }
    }
}
