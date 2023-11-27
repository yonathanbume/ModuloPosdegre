using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.UniversityAuthorityViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-de-autoridades")]
    public class UniversityAuthorityController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IUniversityAuthorityService _universityAuthorityService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public UniversityAuthorityController(
            IDataTablesService dataTablesService,
            IUniversityAuthorityService universityAuthorityService,
            IUserService userService,
             UserManager<ApplicationUser> userManager,
              IOptions<CloudStorageCredentials> storageCredentials,
              RoleManager<ApplicationRole> roleManager
        ) : base(userService)
        {
            _dataTablesService = dataTablesService;
            _universityAuthorityService = universityAuthorityService;
            _userManager = userManager;
            _storageCredentials = storageCredentials;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Vista donde se gestionan las autoridades
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de autoridades de la universidad
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de autoridades</returns>
        [HttpGet("obtener-autoridades")]
        public async Task<IActionResult> GetAbilites(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _universityAuthorityService.GetUniversityAuthority(sentParameters, searchValue);

            return Ok(result);
        }

        /// <summary>
        /// Método para crear una autoridad
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva autoridad</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar-autoridad")]
        public async Task<IActionResult> SaveAbilities(UniversityAuthorityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Hubo un problema al momento de agregar la autoridad");
            }
            if (await _universityAuthorityService.ExistAuthorityType(model.Type, null))
            {
                return BadRequest("Ya se encuentra un registro con el mismo tipo ingresado");
            }
            if (model.File == null)
                return BadRequest("Por favor seleccione un archivo.");
            //crear roles
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.VICERRECTOR });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.RECTOR });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.GENERAL_SECRETARY });

            //asignar rol al usuario
            var user = await _userService.Get(model.UserId);
            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.RECTOR)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.RECTOR);
            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.VICERRECTOR)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.VICERRECTOR);
            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.GENERAL_SECRETARY)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.GENERAL_SECRETARY);

            var newEntity = new UniversityAuthority
            {
                UserId = model.UserId,
                Type = model.Type
            };
            newEntity.UniversityAuthorityHistories = new List<UniversityAuthorityHistory>();

            var history = new UniversityAuthorityHistory()
            {
                UserId = model.UserId,
                Type = model.Type,
                ResolutionDate = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.ResolutionDate),
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                history.FileUrl = await storage.UploadFile(model.File.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            newEntity.UniversityAuthorityHistories.Add(history);

            await _universityAuthorityService.Insert(newEntity);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar una autoridad
        /// </summary>
        /// <param name="id">Identificador de la autoridad</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> DeleteDeanFaculty(Guid id)
        {
            await _universityAuthorityService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene los datos de la autoridad
        /// </summary>
        /// <param name="id">Identificador de la autoridad</param>
        /// <returns>Objeto que contiene los datos de la autoridad</returns>
        [HttpGet("obtener-autoridad/{id}")]
        public async Task<IActionResult> GetAbility(Guid id)
        {
            var universityAuthority = await _universityAuthorityService.Get(id);
            var result = new
            {
                fullName = $"{universityAuthority.User.Name} {universityAuthority.User.PaternalSurname} {universityAuthority.User.MaternalSurname}",
                universityAuthority.UserId,
                universityAuthority.Type,
                universityAuthority.Id
            };
            return Ok(result);
        }

        /// <summary>
        /// Método para editar una autoridad
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la autoridad</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar-autoridad")]
        public async Task<IActionResult> EditAbility(UniversityAuthorityViewModel model)
        {
            var universityAuthority = await _universityAuthorityService.Get(model.Id);

            if (await _universityAuthorityService.ExistAuthorityType(model.Type, model.Id))
            {
                return BadRequest("Ya se encuentra un registro con el mismo tipo ingresado");
            }
            if (model.File == null)
                return BadRequest("Por favor seleccione un archivo.");
            universityAuthority.UserId = model.UserId;
            universityAuthority.Type = model.Type;

            //crear roles
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.VICERRECTOR });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.RECTOR });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.GENERAL_SECRETARY });

            //asignar rol al usuario
            var user = await _userService.Get(model.UserId);
            await _userManager.RemoveFromRoleAsync(user, ConstantHelpers.ROLES.RECTOR);
            await _userManager.RemoveFromRoleAsync(user, ConstantHelpers.ROLES.VICERRECTOR);
            await _userManager.RemoveFromRoleAsync(user, ConstantHelpers.ROLES.GENERAL_SECRETARY);

            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.RECTOR)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.RECTOR);
            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.VICERRECTOR)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.VICERRECTOR);
            if (model.Type == ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.GENERAL_SECRETARY)
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.GENERAL_SECRETARY);

            if (universityAuthority.UniversityAuthorityHistories == null)
                universityAuthority.UniversityAuthorityHistories = new List<UniversityAuthorityHistory>();

            var history = new UniversityAuthorityHistory()
            {
                UserId = model.UserId,
                Type = model.Type,
                ResolutionDate = ConvertHelpers.DatepickerToUtcDateTime(model.ResolutionDate)
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                history.FileUrl = await storage.UploadFile(model.File.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.INTRANET_FILES,
                    Path.GetExtension(model.File.File.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            universityAuthority.UniversityAuthorityHistories.Add(history);

            await _universityAuthorityService.Update(universityAuthority);
            return Ok();
        }

        /// <summary>
        /// Obtiene el historial de cambios de autoridad
        /// </summary>
        /// <param name="id">identificador del tipo de autoridad</param>
        /// <returns>Objeto que contiene el historial</returns>
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var result = await _universityAuthorityService.GetUniversityAuthorityHistory(id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [HttpGet("usuarios/get")]
        public async Task<IActionResult> GetAuthoritiesSelect2(string term)
        {
            var result = await _userService.GetUsersAuthoritySelect2ServerSide(term);
            return Ok(new { items = result });
        }
    }
}
