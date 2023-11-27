using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using AKDEMIC.INTRANET.Areas.Admin.Models.TutoringAnnouncementViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using System.IO;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/comunicados")]
    public class TutoringAnnouncementController : BaseController
    {
        private readonly ITutoringAnnouncementService _tutoringAnnouncementService;
        private readonly ITutoringAnnouncementRoleService _tutoringAnnouncementRoleService;
        private readonly ITutoringAnnouncementCareerService _tutoringAnnouncementCareerService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IDataTablesService _dataTablesService;

        public TutoringAnnouncementController(ITutoringAnnouncementService tutoringAnnouncementService,
            ITutoringAnnouncementRoleService tutoringAnnouncementRoleService,
            ITutoringAnnouncementCareerService tutoringAnnouncementCareerService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IDataTablesService dataTablesService) : base()
        {
            _tutoringAnnouncementService = tutoringAnnouncementService;
            _tutoringAnnouncementRoleService = tutoringAnnouncementRoleService;
            _tutoringAnnouncementCareerService = tutoringAnnouncementCareerService;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
        }

        /// <summary>
        /// Vista donde se gestionan los comunicados del sistema
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de comunicados 
        /// </summary>
        /// <param name="published">¿Publicado?</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de comunicados</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTutoringAnnouncements(bool? published, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _tutoringAnnouncementService.GetTutoringAnnouncementsDatatable(sentParameters, ConstantHelpers.SYSTEMS.INTRANET, search, published);
            var newData = result.Data.Select(x => new
            {
                x.Id,
                x.Title,
                x.AllRoles,
                x.AllCareers,
                tutoringAnnouncementRoles = x.TutoringAnnouncementRoles.Select(ta => ta.Role.Name).ToList(),
                tutoringAnnouncementCareers = x.TutoringAnnouncementCareers.Select(tc => tc.Career.Name).ToList(),
                displayTime = x.DisplayTime.ToLocalDateTimeFormat(),
                endTime = x.EndTime.ToLocalDateTimeFormat()
            }).ToList();
            var newResult = new DataTablesStructs.ReturnedData<object>
            {
                Data = newData,
                DrawCounter = result.DrawCounter,
                RecordsTotal = result.RecordsTotal,
                RecordsFiltered = result.RecordsFiltered
            };
            return Ok(newResult);
        }

        /// <summary>
        /// Obtiene los datos del comunicado
        /// </summary>
        /// <param name="id">Identificador del comunicado</param>
        /// <returns>Objeto que contiene los datos del comunicado</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetTutoringAnnouncement(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest($"No se encontró un comunicado con el Id {id}");
            var tutoringAnnouncement = await _tutoringAnnouncementService.GetWithCareersAndRolesById(id);
            return Ok(new
            {
                tutoringAnnouncement.Id,
                tutoringAnnouncement.Message,
                tutoringAnnouncement.Title,
                roles = tutoringAnnouncement.TutoringAnnouncementRoles?.Select(x => x.RoleId).ToList(),
                careers = tutoringAnnouncement.TutoringAnnouncementCareers?.Select(x => x.CareerId).ToList(),
                tutoringAnnouncement.AllCareers,
                tutoringAnnouncement.AllRoles,
                displayTime = tutoringAnnouncement.DisplayTime.ToLocalDateTimeFormat(),
                endTime = tutoringAnnouncement.EndTime.ToLocalDateTimeFormat()
            });
        }

        /// <summary>
        /// Método para crear un comunicado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo comunicado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> Create([Bind(Prefix = "Add")] TutoringAnnouncementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var tutoringAnnouncement = new TutoringAnnouncement();
            var startDate = ConvertHelpers.DatetimepickerToUtcDateTime(model.DisplayTime);

            var endDate = ConvertHelpers.DatetimepickerToUtcDateTime(model.EndTime);

            if (startDate > endDate)
            {
                return BadRequest($"Verificar las fechas");
            }
            if (model.File != null)
            {
                var documentFile = model.File;

                if (documentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{documentFile.FileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC / 1024 / 1024}MB");
                }

                var cloudStorageService = new CloudStorageService(_storageCredentials);
                try
                {
                    var uploadFilePath = await cloudStorageService.UploadFile(documentFile.OpenReadStream(),
                        ConstantHelpers.CONTAINER_NAMES.TUTORING_ANNOUNCEMENTS, Path.GetExtension(documentFile.FileName),
                        ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                    tutoringAnnouncement.File = uploadFilePath;
                }
                catch (Exception)
                {
                    return BadRequest($"Hubo un problema al subir el archivo '{documentFile.FileName}'");
                }
            }
            await SaveTutoringAnnouncement(tutoringAnnouncement, model);

            return Ok();
        }

        /// <summary>
        /// Método para editar un comunicado
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del comunicado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> Edit([Bind(Prefix = "Edit")] TutoringAnnouncementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!model.Id.HasValue || model.Id == Guid.Empty)
                return BadRequest($"No se encontró un comunicado con el Id '{model.Id}'");
            var tutoringAnnouncement = await _tutoringAnnouncementService.Get(model.Id.Value);

            var startDate = ConvertHelpers.DatetimepickerToUtcDateTime(model.DisplayTime);

            var endDate = ConvertHelpers.DatetimepickerToUtcDateTime(model.EndTime);

            if (startDate > endDate)
            {
                return BadRequest($"Verificar las fechas");
            }

            if (model.File != null)
            {
                var documentFile = model.File;

                if (documentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{documentFile.FileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC / 1024 / 1024}MB");
                }

                var cloudStorageService = new CloudStorageService(_storageCredentials);
                try
                {
                    var uploadFilePath = await cloudStorageService.UploadFile(documentFile.OpenReadStream(),
                        ConstantHelpers.CONTAINER_NAMES.TUTORING_ANNOUNCEMENTS, Path.GetExtension(documentFile.FileName),
                        ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                    tutoringAnnouncement.File = uploadFilePath;
                }
                catch (Exception)
                {
                    return BadRequest($"Hubo un problema al subir el archivo '{documentFile.FileName}'");
                }
            }
            await SaveTutoringAnnouncement(tutoringAnnouncement, model);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar un comunicado
        /// </summary>
        /// <param name="id">Identificador del comunicado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> DeleteTutoringAnnouncement(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest($"No se encontró un comunicado con el Id '{id}'");
            await _tutoringAnnouncementService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Método para guardar los datos del anuncio
        /// </summary>
        /// <param name="tutoringAnnouncement">Identificador del comunicado</param>
        /// <param name="model">Objeto que contiene los datos actualizados del comunicado</param>
        private async Task SaveTutoringAnnouncement(TutoringAnnouncement tutoringAnnouncement, TutoringAnnouncementViewModel model)
        {
            tutoringAnnouncement.Title = model.Title;
            tutoringAnnouncement.Message = model.Message;
            tutoringAnnouncement.DisplayTime = !string.IsNullOrEmpty(model.DisplayTime)
                ? ConvertHelpers.DatetimepickerToUtcDateTime(model.DisplayTime) : DateTime.UtcNow;

            tutoringAnnouncement.EndTime = !string.IsNullOrEmpty(model.EndTime)
    ? ConvertHelpers.DatetimepickerToUtcDateTime(model.EndTime) : DateTime.UtcNow;

            tutoringAnnouncement.AllCareers = model.AllCareers;
            tutoringAnnouncement.AllRoles = model.AllRoles;
            tutoringAnnouncement.System = ConstantHelpers.SYSTEMS.INTRANET;


            if (model.Id.HasValue)
                await _tutoringAnnouncementService.Update(tutoringAnnouncement);
            else
                await _tutoringAnnouncementService.Insert(tutoringAnnouncement);

            if (!model.AllRoles)
            {
                if (model.TutoringAnnouncementRoleIds.Any())
                {
                    if (model.Id.HasValue)
                        await _tutoringAnnouncementRoleService.DeleteByTutoringAnnouncementId(tutoringAnnouncement.Id);
                    await _tutoringAnnouncementRoleService.InsertRange(
                            model.TutoringAnnouncementRoleIds.Select(x => new TutoringAnnouncementRole
                            {
                                TutoringAnnouncementId = tutoringAnnouncement.Id,
                                RoleId = x
                            }));
                }
            }
            if (!model.AllCareers)
            {
                if (model.TutoringAnnouncementCareerIds.Any())
                {
                    if (model.Id.HasValue)
                        await _tutoringAnnouncementCareerService.DeleteByTutoringAnnouncementId(tutoringAnnouncement.Id);
                    await _tutoringAnnouncementCareerService.InsertRange(
                            model.TutoringAnnouncementCareerIds.Select(x => new TutoringAnnouncementCareer
                            {
                                TutoringAnnouncementId = tutoringAnnouncement.Id,
                                CareerId = x
                            }));
                }
            }
        }
    }
}
