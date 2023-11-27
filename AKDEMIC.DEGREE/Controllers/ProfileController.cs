
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.DEGREE.Controllers
{
    [Authorize]
    [Route("perfil")]
    public class ProfileController : BaseController
    {
        protected readonly IUserService _userService;
        public ProfileController(UserManager<ApplicationUser> userManager) : base(userManager) { }
         
        
        //public async Task<IActionResult> Index()
        //{
        //    ProfileViewModel profileViewModel = new ProfileViewModel(); 
        //    var userId = _userManager.GetUserId(User);
        //    ApplicationUser user = await _userService.Find(userId);
        //    var student = _context.Students
        //                    .Include(x=>x.Career)
        //                    .Where(x => x.UserId == Convert.ToString(user.Id)).FirstOrDefault();
        //    profileViewModel.User = user;
        //    profileViewModel.Student = student;

        //    return View(profileViewModel);
        //}

        //[Route("editar/post")] 
        //[HttpPost]
        //public async Task<IActionResult> PersonalInformation(ApplicationUser applicationUser)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    ApplicationUser user = await _userService.Find(userId);
        //    user.PhoneNumber = applicationUser.PhoneNumber;
        //    user.Email = applicationUser.Email;
        //    user.NormalizedEmail = applicationUser.Email.Normalize();

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[Route("password/post")]
        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(PasswordViewModel passwordViewModel)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    ApplicationUser user = await _context.Users.FindAsync(userId);
        //    var passwordValidator = new PasswordValidator<ApplicationUser>();
            
            
        //  //  _userManager.PasswordHasher = IPasswordHasher<ApplicationUser>();

        //  //  IPasswordHasher<ApplicationUser> passHash;

        //    var currentPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, passwordViewModel.CurrentPassword);
             
        //   // var passIsVal = passHash.VerifyHashedPassword(user, user.PasswordHash, passwordViewModel.CurrentPassword);
        //    var newPasswordIsValid = passwordValidator.ValidateAsync(_userManager, user, passwordViewModel.NewPassword).Result.Succeeded;
            
        //    if (!currentPasswordIsCorrect || !newPasswordIsValid)
        //    {
        //        return BadRequest();
        //    }

        //    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordViewModel.NewPassword);

        //    await SaveChangesAsync();
        //    return Ok();
        //}

    }
}