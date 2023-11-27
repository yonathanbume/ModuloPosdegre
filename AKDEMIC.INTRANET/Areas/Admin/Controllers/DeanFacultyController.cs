using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.DeanFacultiesViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-de-decanos")]
    public class DeanFacultyController : BaseController
    {
        public readonly IDeanFacultyService _deanFacultyService;
        private readonly IOptions<CloudStorageCredentials> _storageCredential;
        public readonly IDeanService _deanService;
        public readonly ISelect2Service _select2Service;
        protected readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public readonly IFacultyService _facultyService;
        public DeanFacultyController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeanFacultyService deanFacultyService,
            IOptions<CloudStorageCredentials> storageCredential,
            IDeanService deanService,
            ISelect2Service select2Service,
            IUserService userService,
            IDataTablesService dataTablesService,
            IFacultyService facultyService) : base(userService, dataTablesService)
        {
            _deanFacultyService = deanFacultyService;
            _storageCredential = storageCredential;
            _deanService = deanService;
            _facultyService = facultyService;
            _select2Service = select2Service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Vista donde se gestionan los decanos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de facultades donde se detalla su decano asignado
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Listado de facultades</returns>
        [HttpGet("obtener-decanos")]
        public async Task<IActionResult> GetDeanFaculties(Guid? facultyId, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _facultyService.GetFacultiesDatatable(sentParameters, searchValue);
            //var result = await _deanFacultyService.GetDeanFacultiesDatatable(sentParameters, facultyId, searchValue);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los datos del decano asignado por facultad
        /// </summary>
        /// <param name="id">Identificador de la facultad</param>
        /// <returns>Datos del decano</returns>
        [HttpGet("obtener-decano/{id}")]
        public async Task<IActionResult> GetAbility(Guid id)
        {
            var deanFaculty = await _facultyService.Get(id);

            var dean = await _userService.Get(deanFaculty.DeanId);
            var deanSec = await _userService.Get(deanFaculty.SecretaryId);
            var assistance = await _userService.Get(deanFaculty.AdministrativeAssistantId);
            if (dean != null)
            {
                deanFaculty.Dean = new ApplicationUser
                {
                    FullName = dean.FullName
                };
            }
            if (deanSec != null)
            {
                deanFaculty.Secretary = new ApplicationUser
                {
                    FullName = deanSec.FullName
                };
            }
            if (assistance != null)
            {
                deanFaculty.AdministrativeAssistant = new ApplicationUser
                {
                    FullName = assistance.FullName
                };
            }
            return Ok(deanFaculty);
        }

        /// <summary>
        /// Método para editar un decano
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del decano</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar-decano")]
        public async Task<IActionResult> EditDeanFaculty(DeanFacultyViewModel model)
        {
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.DEAN });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.DEAN_SECRETARY });
            await _roleManager.CreateAsync(new ApplicationRole { Name = ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT });

            var storage = new CloudStorageService(_storageCredential);

            var faculty = await _facultyService.GetWithHistory(model.Id);

            if (!string.IsNullOrEmpty(faculty.DeanId))
            {
                var olddean = await _userManager.FindByIdAsync(faculty.DeanId);
                if (olddean != null)
                {
                    await _userManager.RemoveFromRoleAsync(olddean, ConstantHelpers.ROLES.DEAN);
                    var ddd = await _deanService.Get(olddean.Id);
                    if (ddd != null)
                        await _deanService.DeleteById(olddean.Id);
                }
            }

            if (!string.IsNullOrEmpty(faculty.SecretaryId))
            {
                var oldSecretary = await _userManager.FindByIdAsync(faculty.SecretaryId);
                if (oldSecretary != null)
                    await _userManager.RemoveFromRoleAsync(oldSecretary, ConstantHelpers.ROLES.DEAN_SECRETARY);
            }

            if (!string.IsNullOrEmpty(faculty.AdministrativeAssistantId))
            {
                var oldAssistant = await _userManager.FindByIdAsync(faculty.SecretaryId);
                if (oldAssistant != null)
                    await _userManager.RemoveFromRoleAsync(oldAssistant, ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT);
            }

            faculty.DeanGrade = model.DeanGrade;
            faculty.DeanId = model.DeanId;
            faculty.SecretaryId = model.SecretaryId;
            faculty.DeanResolution = model.Resolution;
            faculty.AdministrativeAssistantId = model.AdministrativeAssistantId;

            if (model.ResolutionFile != null)
            {
                var extension = Path.GetExtension(model.ResolutionFile.FileName);
                faculty.DeanResolutionFile = await storage.UploadFile(model.ResolutionFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS, extension, CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var lastHistory = faculty.FacultyHistories.Where(x => x.FacultyId == model.Id).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            if (lastHistory is null || (lastHistory != null && (lastHistory.SecretaryId != model.SecretaryId ||
                lastHistory.DeanId != model.DeanId ||
                lastHistory.DeanResolution != model.Resolution ||
                lastHistory.DeanResolutionFile != faculty.DeanResolutionFile)))
            {
                var deanfaculty = new DeanFaculty
                {
                    FacultyId = model.Id,
                    DeanId = model.DeanId,
                    SecretaryId = model.SecretaryId,
                    DeanResolutionFile = faculty.DeanResolutionFile,
                    DeanResolution = model.Resolution
                };

                await _deanFacultyService.Insert(deanfaculty);
            }

            await _facultyService.Update(faculty);

            if (!string.IsNullOrEmpty(model.DeanId))
            {
                var newdean = await _userManager.FindByIdAsync(faculty.DeanId);
                await _userManager.AddToRoleAsync(newdean, ConstantHelpers.ROLES.DEAN);
            }
            if (!string.IsNullOrEmpty(model.SecretaryId))
            {
                var newSecretary = await _userManager.FindByIdAsync(faculty.SecretaryId);
                await _userManager.AddToRoleAsync(newSecretary, ConstantHelpers.ROLES.DEAN_SECRETARY);
            }
            if (!string.IsNullOrEmpty(model.AdministrativeAssistantId))
            {
                var newAssitant = await _userManager.FindByIdAsync(faculty.AdministrativeAssistantId);
                await _userManager.AddToRoleAsync(newAssitant, ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT);
            }
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de facultades
        /// </summary>
        /// <returns>Listado de facultades</returns>
        [HttpGet("obtener-facultades")]
        public async Task<IActionResult> GetFaculties()
        {
            var result = await _facultyService.GetAllAsSelect2ClientSide2();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de facultades
        /// </summary>
        /// <returns>Listado de facultades</returns>
        [HttpGet("obtener-facultades_todas")]
        public async Task<IActionResult> GetFacultiesAll()
        {
            var result = await _facultyService.GetAllAsSelect2ClientSide2(true);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el historial de cambios de decano
        /// </summary>
        /// <param name="id">Identificador de la facultad</param>
        /// <returns>Historial de cambios</returns>
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var Histories = await _deanFacultyService.GetByFaculty(id);

            var filterRecords = Histories.Count;

            var result = new
            {
                draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                recordsTotal = filterRecords,
                recordsFiltered = filterRecords,
                data = Histories
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("obtener-usuarios")]
        public async Task<IActionResult> GetUsers(string term)
        {
            var sentParameters = _select2Service.GetRequestParameters();
            var result = await _userService.Select2WithOutStudentRole(sentParameters, term);
            return Ok(new { items = result });
        }
    }
}
