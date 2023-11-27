using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Extensions;
using AKDEMIC.DEGREE.ViewModels.AccountViewModels;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly AkdemicContext _dbContext;
        private readonly IActionContextAccessor _accessor;

        public AccountController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IActionContextAccessor accessor,
            AkdemicContext dbContext)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _dbContext = dbContext;
            _accessor = accessor;

        }

        [TempData]
        public string ErrorMessage { get; set; }


        [AllowAnonymous]
        [HttpGet("/consultar-dni-test/{dni}")]
        public async Task<IActionResult> RequestDniTest(string dni)
        {
            var query = new AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST.Query(_accessor);
            var queryResult = await query.GetDni(dni);
            var test = AKDEMIC.CORE.Environments.Environment.WebServices.PIDE.Methods.REST.Query.Url_Dni;
            return Ok(new
            {
                requestUri = test,
                queryResult
            });
        }

        [AllowAnonymous]
        [HttpGet("/consultar-ruc/{ruc}")]
        public async Task<IActionResult> RequestRucTest(string ruc)
        {
            var query = new AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST.Query(_accessor);
            var queryResult = await query.GetMainDataByRuc(ruc);
            var test = AKDEMIC.CORE.Environments.Environment.WebServices.PIDE.Methods.REST.Query.Url_Ruc;
            return Ok(new
            {
                requestUri = test,
                queryResult
            });
        }


        [AllowAnonymous]
        [HttpGet("/consultar-razon-social/{nombre}")]
        public async Task<IActionResult> RequestBusinessNameTest(string nombre)
        {
            var query = new AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST.Query(_accessor);
            var queryResult = await query.SearchBusinessName(nombre);
            var test = AKDEMIC.CORE.Environments.Environment.WebServices.PIDE.Methods.REST.Query.Url_Ruc;
            return Ok(new
            {
                requestUri = test,
                queryResult
            });
        }

        [AllowAnonymous]
        [HttpGet("/consultar-grados-test/{user}/{password}/{document}")]
        public async Task<IActionResult> RequestDegreeTest(string user, string password, string document)
        {
            var query = new AKDEMIC.WEBSERVICE.Services.PIDE.Methods.REST.Query(_accessor);
            var queryResult = await query.GetGrados(user, password, document);
            var test = AKDEMIC.CORE.Environments.Environment.WebServices.PIDE.Methods.REST.Query.Url_Grados;
            return Ok(new
            {
                requestUri = test,
                queryResult
            });
        }

        /// <summary>
        /// Obtiene la vista de login
        /// </summary>
        /// <param name="returnUrl">Texto de redireccion del sistema</param>
        /// <param name="provider">Texto de proveedor para el SSO</param>
        /// <returns>Retorna una vista o redirecciona</returns>
        [HttpGet("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string provider = "OpenIdConnect")
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.Akdemic)
            //{
            //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //    return Challenge(properties, provider);
            //}

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
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

                    if (!await _dbContext.UserLogins.AnyAsync(x => x.UserId == currentUser.Id && x.System == ConstantHelpers.SYSTEMS.DEGREE))
                    {
                        var newLogin = new UserLogin
                        {
                            FirstLogin = DateTime.UtcNow,
                            Ip = GetRequestIP(),
                            LastLogin = DateTime.UtcNow,
                            System = ConstantHelpers.SYSTEMS.DEGREE,
                            UserId = currentUser.Id,
                            UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : ""
                        };

                        await _dbContext.UserLogins.AddAsync(newLogin);
                    }
                    else
                    {
                        var currentLogin = await _dbContext.UserLogins.Where(x => x.UserId == currentUser.Id && x.System == ConstantHelpers.SYSTEMS.DEGREE).FirstOrDefaultAsync();
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (currentUser == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
            if (result.Succeeded && !currentUser.IsLockedOut)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", currentUser.Id);
                return RedirectToLocal(returnUrl);
            }
            else
              if (result.Succeeded && currentUser.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout), new { reason = currentUser.LockedOutReason });
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", currentUser.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout(string reason = "")
        {
            return View("Lockout", reason);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    _emailSender.SendEmailConfirmation(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    await HttpContext.SignOutAsync();

        //    _logger.LogInformation("User logged out.");
        //    //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.Akdemic)
        //    //    return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}

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

        /// <summary>
        /// Método para realizar el cierre de sesión del SSO
        /// </summary>
        /// <returns>Código de estado HTTP</returns>
        [AllowAnonymous]
        [HttpGet("/fc-logout")]
        public async Task<IActionResult> FrontChannelLogout()
        {
            if (_signInManager.IsSignedIn(User) || User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync();
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByIdAsync(info.ProviderKey);

            var add_result = await _userManager.AddLoginAsync(user, info);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            var currentUser = await _userManager.FindByIdAsync(info.ProviderKey);

            if (result.Succeeded && !currentUser.IsLockedOut)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            else if (result.Succeeded && currentUser.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout), new { reason = currentUser.LockedOutReason });
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = await _userService.GetAllByEmail(model.Email);

                if (users.Count > 1)
                {
                    if (string.IsNullOrEmpty(model.UserName))
                    {
                        ViewBag.NeedUserName = true;
                        ModelState.AddModelError(string.Empty, "Debe ingresar un usuario");
                        //Agregar al state, que debe ingresar un usuario, mostrar el usuario input
                        // If we got this far, something failed, redisplay form
                        return View(model);
                    }

                    //Traemos el usuario por username
                    var user = users.Where(x => x.UserName == model.UserName).FirstOrDefault();

                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action(action: nameof(AccountController.ResetPassword), controller: "Account", values: new { userid = user.Id, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Reiniciar contraseña",
                    //     $"Por favor reinicie su contraseña en el siguiente link: <a href='{callbackUrl}'>link</a>");

                    _emailSender.SendEmailPasswordRecovery(Helpers.ConstantHelpers.Project.NAME, model.Email, GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Degree), callbackUrl);
                }
                else if (users.Count == 1)
                {
                    var user = users.FirstOrDefault();

                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action(action: nameof(AccountController.ResetPassword), controller: "Account", values: new { userid = user.Id, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Reiniciar contraseña",
                    //     $"Por favor reinicie su contraseña en el siguiente link: <a href='{callbackUrl}'>link</a>");

                    _emailSender.SendEmailPasswordRecovery(Helpers.ConstantHelpers.Project.NAME, model.Email, GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Degree), callbackUrl);
                }
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string userid = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }

            var message = "";
            if (TempData["message"] != null)
            {
                message = TempData["message"].ToString();
            }

            var model = new ResetPasswordViewModel { Code = code, Message = message, UserId = userid };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userService.GetByUserName(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["message"] = "Revise la informción ingresada";
                return RedirectToAction("ResetPassword", new { code = model.Code });
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            else
            {
                TempData["message"] = "Token inválido";
                return RedirectToAction("ResetPassword", new { code = model.Code });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
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

        #endregion
    }
}
