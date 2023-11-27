
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Generals;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.ViewModels.ProfileViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System.Security.Claims;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("perfil")]
    public class ProfileController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(SignInManager<ApplicationUser> signInManager, IUserService userService,
            IStudentService studentService,
            UserManager<ApplicationUser> userManager) : base(userManager, userService)
        {
            _studentService = studentService;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Obtiene la vista de perfil del usuario logeado
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var student = await _studentService.GetStudentByUser(user.Id);

            var profileViewModel = new ProfileViewModel(user, student);

            return View(profileViewModel);
        }

        /// <summary>
        /// Edita el perfil del usuario logeado
        /// </summary>
        /// <param name="applicationUser">Modelo que contiene el usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> PersonalInformation(ApplicationUser applicationUser)
        {
            var user = await GetCurrentUserAsync();
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.PersonalEmail = applicationUser.Email;
            //user.NormalizedEmail = applicationUser.Email.ToUpper().Normalize();
            //user.NormalizedEmail = _userManager.NormalizeEmail(applicationUser.Email);

            user.DepartmentId = applicationUser.DepartmentId;
            user.ProvinceId = applicationUser.ProvinceId;
            user.DistrictId = applicationUser.DistrictId;
            user.Address = applicationUser.Address;

            await _userService.Update(user);
            return Ok();
        }

        /// <summary>
        /// Cambia la contraseña del usuario
        /// </summary>
        /// <param name="passwordViewModel">Modelo que contiene la información de la contraseña a modificar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("password/post")]
        public async Task<IActionResult> ChangePassword(PasswordViewModel passwordViewModel)
        {
            var user = await GetCurrentUserAsync();
            var passwordValidator = new PasswordValidator<ApplicationUser>();

            //  _userManager.PasswordHasher = IPasswordHasher<ApplicationUser>();
            //  IPasswordHasher<ApplicationUser> passHash;

            var currentPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, passwordViewModel.CurrentPassword);

            // var passIsVal = passHash.VerifyHashedPassword(user, user.PasswordHash, passwordViewModel.CurrentPassword);
            var newPasswordIsValid = passwordValidator.ValidateAsync(_userManager, user, passwordViewModel.NewPassword).Result.Succeeded;

            if (!currentPasswordIsCorrect || !newPasswordIsValid)
                return BadRequest("Revise la información ingresada");

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordViewModel.NewPassword);

            await _userService.Update(user);


            await _signInManager.SignOutAsync();
            //return RedirectToAction(nameof(HomeController.Index), "Home");
            return Ok();
        }
    }
}
