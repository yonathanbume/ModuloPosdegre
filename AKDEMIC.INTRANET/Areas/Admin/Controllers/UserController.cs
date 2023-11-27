using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.UserViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AKDEMIC.REPOSITORY.Data;
using Newtonsoft.Json;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/usuarios")]
    public class UserController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;
        private readonly IUserDependencyService _userDependencyService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(IUserService userService, IRoleService roleService,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            IDataTablesService dataTablesService,
            AkdemicContext context,
            IUserDependencyService userDependencyService)
            : base(userManager, userService, roleService, dataTablesService)
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _userDependencyService = userDependencyService;
            _roleManager = roleManager;
        }

        #region --- JSON ---

        /// <summary>
        /// Obtiene el listado de usuarios con rol docente y estudiante para ser usado en tablas
        /// </summary>
        /// <param name="status">Estado</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetApplicationUsers(int status, string search, string rolesJson)
        {
            var roles = JsonConvert.DeserializeObject<List<string>>(rolesJson);
            var sentParameters = GetSentParameters();
            var result = await _userService.GetUsersDatatableByType(sentParameters, ConstantHelpers.USER_TYPES.ADMINISTRATIVE, search, roles);
            //var result = await _userService.GetUsersDatatable(sentParameters, search, null, new List<string>
            //{
            //    ConstantHelpers.ROLES.STUDENTS,
            //    ConstantHelpers.ROLES.TEACHERS
            //});
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios 
        /// </summary>
        /// <returns>Objeto que contiene el listado de usuarios</returns>
        [Route("2/get")]
        public async Task<IActionResult> GetApplicationUsers()
        {
            var result = await _context.Users
                .Select(x => new
                {
                    id = x.Id,
                    fullName = x.FullName
                })
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de roles asignados al usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Objeto que contiene el listado de roles</returns>
        [HttpGet("{id}/roles/get")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _userService.Get(id);
                if (user != null)
                {
                    var roleNames = await _userService.GetRoles(user);
                    var roles = await _roleService.GetAllByName(roleNames);
                    var result = roles.Select(x => new
                    {
                        id = x.Id,
                        text = x.Name
                    }).ToList();
                    return Ok(new { items = result });
                }
            }
            return BadRequest("Id de usuario inválido");
        }

        /// <summary>
        /// Método para crear el rol de 'Restablecer Clave'
        /// </summary>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("roles/crear")]
        public async Task<IActionResult> CreatePendingRoles()
        {
            var roles = new List<string>
            {
                ConstantHelpers.ROLES.PASSWORD_EDITOR
            };

            foreach (var item in roles)
            {
                if (!await _roleManager.RoleExistsAsync(item))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = item
                    });
                }
            }
            return Ok("Roles creados");
        }

        /// <summary>
        /// Obtiene el listado de dependencias asociadas al usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Objeto que contiene el listado de dependencias</returns>
        [HttpGet("{id}/dependencias/get")]
        public async Task<IActionResult> GetUserDependencies(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var userDependencies = await _userDependencyService.GetUserDependenciesByUser(id);
                var result = userDependencies.Select(x => new
                {
                    id = x.DependencyId,
                    text = x.Dependency.Name
                }).ToList();
                return Ok(new { items = result });
            }

            return BadRequest("Id de usuario inválido");
        }

        /// <summary>
        /// Método para validar si el correo existe
        /// </summary>
        /// <param name="email">Correo electrónico</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna un booleano</returns>
        [HttpGet("validar-correo")]
        public async Task<IActionResult> CheckEmailExist(string email, string userId)
        {
            var anyUser = false;

            if (!string.IsNullOrEmpty(userId)) anyUser = await _userService.AnyByEmail(email, userId);
            else anyUser = await _userService.AnyByEmail(email);

            return Ok(anyUser);
        }

        [HttpGet("get-usuarios-externos/datatable")]
        public async Task<IActionResult> GetExternalUsersDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetUsersDatatableByType(parameters, ConstantHelpers.USER_TYPES.EXTERNAL, search);
            return Ok(result);
        }

        #endregion

        #region --- ACTIONS ---

        [HttpGet("gestion")]
        public IActionResult UserManagement()
            => View();

        /// <summary>
        /// Vista donde se gestionan los usuarios
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet]
        public IActionResult Index() => View();

        /// <summary>
        /// Vista donde se registra un usuario
        /// </summary>
        /// <returns>Vista registro</returns>
        [HttpGet("registrar")]
        public IActionResult Add() => View();

        /// <summary>
        /// Vista donde se edita un usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Vista de edición</returns>
        [HttpGet("{id}/editar")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ApplicationException($"No se pudo encontrar el usuario con el id {id}.");

                var user = await _userService.Get(id);
                var model = new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    PaternalSurname = user.PaternalSurname,
                    MaternalSurname = user.MaternalSurname,
                    Name = user.Name,
                    Address = user.Address,
                    Sex = user.Sex,
                    BirthDate = user.BirthDate.ToLocalDateFormat(),
                    Dni = user.Dni,
                    IsActive = user.IsActive,
                    PictureUrl = user.Picture
                };

                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Vista donde se detalla la información del usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ApplicationException($"No se pudo encontrar el usuario con el id {id}.");

                var user = await _userService.Get(id);
                var model = new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    PaternalSurname = user.PaternalSurname,
                    MaternalSurname = user.MaternalSurname,
                    Name = user.Name,
                    Address = user.Address,
                    Sex = user.Sex,
                    BirthDate = user.BirthDate.ToLocalDateFormat(),
                    Dni = user.Dni,
                    PictureUrl = user.Picture
                };

                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método para registrar un usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> Add(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Por favor, complete los campos obligatorios.");
            if (ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            if (string.IsNullOrEmpty(model.Password))
                return BadRequest("La contraseña es obligatoria.");
            else if (model.Password.Length < 6)
                return BadRequest("La contraseña debe tener 6 caracteres como mínimo.");

            var otherUser = await _userService.GetByUserName(model.UserName);
            if (otherUser != null)
                return BadRequest("El usuario especificado ya se encuentra registrado.");

            var deletedUser = await _userService.AnyByUserNameIgnoreQueryFilter(model.UserName);
            if (deletedUser)
                return BadRequest("Ya existe un usuario eliminado con el mismo código.");

            //var anotherEmail = await _userService.AnyByEmail(model.Email);
            //if (anotherEmail)
            //    return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            var officeRol = await _roleService.GetByName(ConstantHelpers.ROLES.OFFICE);
            var dependencyRol = await _roleService.GetByName(ConstantHelpers.ROLES.DEPENDENCY);
            var costCenterRol = await _roleService.GetByName(ConstantHelpers.ROLES.COST_CENTER);

            if (!model.SelectedRoles.Any(x => x == officeRol.Id || x == dependencyRol.Id || x == costCenterRol.Id) && (model.SelectedDependencies != null && model.SelectedDependencies.Count > 0))
            {
                return BadRequest("No se pueden agregar dependencias a este usuario");
            }
            if (model.SelectedRoles.Any(x => x == officeRol.Id || x == dependencyRol.Id || x == costCenterRol.Id) && (model.SelectedDependencies == null || model.SelectedDependencies.Count == 0))
            {
                return BadRequest("El campo de dependencia se encuentra incompleto");
            }

            var user = new ApplicationUser();

            user.Name = model.Name;
            user.MaternalSurname = model.MaternalSurname;
            user.PaternalSurname = model.PaternalSurname;
            user.FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")} {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}";
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            user.Address = model.Address;
            user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);
            user.Sex = model.Sex;
            user.Dni = model.Dni;
            user.Document = model.Dni;
            user.Type = ConstantHelpers.USER_TYPES.ADMINISTRATIVE;

            foreach (IPasswordValidator<ApplicationUser> passwordValidator in _userManager.PasswordValidators)
            {
                var result = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(string.Join("; ", result.Errors.Select(x => x.Description).ToList()));
            }


            await _userManager.CreateAsync(user, model.Password);

            return await Save(model, user);
        }

        /// <summary>
        /// Método para editar un usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <param name="model">Objeto que contiene los datos actualizados del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("{id}/editar")]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            var ids = ModelState.Where(x => x.Key.Equals("Name") || x.Key.Equals("MaternalSurname") || x.Key.Equals("PaternalSurname"));
            foreach (var item in ids)
            {
                ModelState[item.Key].Errors.Clear();
                ModelState[item.Key].ValidationState = ModelValidationState.Valid;
            }
            if (!ModelState.IsValid)
                return BadRequest("Por favor, completar los campos obligatorios.");
            if (ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password.Length < 6)
                    return BadRequest("La contraseña debe tener 6 caracteres como mínimo.");
            }

            var otherUser = await _userService.GetByUserName(model.UserName);
            if (otherUser != null && otherUser.Id != model.Id)
                return BadRequest("El usuario especificado ya se encuentra registrado.");

            var deletedUser = await _userService.AnyByUserNameIgnoreQueryFilter(model.UserName, model.Id);
            if (deletedUser) return BadRequest("Ya existe un usuario eliminado con el mismo código.");

            //var anotherEmail = await _userService.AnyByEmail(model.Email, model.Id);
            //if (anotherEmail)
            //    return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            if (!await _roleManager.RoleExistsAsync(ConstantHelpers.ROLES.OFFICE))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = ConstantHelpers.ROLES.OFFICE
                });
            }

            if (!await _roleManager.RoleExistsAsync(ConstantHelpers.ROLES.DEPENDENCY))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = ConstantHelpers.ROLES.DEPENDENCY
                });
            }

            if (!await _roleManager.RoleExistsAsync(ConstantHelpers.ROLES.COST_CENTER))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = ConstantHelpers.ROLES.COST_CENTER
                });
            }


            var officeRol = await _roleService.GetByName(ConstantHelpers.ROLES.OFFICE);
            var dependencyRol = await _roleService.GetByName(ConstantHelpers.ROLES.DEPENDENCY);
            var costCenterRol = await _roleService.GetByName(ConstantHelpers.ROLES.COST_CENTER);


            if (!model.SelectedRoles.Any(x => x == officeRol.Id || x == dependencyRol.Id || x == costCenterRol.Id) && (model.SelectedDependencies != null && model.SelectedDependencies.Count > 0))
                return BadRequest("No se pueden agregar dependencias a este usuario");

            if (model.SelectedRoles.Any(x => x == officeRol.Id || x == dependencyRol.Id || x == costCenterRol.Id) && (model.SelectedDependencies == null || model.SelectedDependencies.Count == 0))
                return BadRequest("El campo de dependencia se encuentra incompleto");

            var user = await _userService.GetUserWithDependecies(id);

            user.UserName = model.UserName;
            user.NormalizedUserName = _userManager.NormalizeName(model.UserName);
            user.Email = model.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);
            user.Sex = model.Sex;
            user.Dni = model.Dni;
            user.Document = model.Dni;
            user.Type = ConstantHelpers.USER_TYPES.ADMINISTRATIVE;
            user.BirthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);

            if (!string.IsNullOrEmpty(model.Password))
            {
                foreach (IPasswordValidator<ApplicationUser> passwordValidator in _userManager.PasswordValidators)
                {
                    var result = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

                    if (!result.Succeeded)
                        return BadRequest(string.Join("; ", result.Errors.Select(x => x.Description).ToList()));
                }

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }

            var identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
                return BadRequest("Ocurrió un error al guardar.");

            return await Save(model, user);
        }

        /// <summary>
        /// Método para guardar los datos del usuario
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del usuario</param>
        /// <param name="user">Entidad usuario</param>
        /// <returns>Código de estado HTTP</returns>
        private async Task<IActionResult> Save(UserViewModel model, ApplicationUser user)
        {
            // Update user dependencies
            if (user.UserDependencies != null)
                await _userDependencyService.DeleteRange(user.UserDependencies);
            if (model.SelectedDependencies != null)
                await _userDependencyService.InsertRange(model.SelectedDependencies.Select(x => new UserDependency() { UserId = user.Id, DependencyId = x }));

            // Update user roles
            var userRoles = await _roleService.GetAllById(model.SelectedRoles);

            //if (userRoles.Any(y => y.Name == ConstantHelpers.ROLES.NUTRITION || y.Name == ConstantHelpers.ROLES.INFIRMARY || y.Name == ConstantHelpers.ROLES.PSYCHOLOGY))
            //{
            //    var welfareRoles = userRoles.Where(y => y.Name == ConstantHelpers.ROLES.NUTRITION || y.Name == ConstantHelpers.ROLES.INFIRMARY || y.Name == ConstantHelpers.ROLES.PSYCHOLOGY).ToList();
            //    if (welfareRoles.Count() > 1)
            //        return BadRequest("Solo se puede seleccionar un rol de categoria 'Doctor'.");

            //    var doctorRole = welfareRoles.FirstOrDefault();

            //    var doctorSpeciality = await _context.DoctorSpecialties.Where(x => x.Description == doctorRole.Name).FirstOrDefaultAsync();

            //    if (doctorSpeciality is null)
            //    {
            //        doctorSpeciality = new ENTITIES.Models.Intranet.DoctorSpecialty
            //        {
            //            Description = doctorRole.Name
            //        };

            //        await _context.DoctorSpecialties.AddAsync(doctorSpeciality);
            //    }

            //    var doctor = await _context.Doctors.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

            //    if (doctor is null)
            //    {
            //        doctor = new ENTITIES.Models.Generals.Doctor
            //        {
            //            UserId = user.Id,
            //            DoctorSpecialtyId = doctorSpeciality.Id
            //        };

            //        await _context.Doctors.AddAsync(doctor);
            //    }

            //    doctor.DoctorSpecialtyId = doctorSpeciality.Id;

            //}

            await _userService.RemoveFromRoles(user, await _userService.GetRoles(user));
            await _userManager.AddToRolesAsync(user, userRoles.Select(x => x.Name));

            // Upload Picture
            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (!string.IsNullOrEmpty(user.Picture))
                    await storage.TryDelete(user.Picture, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);

                user.Picture = await storage.UploadFile(model.Picture.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE,
                    Path.GetExtension(model.Picture.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _userService.Update(user);
            //SendConfirmationEmail(user);
            return Ok();
        }

        /// <summary>
        /// Método para cambiar el estado del usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <param name="isActive">Activo / Inactivo</param>
        /// <returns></returns>
        [HttpPost("{id}/editar/estado/post")]
        public async Task<IActionResult> UpdateStatus(string id, bool isActive)
        {
            var user = await _userService.Get(id);
            user.IsActive = isActive;
            await _userService.Update(user);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar un usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Get(id);
            var rolesToRemove = await _userService.GetRoles(user);
            await _userService.RemoveFromRoles(user, rolesToRemove);
            await _userDependencyService.DeleteByUser(user.Id);
            if (!string.IsNullOrEmpty(user.Picture))
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(user.Picture, ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);
            }
            await _userService.Delete(user);
            return Ok();
        }
        #endregion
    }
}
