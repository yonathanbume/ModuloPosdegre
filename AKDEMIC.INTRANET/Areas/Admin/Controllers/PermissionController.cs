using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Permission;
using AKDEMIC.INTRANET.Areas.Admin.Models.PermissionViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/permisos")]
    public class PermissionController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly ITeacherService _teacherService;
        private readonly IUserService _userService;
        public PermissionController(
            ISelect2Service select2Service,
            IUserService userService,
            ITeacherService teacherService,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> rolemanager
            ) : base(context, userManager, rolemanager)
        {
            _select2Service = select2Service;
            _teacherService = teacherService;
            _userService = userService;
        }

        #region --- JSON ---

        /// <summary>
        /// Obtiene el listado de roles
        /// </summary>
        /// <returns>Listado de roles</returns>
        [HttpGet("roles/get")]
        public IActionResult ListRoles()
        {
            var result = _context.Roles
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de permisos según rol
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        /// <returns>Listado de permisos</returns>
        [HttpGet("roles/{id}/get")]
        public IActionResult ListPermissionsByRole(string id)
        {
            var permissions = _context.RolePermission
                .Where(x => x.RoleId == id)
                .Select(x => new
                {
                    id = x.Id,
                    name = EnumHelpers.GetDescription((ConstantHelpers.PermissionHelpers.Permission)Enum.Parse(typeof(ConstantHelpers.PermissionHelpers.Permission), x.PermissionLabel)),
                    value = x.PermissionLabel
                })
                .ToList();

            var role = _context.Roles.FirstOrDefault(x => x.Id == id);

            return Ok(new { permissions, role });
        }

        /// <summary>
        /// Método para registrar los permisos del rol
        /// </summary>
        /// <param name="model">Objeto que contiene los permisos del rol</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar/post")]
        public async Task<IActionResult> SetPermission(RolePermissionViewModel model)
        {
            var role = new ApplicationRole();

            if (String.IsNullOrEmpty(model.Id))
            {
                role = new ApplicationRole { Name = model.Name };

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded) return BadRequest();
            }
            else
            {
                role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (!role.IsStatic)
                {
                    role.Name = model.Name;
                    role.NormalizedName = model.Name.Normalize();
                }

                var oldPermissions = _context.RolePermission.Where(x => x.RoleId == role.Id);

                _context.RolePermission.RemoveRange(oldPermissions);

                await _context.SaveChangesAsync();
            }

            if (model.ListPermissions == null) return Ok();

            foreach (var permission in model.ListPermissions)
            {
                var per_enum = (ConstantHelpers.PermissionHelpers.Permission)Enum.Parse(typeof(ConstantHelpers.PermissionHelpers.Permission), permission.Value, true);

                var lPermission = Enum.GetName(typeof(ConstantHelpers.PermissionHelpers.Permission), per_enum);

                var obj = new RolePermission
                {
                    PermissionLabel = lPermission,
                    Permission = (int)per_enum,
                    RoleId = role.Id
                };

                await _context.RolePermission.AddAsync(obj);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Método para eliminar los permisos del rol
        /// </summary>
        /// <param name="id">Identificador del rol</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("roles/eliminar/post")]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.UserRoles.Any(x => x.RoleId == id) || _context.Roles.Any(x => x.Id == id && x.IsStatic)) return BadRequest();

            var role = _context.Roles.FirstOrDefault(x => x.Id == id);

            var permissions = _context.RolePermission.Where(x => x.RoleId == id).ToList();

            _context.RolePermission.RemoveRange(permissions);

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de usuarios
        /// </summary>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("usuarios/admin/get")]
        public async Task<IActionResult> ListAdmins()
        {
            //var result = await _context.UserRoles.Where(ur => ConstantHelpers.Solution.Roles.Any(x => x.Value == ur.Role.Name))
            //    .Select(x => new
            //    {
            //        id = x.User.Id,
            //        name = x.User.FullName,
            //        username = x.User.UserName
            //    })
            //    //.Distinct()
            //    .ToListAsync();

            //result = result.Distinct().ToList();


            var roles = ConstantHelpers.Solution.Roles.Select(x => x.Value).ToHashSet();

            var result = await _context.Users
                .Where(x => x.UserRoles.Any(r => roles.Contains(r.Role.Name)))
                    .Select(x => new
                    {
                        id = x.Id,
                        name = x.FullName,
                        username = x.UserName
                    })
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el sistema habilitado para el usuario
        /// </summary>
        /// <param name="Id">Identificador del usuario</param>
        /// <returns>Objeto que contiene los datos del usuario</returns>
        [HttpGet("usuarios/admin/detail/get/{id}")]
        public async Task<IActionResult> AdminDetail(string Id)
        {
            var result = await _context.UserRoles.Where(ur => ur.UserId == Id)
             .Select(x => new
             {
                 x.User.AllowedSystem
             })
             .FirstOrDefaultAsync();

            if (result != null)
            {
                var systems = string.IsNullOrEmpty(result.AllowedSystem) ? new List<string>() : result.AllowedSystem.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                return Ok(systems);
            }

            return BadRequest();
        }

        /// <summary>
        /// Método para asignar roles y permisos 
        /// </summary>
        /// <param name="model">Objeto que contiene los roles y permisos</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("usuarios/admin/add")]
        public async Task<IActionResult> NewAdmin(AdminViewModels model)
        {
            //foreach (var item in ConstantHelpers.Solution.Roles)
            //    await _roleManager.CreateAsync(new ApplicationRole { Name = item.Value });

            if (!ModelState.IsValid)
                return BadRequest("Completas los campos indicados.");

            //buscar el usuario
            var user = await _userService.Get(model.TeacherId);
            var systems = string.Join(",", model.ListChecked);
            user.AllowedSystem = systems;

            var roles = await _userManager.GetRolesAsync(user);
            var adminRoles = roles.Where(x => ConstantHelpers.Solution.Roles.Any(y => y.Value == x)).ToArray();
            await _userManager.RemoveFromRolesAsync(user, adminRoles);

            foreach (var item in model.ListChecked)
            {
                var role = ConstantHelpers.Solution.Roles[item];

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new ApplicationRole { Name = role });

                    if (!result.Succeeded) return BadRequest("Ocurrió un problema creando el rol");
                }
                await _userManager.AddToRoleAsync(user, role);
            }

            await _userService.Update(user);

            return Ok("El administrador ha sido asignado correctamente.");
        }

        /// <summary>
        /// Método para eliminar roles y permisos
        /// </summary>
        /// <param name="Id">Identificador del usuario</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("usuarios/admin/delete")]
        public async Task<IActionResult> DeleteAdmin(string Id)
        {
            //buscar el usuario
            ApplicationUser user = await _userService.Get(Id);

            user.AllowedSystem = null;

            var roles = await _userManager.GetRolesAsync(user);
            var adminRoles = roles.Where(x => ConstantHelpers.Solution.Roles.Any(y => y.Value == x)).ToArray();
            await _userManager.RemoveFromRolesAsync(user, adminRoles);

            await _userService.Update(user);

            return Ok("Se removieron los permisos de administrador al usuario.");
        }

        /// <summary>
        /// Obtiene el listado de docentes
        /// </summary>
        /// <param name="q">Texto de búsqueda</param>
        /// <returns>Listado de docentes</returns>
        [HttpGet("usuarios/get")]
        public async Task<IActionResult> GetTeachers(string q)
        {
            var query = _context.Users
                .Where(x => x.UserRoles.All(r => r.Role.Name != ConstantHelpers.ROLES.STUDENTS
                && r.Role.Name != ConstantHelpers.ROLES.COMPUTER_STUDENTS
                && r.Role.Name != ConstantHelpers.ROLES.LANGUAGE_STUDENTS
                && r.Role.Name != ConstantHelpers.ROLES.PRE_UNIVERSITARY_STUDENTS
                && r.Role.Name != ConstantHelpers.ROLES.TEACHERS))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(x => x.FullName.SplitContains(q.Split(' ', StringSplitOptions.RemoveEmptyEntries)));

            var teachers = await query
                .Select(
                    x => new
                    {
                        id = x.Id,
                        text = x.FullName
                    }
                )
                .ToListAsync();

            teachers = teachers.OrderBy(x => x.text).ToList();

            return Ok(teachers);
        }

        #endregion


        #region --- ACTIONS ---

        //[RequiresPermission(AddUser, ManageRoles)]
        /// <summary>
        /// Vista donde se gestionan los permisos del rol
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            var model = new RolePermissionViewModel();

            return View(model);
        }

        /// <summary>
        /// Vista donde se gestionan los administradores
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("usuarios/admin")]
        public IActionResult Admins()
        {
            var model = new AdminViewModels();

            return View(model);
        }

        #endregion
    }
}
