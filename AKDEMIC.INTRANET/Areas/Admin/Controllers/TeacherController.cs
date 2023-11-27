using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.TeacherViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Services;
using System.IO;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using iTextSharp.text.pdf.qrcode;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN )]
    [Area("Admin")]
    [Route("admin/docentes")]
    public class TeacherController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly ITeacherService _teacherService;
        private readonly ICareerService _careerService;

        public TeacherController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            RoleManager<ApplicationRole> roleManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            ITeacherService teacherService, ICareerService careerService)
                : base(userManager, userService, dataTablesService)
        {
            _storageCredentials = storageCredentials;
            _teacherService = teacherService;
            _careerService = careerService;
        }

        /// <summary>
        /// Vista donde se gestionan los docentes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista donde de crea un docente
        /// </summary>
        /// <returns>Vista registro</returns>
        [HttpGet("registrar")]
        public IActionResult Add()
        {
            var model = new TeacherViewModel();
            return View(model);
        }

        /// <summary>
        /// Método para editar los datos del docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <param name="model">Objeto que contiene los datos actualizados del docente</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Edit(string id, TeacherViewModel model)
        {
            var ids = ModelState.Where(x => x.Key.Equals("Name") || x.Key.Equals("MaternalSurname") || x.Key.Equals("PaternalSurname"));
            foreach (var item in ids)
            {
                ModelState[item.Key].Errors.Clear();
                ModelState[item.Key].ValidationState = ModelValidationState.Valid;
            }
            if (!ModelState.IsValid)
                return BadRequest("Revise la información ingresada");
            if (ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            var passwordIsValid = false;

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password.Length < 6) return BadRequest("La contraseña debe tener 6 caracteres como mínimo.");
                else
                {
                    var passwordValidator = new PasswordValidator<ApplicationUser>();
                    passwordIsValid = passwordValidator.ValidateAsync(_userManager, new ApplicationUser(), model.Password).Result.Succeeded;
                    if (!passwordIsValid) return BadRequest("La contraseña debe contener al menos 1 letra mayúscula, 1 letra minúscula, 1 dígito y un caracter no alfanumérico.");
                }
            }

            var user = await _userService.Get(id);

            var userByUsername = await _userService.GetByUserName(model.UserName);
            if (userByUsername != null && userByUsername.Id != user.Id) return BadRequest("El usuario especificado ya se encuentra registrado.");

            var anyWithEmail = await _userService.AnyWithSameEmail(user.Id, model.Email);
            if (anyWithEmail) return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            user.Name = model.Name;
            user.MaternalSurname = model.MaternalSurname;
            user.PaternalSurname = model.PaternalSurname;
            user.FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")} {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}";

            user.Email = model.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.NormalizedUserName = _userManager.NormalizeName(model.UserName);
            user.Address = model.Address;
            user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);
            user.Sex = model.Sex;
            user.Dni = model.Dni;
            user.Document = model.Dni;

            user.Type = ConstantHelpers.USER_TYPES.TEACHER;
            if (passwordIsValid)
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

            var teacher = await _teacherService.GetByUserId(user.Id);
            teacher.AcademicDepartmentId = model.AcademicDepartmentId;

            await _userService.Update(user);

            return await Save(model, user);
        }

        /// <summary>
        /// Método para registrar un docente
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo docente</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> Add(TeacherViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Revise la información ingresada");
            if (ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            if (string.IsNullOrEmpty(model.Password)) return BadRequest("La contraseña es obligatoria.");
            else if (model.Password.Length < 6) return BadRequest("La contraseña debe tener 6 caracteres como mínimo.");
            else
            {
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var passwordIsValid = passwordValidator.ValidateAsync(_userManager, new ApplicationUser(), model.Password).Result.Succeeded;
                if (!passwordIsValid) return BadRequest("La contraseña debe contener al menos 1 letra mayúscula, 1 letra minúscula, 1 dígito y un caracter no alfanumérico.");
            }

            var userUsername = await _userService.GetByUserName(model.UserName);
            if (userUsername != null) return BadRequest("El usuario especificado ya se encuentra registrado.");

            var anyWithEmail = await _userService.AnyWithSameEmail(null, model.Email);
            if (anyWithEmail) return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            var user = new ApplicationUser();
            FillApplicationUser(ref user, model);
            await _userManager.CreateAsync(user, model.Password);

            var teacher = new ENTITIES.Models.Generals.Teacher
            {
                UserId = user.Id,
                //Status = ConstantHelpers.    
                TeacherInformation = new ENTITIES.Models.TeachingManagement.TeacherInformation()
                {
                    Resolution = new ENTITIES.Models.Enrollment.Resolution(),
                },
                AcademicDepartmentId = model.AcademicDepartmentId
            };

            await _teacherService.AddAsync(teacher);

            return await Save(model, user);
        }

        /// <summary>
        /// Método para guardar los datos del docente
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del docente</param>
        /// <param name="user">Entidad usuario</param>
        /// <returns>Código de estado HTTP</returns>
        private async Task<IActionResult> Save(TeacherViewModel model, ApplicationUser user)
        {
            // Update user roles
            if (!await _userManager.IsInRoleAsync(user, ConstantHelpers.ROLES.TEACHERS))
                await _userService.AddToRole(user, ConstantHelpers.ROLES.TEACHERS);

            // Upload Picture
            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                if (!string.IsNullOrEmpty(user.Picture)) await storage.TryDelete(user.Picture, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);

                user.Picture = await storage.UploadFile(model.Picture.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE,
                    Path.GetExtension(model.Picture.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _userService.Update(user);

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [Route("get")]
        public async Task<IActionResult> GetTeachers(string search = null, Guid? academicDepartmentId = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _teacherService.GetIntranetTeachersDatatable(sentParameters, academicDepartmentId, search);
            return Ok(result);
        }

        /// <summary>
        /// Guarda los datos actualizados en la entidad docente
        /// </summary>
        /// <param name="user">Entidad docente</param>
        /// <param name="model">Objeto que contiene los datos actualizados del docente</param>
        private void FillApplicationUser(ref ApplicationUser user, TeacherViewModel model)
        {
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.MaternalSurname = model.MaternalSurname;
            user.PaternalSurname = model.PaternalSurname;
            user.FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")} {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}";
            user.Address = model.Address;
            user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);
            user.Sex = model.Sex;
            user.Dni = model.Dni;
            user.Document = model.Dni;
            user.IsActive = true;
            user.Type = ConstantHelpers.USER_TYPES.TEACHER;
        }

        /// <summary>
        /// Vista donde se edita los datos del docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Vista edición</returns>
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.Get(id);
            var teacher = await _teacherService.GetByUserId(user.Id);
            var roles = await _userService.GetRoles(user);

            var model = new TeacherViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                PaternalSurname = user.PaternalSurname,
                MaternalSurname = user.MaternalSurname,
                Name = user.Name,
                PicturePath = user.Picture,
                Dni = user.Dni,
                Sex = user.Sex,
                BirthDate = user.BirthDate.ToString("dd/MM/yyyy"),
                Address = user.Address,
                AcademicDepartmentId = teacher.AcademicDepartmentId,
                Roles = string.Join(", ", roles.ToList()),
                UserWeb = user.UserWeb
            };

            return View(model);
        }

        /// <summary>
        /// Método para eliminar a un docente
        /// </summary>
        /// <param name="id">identificador del docente</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Get(id);

            var teacher = await _teacherService.GetTeacherWithData(id);
            await _teacherService.DeleteAsync(teacher);

            var rolesToRemove = await _userService.GetRoles(user);
            await _userService.RemoveFromRoles(user, rolesToRemove);
            if (!string.IsNullOrEmpty(user.Picture))
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(user.Picture, ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);
            }
            await _userService.Delete(user);
            return Ok();
        }
    }
}
