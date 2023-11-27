using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.POSDEGREE.ViewModels.AccountViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace AKDEMIC.POSDEGREE.Controllers
{
    [Authorize]
    [Route("[controller]/[Action]")]
    public class AccountController : Controller
    {
        /* [HttpGet("/")]
         public async Task<IActionResult> Login()
         {
             if (false) return RedirectToAction(nameof(HomeController.Index), "Home");

             //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

             //ViewData["ReturnUrl"] = returnUrl;
             //return View();
             return Ok("no molestar");
         }*/
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
     
        private readonly ILogger _logger;
        private readonly AkdemicContext _dbContext;
        private readonly IActionContextAccessor _accessor;

        public AccountController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IActionContextAccessor accessor,
            AkdemicContext dbContext)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
            _accessor = accessor;

        }
        [HttpGet("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string provider = "OpenIdConnect")
       {
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");
              //se establese los permisos 
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
           
        }
        [HttpGet("/prueba")]
        [AllowAnonymous]
        public async Task<IActionResult> _TopBarPartial()
        {
         
            return View("Views/Shared/Templates/Default/Partials/TopBarPartial.cshtml");

        }
        /// <summary>
        /// Realiza el login del usuario
        /// </summary>
        /// <param name="model">Modelo que contiene la informacion del login</param>
        /// <param name="returnUrl">Texto de redireccion</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                var currentUser = await _dbContext.Users.Where(x => x.UserName == model.UserName).FirstOrDefaultAsync();

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuario logeado");

                    if (!await _dbContext.UserLogins.AnyAsync(x => x.UserId == currentUser.Id && x.System == ConstantHelpers.SYSTEMS.POSDEGREE))
                    {
                        var newLogin = new UserLogin
                        {
                            FirstLogin = DateTime.UtcNow,
                            Ip = GetRequestIP(),
                            LastLogin = DateTime.UtcNow,
                            System = ConstantHelpers.SYSTEMS.POSDEGREE,
                            UserId = currentUser.Id,
                            UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : ""
                        };

                        await _dbContext.UserLogins.AddAsync(newLogin);
                    }
                    else
                    {
                        var currentLogin = await _dbContext.UserLogins.Where(x => x.UserId == currentUser.Id && x.System == ConstantHelpers.SYSTEMS.POSDEGREE).FirstOrDefaultAsync();
                        currentLogin.LastLogin = DateTime.UtcNow;
                        currentLogin.Ip = GetRequestIP();
                        currentLogin.UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "";
                    }
                    await _dbContext.SaveChangesAsync();

                    return Ok(returnUrl ?? "/");
                }


                if (result.IsLockedOut)
                {
                    return BadRequest($"ERROR: Intente en {CORE.Helpers.ConstantHelpers.Lockout.Time} minutos");
                }
                else
                {
                    return BadRequest("Usuario y/o Contraseña Incorrectos");
                }

            }

            return BadRequest("Usuario y/o Contraseña Incorrectos");
            // If we got this far, something failed, redisplay form
        }

        /// <summary>
        /// Método usado para cerrar la sesión del usuario
        /// </summary>
        /// <returns>Redirecciona al inicio</returns>
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
                return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>("X-Forwarded-For");
                ip = SplitCsv(csvList).FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _accessor.ActionContext.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }
        private T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_accessor.ActionContext.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
