using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AKDEMIC.INTRANET.Controllers
{
    [Route("investigation")]
    public class APIInvestigationController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public static string theme = GeneralHelpers.GetInstitutionAbbreviation().ToLower();
        public static string logo_company = "/images/themes/" + theme.ToLower() + "/logo.png";
        public static string buttonColor = ConstantHelpers.THEME_PARAMETERS.COLORS[GeneralHelpers.GetInstitutionAbbreviation()];

        public APIInvestigationController(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger) : base(context, userManager)
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene los roles al logear el usuario en Quibuk
        /// </summary>
        /// <param name="username">Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="passwordakdemic">Password entre Quibuk y akdemic</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("api/login/{username}/{password}/{passwordakdemic}")]
        public async Task<IActionResult> Index(string username, string password, string passwordakdemic)
        {

            if (passwordakdemic != ConstantHelpers.PASSWORDINVESTIGATION)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
                var roles = await _userManager.GetRolesAsync(user);

                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());


                return Ok(roles);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
