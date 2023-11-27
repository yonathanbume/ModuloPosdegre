using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.AnnouncementViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System.Globalization;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/anuncios")]
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IBeginningAnnouncementService _beginningAnnouncementService;
        private readonly IUserAnnouncementService _userAnnouncementService;
        private readonly IRolAnnouncementService _rolAnnouncementService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IDataTablesService _dataTablesService;

        public AnnouncementController(IAnnouncementService announcementService,
            IRolAnnouncementService rolAnnouncementService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IDataTablesService dataTablesService,
            IBeginningAnnouncementService beginningAnnouncementService,
            IUserAnnouncementService userAnnouncementService,
            IUserService userService) : base(userService)
        {
            _announcementService = announcementService;
            _rolAnnouncementService = rolAnnouncementService;
            _storageCredentials = storageCredentials;
            _dataTablesService = dataTablesService;
            _beginningAnnouncementService = beginningAnnouncementService;
            _userAnnouncementService = userAnnouncementService;
        }

        /// <summary>
        /// Vista donde se gestionan los anuncion
        /// </summary>
        /// <returns>Vista</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de anuncion
        /// </summary>
        /// <returns>Listado de anuncios</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetAllAnnouncement()
        {
            var announcements = await _announcementService.GetAll();
            var result = announcements.Select(sc => new
            {
                id = sc.Id,
                title = sc.Title,
                description = sc.Description,
                startdate = sc.StartDate.ToLocalDateFormat(),
                enddate = sc.EndDate.ToLocalDateFormat()
            }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del anuncio
        /// </summary>
        /// <param name="id">Identificador del anuncion</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalles/get/{id}")]
        public async Task<IActionResult> GetAnnouncementDetails(Guid id)
        {
            var announcement = await _announcementService.Get(id);
            var result = new AnnouncementViewModel
            {
                Id = announcement.Id,
                Description = announcement.Description,
                Title = announcement.Title,
                StartDate = announcement.StartDate.ToLocalDateFormat(),
                EndDate = announcement.EndDate.ToLocalDateFormat(),
                Pathfile = announcement.Pathfile,
                SelectedRoles = announcement.RolAnnouncements.Select(x => x.RolId).ToList()
            };
            return View(result);
        }

        /// <summary>
        /// Vista donde se edita el anuncio
        /// </summary>
        /// <param name="id">Identificador del anuncio</param>
        /// <returns>Vista edición</returns>
        [HttpGet("editar/get/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var announcement = await _announcementService.Get(id);
            var result = new AnnouncementViewModel
            {
                Id = announcement.Id,
                Description = announcement.Description,
                Title = announcement.Title,
                StartDate = announcement.StartDate.ToLocalDateFormat(),
                EndDate = announcement.EndDate.ToLocalDateFormat(),
                Pathfile = announcement.Pathfile,
                SelectedRoles = announcement.RolAnnouncements.Select(x => x.RolId).ToList()
            };
            return View(result);
        }

        /// <summary>
        /// Vista donde se crean anuncios
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("agregar/get")]
        public IActionResult Add() => View();

        /// <summary>
        /// Método para crear un anuncio
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo anuncio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> AddAnnouncement(AnnouncementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Ocurrió un problema al registrar.");

            var announcement = new Announcement
            {
                Title = model.Title,
                Description = model.Description,
                EndDate = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.EndDate),
                StartDate = ConvertHelpers.DatepickerToUtcDateTime(model.StartDate)
            };

            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (!string.IsNullOrEmpty(announcement.Pathfile))
                    await storage.TryDelete(announcement.Pathfile, ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);
                announcement.Pathfile = await storage.UploadFile(model.Picture.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.ANNOUNCEMENT_FILES, Path.GetExtension(model.Picture.FileName));
            }

            await _announcementService.Insert(announcement);

            var rolAnnouncements = model.SelectedRoles.Select(x => new RolAnnouncement
            {
                Announcement = announcement,
                RolId = x
            }).ToList();

            await _rolAnnouncementService.InsertRange(rolAnnouncements);

            SuccessToastMessage(ConstantHelpers.MESSAGES.SUCCESS.MESSAGE, ConstantHelpers.MESSAGES.SUCCESS.TITLE);
            RedirectToAction("Index");
            return Ok();
        }

        /// <summary>
        /// Método para eliminar el anuncio
        /// </summary>
        /// <param name="id">Identificador del anuncio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post/{id}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Ocurrió un problema al eliminar.");
            var announcement = await _announcementService.Get(id);
            if (announcement.RolAnnouncements != null || announcement.RolAnnouncements.Any())
                await _rolAnnouncementService.DeleteRange(announcement.RolAnnouncements);
            await _announcementService.Delete(announcement);
            return Ok();
        }

        /// <summary>
        /// Método para editar el anuncio
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del anuncio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/post/{id}")]
        public async Task<IActionResult> EditAnnouncement(AnnouncementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Ocurrió un problema al editar.");

            var announcement = await _announcementService.Get(model.Id);
            announcement.Title = model.Title;
            announcement.Description = model.Description;
            announcement.StartDate = ConvertHelpers.DatepickerToUtcDateTime(model.StartDate);
            announcement.EndDate = ConvertHelpers.DatepickerToUtcDateTime(model.EndDate);

            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                if (!string.IsNullOrEmpty(announcement.Pathfile))
                    await storage.TryDelete(announcement.Pathfile, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);

                announcement.Pathfile = await storage.UploadFile(model.Picture.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.ANNOUNCEMENT_FILES,
                    Path.GetExtension(model.Picture.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            if (announcement.RolAnnouncements != null || announcement.RolAnnouncements.Any())
                await _rolAnnouncementService.DeleteRange(announcement.RolAnnouncements);

            if (model.SelectedRoles != null && model.SelectedRoles.Count > 0)
                await _rolAnnouncementService.InsertRange(model.SelectedRoles.Select(x => new RolAnnouncement
                {
                    Announcement = announcement,
                    RolId = x
                }).ToList());

            await _announcementService.Update(announcement);
            SuccessToastMessage(ConstantHelpers.MESSAGES.SUCCESS.MESSAGE, ConstantHelpers.MESSAGES.SUCCESS.TITLE);
            return Ok();
        }

        #region por sistema

        /// <summary>
        /// Vista donde se gestionan los anuncios por sistema
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("por-sistema")]
        public IActionResult Beginning()
        {
            return View();
        }

        /// <summary>
        /// Vista donde se crean los anuncios por sistema
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("por-sistema/agregar")]
        public IActionResult AddBeginning()
        {
            return View();
        }

        /// <summary>
        /// Vista donde se editan los anuncios por sistema
        /// </summary>
        /// <param name="id">identificador del anuncio por sistema</param>
        /// <returns></returns>
        [HttpGet("por-sistema/editar/{id}")]
        public async Task<IActionResult> EditBeginning(Guid id)
        {
            var announcement = await _beginningAnnouncementService.Get(id);
            var model = new BeginningAnnouncementViewModel();
            var roles = await _beginningAnnouncementService.GetRoles(id);

            model.Id = announcement.Id;
            model.Description = announcement.Description;
            model.System = announcement.System;
            model.Type = announcement.Type;
            model.Title = announcement.Title;
            model.StartDate = announcement.StartDate.ToString(ConstantHelpers.FORMATS.DATE);
            model.EndDate = announcement.EndDate.ToString(ConstantHelpers.FORMATS.DATE);

            model.ImageUrl = announcement.ImageUrl;
            model.YoutubeUrl = announcement.YouTubeUrl;
            model.AppearsIn = announcement.AppearsIn;
            model.FileUrl = announcement.FileUrl;
            model.Roles = roles.Select(x => x.RoleId).ToList();
            model.ImgOrVid = !string.IsNullOrEmpty(announcement.ImageUrl) ? ConstantHelpers.ANNOUNCEMENT.IMAGE_OR_VIDEO.IMAGE : ConstantHelpers.ANNOUNCEMENT.IMAGE_OR_VIDEO.VIDEO;

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de anuncios por sistema
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de anuncios por sistema</returns>
        [HttpGet("por-sistema/get")]
        public async Task<IActionResult> GetAnnouncements(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _beginningAnnouncementService.GetDataTable(sentParameters, search);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los detalles del anuncio por sistema
        /// </summary>
        /// <param name="id">Identificador del anuncio por sistema</param>
        /// <returns>Datos del anuncio</returns>
        [HttpGet("por-sistema/get/{id}")]
        public async Task<IActionResult> GetAnnouncements(Guid id)
        {
            var announcement = await _beginningAnnouncementService.Get(id);
            var roles = await _beginningAnnouncementService.GetRoles(id);

            var model = new BeginningAnnouncementViewModel();
            model.Id = announcement.Id;
            model.Description = announcement.Description;
            model.System = announcement.System;
            model.Type = announcement.Type;
            model.Title = announcement.Title;
            model.StartDate = announcement.StartDate.ToDateFormat();
            model.EndDate = announcement.EndDate.ToDateFormat();
            model.Roles = roles.Select(x => x.RoleId).ToList();
            return Ok(model);
        }

        /// <summary>
        /// Método para crear un anuncio por sistema
        /// </summary>
        /// <param name="model">objeto que contiene los datos del nuevo anuncio por sistema</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-sistema/guardar")]
        public async Task<IActionResult> CreateAnnouncement(BeginningAnnouncementViewModel model)
        {
            var picture = "";
            if (model.Image != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                picture = await storage.UploadFile(model.Image.OpenReadStream(), "announcement", Path.GetExtension(model.Image.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }
            var file = "";
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                file = await storage.UploadFile(model.File.OpenReadStream(), "announcement", Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            var announcement = new BeginningAnnouncement
            {
                Description = model.Description,
                System = model.System,
                Type = model.Type,
                Title = model.Title,
                FileUrl = file,
                ImageUrl = picture,
                Status = ConstantHelpers.STATES.INACTIVE,
                YouTubeUrl = model.YoutubeUrl,
                AppearsIn = model.AppearsIn,
                StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate),
                EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate)
            };

            if (model.Roles != null && model.Roles.Where(x => x != "0").Any())
            {
                announcement.BeginningAnnouncementRoles = model.Roles.Select(x => new BeginningAnnouncementRole
                {
                    RoleId = x
                }).ToList();
            }

            //var exist = await _beginningAnnouncementService.AnyInDates(announcement.StartDate, announcement.EndDate, announcement.AppearsIn);

            await _beginningAnnouncementService.Insert(announcement);
            return Ok();
        }

        /// <summary>
        /// Método para editar un anuncio por sistema
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del anuncio por sistema</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-sistema/editar")]
        public async Task<IActionResult> EditAnnouncement(BeginningAnnouncementViewModel model)
        {
            var StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
            var EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate);
            var announcement = await _beginningAnnouncementService.Get(model.Id);

            announcement.Description = model.Description;
            announcement.System = model.System;
            announcement.Type = model.Type;
            announcement.Title = model.Title;
            announcement.YouTubeUrl = model.YoutubeUrl;
            var picture = "";
            if (model.Image != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                picture = await storage.UploadFile(model.Image.OpenReadStream(), "announcement", Path.GetExtension(model.Image.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                if (!string.IsNullOrEmpty(announcement.ImageUrl))
                    await storage.TryDelete(announcement.ImageUrl, "announcement");
                announcement.ImageUrl = picture;
                announcement.YouTubeUrl = "";
            }
            var file = "";
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                file = await storage.UploadFile(model.File.OpenReadStream(), "announcement", Path.GetExtension(model.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                if (!string.IsNullOrEmpty(announcement.FileUrl))
                    await storage.TryDelete(announcement.FileUrl, "announcement");
                announcement.FileUrl = file;
            }
            announcement.Status = ConstantHelpers.STATES.INACTIVE;
            announcement.AppearsIn = model.AppearsIn;
            announcement.StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
            announcement.EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate);

            await _beginningAnnouncementService.RemoveRoles(announcement.Id);


            if (model.Roles != null && model.Roles.Where(x => x != "0").Any())
            {
                announcement.BeginningAnnouncementRoles = model.Roles.Select(x => new BeginningAnnouncementRole
                {
                    RoleId = x
                }).ToList();

            }

            await _beginningAnnouncementService.Update(announcement);

            return Ok();
        }

        /// <summary>
        /// Método para eliminar un anuncio por sistema
        /// </summary>
        /// <param name="id">Identificador del anuncio por sistema</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-sistema/eliminar/{id}")]
        public async Task<IActionResult> DeleteBeginningAnnouncement(Guid id)
        {
            await _beginningAnnouncementService.RemoveRoles(id);
            await _beginningAnnouncementService.DeleteById(id);
            return Ok();
        }
        #endregion

        #region por usuarios

        /// <summary>
        /// Vista donde se gestionan los anuncios por usuario
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("por-usuarios")]
        public IActionResult UserAnnouncement()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de anuncios por usuario
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de anuncios por usuario</returns>
        [HttpGet("por-usuarios/get")]
        public async Task<IActionResult> Get(string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _userAnnouncementService.GetDataTable(sentParameters, search);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los detalles del anuncio por usuario
        /// </summary>
        /// <param name="id">Identificador del anuncio por usuario</param>
        /// <returns>Datos del anuncio por usuario</returns>
        [HttpGet("por-usuarios/get/{id}")]
        public async Task<IActionResult> GetUserAnnouncements(Guid id)
        {
            var announcement = await _userAnnouncementService.Get(id);
            var model = new UserAnnouncementViewModel();
            model.Id = announcement.Id;
            model.Description = announcement.Description;
            model.UserId = announcement.UserId;
            model.FullName = announcement.User.FullName;
            model.Type = announcement.Type;
            model.Title = announcement.Title;
            model.StartDate = announcement.StartDate.ToDateFormat();
            model.EndDate = announcement.EndDate.ToDateFormat();

            return Ok(model);
        }

        /// <summary>
        /// Método para crear un anuncio por usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo anuncio por usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-usuarios/guardar")]
        public async Task<IActionResult> CreateUserAnnouncement(UserAnnouncementViewModel model)
        {
            var announcement = new UserAnnouncement
            {
                Description = model.Description,
                UserId = model.UserId,
                Type = model.Type,
                Title = model.Title,
                StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate),
                EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate)
            };
            var exist = await _userAnnouncementService.AnyInDates(announcement.StartDate, announcement.EndDate);
            if (!exist)
            {
                await _userAnnouncementService.Insert(announcement);
                return Ok();
            }
            else
            {
                return BadRequest("Existe otro anuncio durante las fechas indicadas");
            }
        }

        /// <summary>
        /// Método para editar un anuncio por usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del anuncio por usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-usuarios/editar")]
        public async Task<IActionResult> EditUserAnnouncement(UserAnnouncementViewModel model)
        {
            var StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
            var EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate);
            var exist = await _userAnnouncementService.AnyInDates(StartDate, EndDate, model.Id);
            if (!exist)
            {
                var announcement = await _userAnnouncementService.Get(model.Id);

                announcement.Description = model.Description;
                announcement.UserId = model.UserId;
                announcement.Type = model.Type;
                announcement.Title = model.Title;
                announcement.StartDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
                announcement.EndDate = ConvertHelpers.DatepickerToDatetime(model.EndDate);
                await _userAnnouncementService.Update(announcement);
                return Ok();
            }
            else
            {
                return BadRequest("Existe otro anuncio durante las fechas indicadas");
            }
        }

        /// <summary>
        /// Método para eliminar un anuncio por usuario
        /// </summary>
        /// <param name="id">identificador del anuncio por usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("por-usuarios/eliminar/{id}")]
        public async Task<IActionResult> DeleteUserAnnouncement(Guid id)
        {
            await _userAnnouncementService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de usuarios para usarlos en select
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("por-usuarios/usuarios/get")]
        public async Task<IActionResult> GetUsersSelect2(string term)
        {
            var result = await _userService.GetAllUsersSelect2ServerSide(term);
            return Ok(new { items = result });
        }
        #endregion
    }
}
