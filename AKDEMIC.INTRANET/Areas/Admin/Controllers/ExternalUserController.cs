using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExternalUserViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Extensions;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/usuarios-externos")]
    public class ExternalUserController : BaseController
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IExternalUserService _externalUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExternalUserController(
            ICloudStorageService cloudStorageService,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            IDataTablesService dataTablesService,
            IExternalUserService externalUserService,
            UserManager<ApplicationUser> userManager
            )
        {
            _cloudStorageService = cloudStorageService;
            _roleManager = roleManager;
            _userService = userService;
            _dataTablesService = dataTablesService;
            _externalUserService = externalUserService;
            _userManager = userManager;
        }

        public IActionResult IndeX()
            => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetUsersDatatableByType(parameters, ConstantHelpers.USER_TYPES.EXTERNAL, search);
            return Ok(result);
        }

        [HttpGet("agregar")]
        public IActionResult Add()
            => View();

        [HttpPost("agregar")]
        public async Task<IActionResult> Add(UserViewModel model)
        {
            var userDni = await _userManager.FindByNameAsync(model.Document);
            var userEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userEmail != null)
                return BadRequest($"El correo {model.Email} ya se encuentra registrado.");

            if (userDni != null)
                return BadRequest($"El documento {userDni} ya se encuentra registrado.");

            var birthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);

            var user = new ApplicationUser
            {
                UserName = model.Document,
                DocumentType = ConstantHelpers.DOCUMENT_TYPES.DNI,
                Dni = model.Document,
                Sex = model.Sex,
                Address = model.Address,
                BirthDate = birthDate,
                Email = model.Email,
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                Document = model.Document,
                PhoneNumber = model.PhoneNumber,
                Type = ConstantHelpers.USER_TYPES.EXTERNAL,
                EmailConfirmed = true,
                FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")} {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(string.Join(", ", result.Errors.Select(x => x.Description).ToList()));

            var externalUser = new ExternalUser
            {
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                FullName = $"{(string.IsNullOrEmpty(model.PaternalSurname) ? "" : $"{model.PaternalSurname} ")}{(string.IsNullOrEmpty(model.MaternalSurname) ? "" : $"{model.MaternalSurname}")} {(string.IsNullOrEmpty(model.Name) ? "" : $"{model.Name}")}",
                BirthDate = birthDate,
                DocumentType = model.DocumentType,
                DocumentNumber = model.Document,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserId = user.Id
            };

            if (!await _roleManager.RoleExistsAsync(ConstantHelpers.ROLES.EXTERNAL_USER))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = ConstantHelpers.ROLES.EXTERNAL_USER
                });
            }

            await _externalUserService.Insert(externalUser);
            await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.EXTERNAL_USER);
            return Ok();
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.Get(id);
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DocumentType = user.DocumentType,
                UserName = user.UserName,
                PaternalSurname = user.PaternalSurname,
                MaternalSurname = user.MaternalSurname,
                Name = user.Name,
                Address = user.Address,
                Sex = user.Sex,
                BirthDate = user.BirthDate.ToLocalDateFormat(),
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await _userService.Get(model.Id);

            var birthDate = ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate);

            user.Email = model.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            user.PhoneNumber = model.PhoneNumber;
            user.PaternalSurname = model.PaternalSurname;
            user.MaternalSurname = model.MaternalSurname;
            user.Name = model.Name;
            user.Address = model.Address;
            user.Sex = user.Sex;
            user.BirthDate = birthDate;
            user.Type = ConstantHelpers.USER_TYPES.EXTERNAL;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }

            await _userService.Update(user);
            return Ok();
        }
    }
}
