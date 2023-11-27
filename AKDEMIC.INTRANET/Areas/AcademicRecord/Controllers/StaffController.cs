using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Staff;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_RECORD)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/personal")]
    public class StaffController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IAcademicRecordDepartmentService _academicRecordDepartmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffController(IDataTablesService dataTablesService, IUserService userService, IRoleService roleService,
            IAcademicRecordDepartmentService academicRecordDepartmentService,
            UserManager<ApplicationUser> userManager) : base(userService, roleService)
        {
            _dataTablesService = dataTablesService;
            _academicRecordDepartmentService = academicRecordDepartmentService;
            _userManager = userManager;
        }

        /// <summary>
        /// Vista donde se gestiona el personal de registros académicos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Users() => View();

        /// <summary>
        /// Vista donde se asignan los departamentos al personas de registros académicos
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("asignacion-departamentos")]
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de usuarios con el rol de personal de registro académico
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetUsersDatatable(parameters, search, ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de usuarios que pueden ser asignados como personal de registro académico
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de usurios</returns>
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers(string search)
        {
            var paramters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetAcademicRecordUsers(paramters, search);
            return Ok(result);
        }

        /// <summary>
        /// Método para asignar un departamento al personal de registros académicos
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del departamento académico</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asignar-departamentos")]
        public async Task<IActionResult> AssignDepartments(AssignDepartmentsViewModel model)
        {
            var user = await _userService.Get(model.UserId);

            if (user == null)
                return BadRequest("No se encontró al usuario seleccionado.");

            var departments = await _academicRecordDepartmentService.GetByUserId(model.UserId);

            if (departments.Count() > 0)
                await _academicRecordDepartmentService.DeleteRange(departments);

            if (model.AcademicDepartments != null)
            {
                var academicRecordCareers = new List<AcademicRecordDepartment>();

                for (int i = 0; i < model.AcademicDepartments.Length; i++)
                {
                    academicRecordCareers.Add(new AcademicRecordDepartment
                    {
                        AcademicDepartmentId = model.AcademicDepartments[i],
                        UserId = user.Id
                    });
                }

                await _academicRecordDepartmentService.InsertRange(academicRecordCareers);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de departamentos asignados al personal de registros académicos
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Listado de departamentos académicos</returns>
        [HttpGet("{userId}/get-departamentos-asignados")]
        public async Task<IActionResult> GetDepartmentsAssigned(string userId)
        {
            var departments = await _academicRecordDepartmentService.GetByUserId(userId);
            var result = departments.Select(x => x.AcademicDepartmentId).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Vista para crear un nuevo personal de registros académicos
        /// </summary>
        /// <returns>Vista nuevo personal</returns>
        [HttpGet("nuevo")]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Método para crear un nuevo usuario como personal de registros académicos
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del nuevo usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("nuevo")]
        public async Task<IActionResult> Add(NewStaff model)
        {
            if (!model.Password.Equals(model.PasswordVerifier))
                return BadRequest("Contraseñas Incorrectas");

            var username = await _userService.GetByUserName(model.UserName);

            if (username != null)
                return BadRequest($"El usuario '{model.UserName}' ya se encuentra registrado.");

            if (await _userService.AnyByEmail(model.Email))
                return BadRequest($"El correo {model.Email} ya esta siendo usado.");

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                MaternalSurname = model.MaternalSurname,
                PaternalSurname = model.PaternalSurname,
                FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")}, {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}",
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            //await _emailSender.SendEmailConfirmUserAsync(model.Email, callbackUrl);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
                return Ok();
            }
            else
            {
                return BadRequest("Error al crear al usuario.");
            }
        }

        /// <summary>
        /// Vista edición del usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Vista edición</returns>
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.Get(id);

            var model = new EditStaff
            {
                Email = user.Email,
                MaternalSurname = user.MaternalSurname,
                Name = user.Name,
                PaternalSurname = user.PaternalSurname,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
                UserName = user.UserName,
            };

            return View(model);
        }

        /// <summary>
        /// Método para editar un usuario 
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(EditStaff model)
        {
            var user = await _userService.Get(model.UserId);

            if (await _userService.AnyByEmail(model.Email, model.UserId))
                return BadRequest($"El correo {model.Email} ya se encuentro registrado.");

            if (await _userService.AnyByUserName(model.UserName, model.UserId))
                return BadRequest($"El usuario '{model.UserName}' ya se encuentra registrado.");

            //user.MaternalSurname = model.MaternalSurname;
            //user.PaternalSurname = model.PaternalSurname;
            //user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            await _userService.Update(user);
            return Ok();
        }

        /// <summary>
        /// Método para asignar / remover el rol de personal de registros académicos
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asignar-remover-rol")]
        public async Task<IActionResult> AssignRemoveRol(string userId)
        {
            var user = await _userService.Get(userId);
            if (user is null) return BadRequest("No se encontró al usuario seleccionado.");

            var roles = await _userService.GetRoles(user);

            if (roles.Any(y => y == ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                await _userService.RemoveFromRole(user, ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
                return Ok("Rol removido satisfactoriamente.");
            }
            else
            {
                await _userService.AddToRole(user, ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF);
                return Ok("Rol asignado satisfactoriamente.");
            }

        }
    }
}
