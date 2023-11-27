using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Handlers;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Extensions;
using AKDEMIC.INTRANET.ViewModels.AccountViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Bogus.DataSets;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfigurationService _configurationService;
        private readonly ISurveyUserService _surveyUserService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IStudentObservationService _studentObservationService;
        private readonly AkdemicContext _context;
        private readonly IBeginningAnnouncementService _beginningAnnouncementService;
        private readonly IActionContextAccessor _accessor;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            AkdemicContext akdemicContext,
            IConfigurationService configurationService,
            ISurveyUserService surveyUserService,
            IEmailSender emailSender,
            IUserService userService,
            IStudentObservationService studentObservationService,
              IBeginningAnnouncementService beginningAnnouncementService,
              IActionContextAccessor accessor,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _configurationService = configurationService;
            _surveyUserService = surveyUserService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = akdemicContext;
            _emailSender = emailSender;
            _logger = logger;
            _userService = userService;
            _studentObservationService = studentObservationService;
            _beginningAnnouncementService = beginningAnnouncementService;
            _accessor = accessor;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Obtiene la vista inicial de login
        /// </summary>
        /// <param name="returnUrl">Url de redirección</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewBag.IsLockedOut = false;
            ViewBag.LockedOutReason = "";

            var beginning = await _beginningAnnouncementService.GetBeginningAnnouncementsForLogin(ConstantHelpers.ANNOUNCEMENT.SYSTEM.INTRANET);
            var model = new LoginViewModel()
            {
                BeginningAnnouncements = beginning
            };

            var error = TempData["Error"] != null ? Convert.ToBoolean(TempData["Error"]) : false;
            if (error)
            {
                var errorMessage = TempData["ErrorMessage"] == null ? "Intento inválido" : TempData["ErrorMessage"].ToString();
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        /// <summary>
        /// Obtiene la vista de login bienestar
        /// </summary>
        /// <param name="returnUrl">Url de redirección</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("/bienestar")]
        [AllowAnonymous]
        public async Task<IActionResult> WelfareLogin(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            ViewData["LoginTitle"] = "Bienestar Universitario";
            return View("Login");
        }

        /// <summary>
        /// Obtiene una vista de login del campus
        /// </summary>
        /// <param name="returnUrl">Url de redirección</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("/campus")]
        [AllowAnonymous]
        public IActionResult CampusLogin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var systems = new List<string>();

            switch (ConstantHelpers.GENERAL.Institution.Value)
            {
                case ConstantHelpers.Institution.UNAMAD:
                    systems.Add("sic");
                    systems.Add("gradosytitulos");
                    systems.Add("matricula");
                    systems.Add("gef");
                    systems.Add("aulavirtual2");
                    systems.Add("gestiondocente");
                    systems.Add("escalafon");
                    systems.Add("bolsadetrabajo");
                    systems.Add("tutoria2");
                    systems.Add("biblioteca2");
                    systems.Add("gruposdeinteres");
                    systems.Add("transparencia");
                    systems.Add("rsu");
                    systems.Add("investigacion");
                    systems.Add("sgbi");
                    break;
                case ConstantHelpers.Institution.UNFV:
                case ConstantHelpers.Institution.Akdemic:
                    systems.Add("sisco");
                    systems.Add("grados");
                    systems.Add("matricula");
                    systems.Add("finanzas");
                    systems.Add("laurassia");
                    systems.Add("docente");
                    systems.Add("escalafon");
                    systems.Add("bolsa2");
                    systems.Add("tutoria");
                    systems.Add("biblioteca");
                    systems.Add("ginteres");
                    systems.Add("transparencia");
                    systems.Add("evaluacion");
                    systems.Add("investigacion");
                    systems.Add("indicadores");
                    break;
                case ConstantHelpers.Institution.UNJBG:
                    systems.Add("docente");
                    systems.Add("matricula");
                    systems.Add("laurassia");
                    systems.Add("finanzas");
                    systems.Add("indicadores");
                    systems.Add("bolsa2");
                    systems.Add("portalweb");
                    systems.Add("agenda");
                    systems.Add("directorio");
                    systems.Add("tramites");
                    systems.Add("actosresolutivos");
                    systems.Add("preuniversitario");
                    systems.Add("biblioteca");
                    systems.Add("bienestar");
                    systems.Add("admision");
                    systems.Add("comedor");
                    systems.Add("investigacion");
                    systems.Add("cooperacion");
                    break;
                case ConstantHelpers.Institution.PMESUT:
                    systems.Add("admision");
                    systems.Add("bienestar");
                    systems.Add("bolsa");
                    systems.Add("docente");
                    systems.Add("escalafon");
                    systems.Add("finanzas");
                    systems.Add("grados");
                    systems.Add("matricula");
                    systems.Add("sisco");
                    systems.Add("tramites");
                    systems.Add("tutorias");
                    break;
            }
            ViewBag.Systems = systems;

            return View("Campus");
        }

        /// <summary>
        /// Realiza el login del usuario
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros del login</param>
        /// <param name="returnUrl">Url a redirecionar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _signInManager.SignOutAsync();
                }

                var user = await _userService.GetByUserWeb(model.UserName);
                if (user != null) model.UserName = user.UserName;
                if (user == null) user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var mysqlPassword = ConvertHelpers.StringToMySQLPasswordHash(model.Password);
                    var mysqlOldPassword = ConvertHelpers.StringToMySQLOldPasswordHash(model.Password);
                    if (!string.IsNullOrEmpty(user.MysqlPasswordHash) && (user.MysqlPasswordHash == mysqlOldPassword || user.MysqlPasswordHash == mysqlPassword))
                    {
                        var passwordHashed = _userManager.PasswordHasher.HashPassword(user, model.Password);
                        user.PasswordHash = passwordHashed;
                        user.MysqlPasswordHash = null;
                        await _context.SaveChangesAsync();
                    }
                }

                if (user == null)
                {
                    return BadRequest(new
                    {
                        type = 0,
                        message = "Usuario y/o Contraseña Incorrectos"
                    });
                }

                if (user.IsLockedOut)
                {
                    return BadRequest(new
                    {
                        type = 1,
                        message = "Usuario Bloqueado",
                        reason = user.LockedOutReason
                    });
                }

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!await _context.UserLogins.AnyAsync(x => x.UserId == user.Id && x.System == ConstantHelpers.SYSTEMS.INTRANET))
                    {
                        var newLogin = new UserLogin
                        {
                            FirstLogin = DateTime.UtcNow,
                            Ip = GetRequestIP(),
                            LastLogin = DateTime.UtcNow,
                            System = ConstantHelpers.SYSTEMS.INTRANET,
                            UserId = user.Id,
                            UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : ""
                        };

                        await _context.UserLogins.AddAsync(newLogin);
                    }
                    else
                    {
                        var currentLogin = await _context.UserLogins.Where(x => x.UserId == user.Id && x.System == ConstantHelpers.SYSTEMS.INTRANET).FirstOrDefaultAsync();
                        currentLogin.LastLogin = DateTime.UtcNow;
                        currentLogin.Ip = GetRequestIP();
                        currentLogin.UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "";
                    }
                    await _context.SaveChangesAsync();
                    if (await _userManager.IsInRoleAsync(user, ConstantHelpers.ROLES.STUDENTS))
                    {
                        var student = await _context.Students.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
                        if (student != null)
                        {
                            var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED);

                            if (configuration is null)
                            {
                                configuration = new ENTITIES.Models.Configuration
                                {
                                    Key = ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED,
                                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.PERFORMANCE_EVALUATION_REQUIRED]
                                };
                            }
                            var required = Convert.ToBoolean(configuration.Value);

                            if (required)
                            {
                                var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE && !x.IsSummer).Select(x => x.Id).FirstOrDefaultAsync();

                                if (termId != null)
                                {
                                    var evaluations = await _context.PerformanceEvaluations.Where(x => x.TermId == termId).ToListAsync();

                                    if (evaluations.Any(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date))
                                    {
                                        var evaluation = evaluations.Where(x => x.StartDate.Date <= DateTime.UtcNow.ToDefaultTimeZone().Date && x.EndDate.Date >= DateTime.UtcNow.ToDefaultTimeZone().Date).FirstOrDefault();
                                        var teacherRole = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.TEACHERS).Select(x => x.Id).FirstOrDefaultAsync();
                                        var userEvaluation = await _context.PerformanceEvaluationUsers.Where(x => x.FromUserId == student.UserId && x.PerformanceEvaluationId == evaluation.Id && x.FromRole.Name == ConstantHelpers.ROLES.STUDENTS).CountAsync();
                                        var sections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == termId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(x => x.SectionId).ToListAsync();
                                        var teacherSection = await _context.TeacherSections.Where(x => sections.Contains(x.SectionId)).CountAsync();
                                        if (teacherSection > userEvaluation)
                                        {
                                            return Ok("/alumno/evaluacion-docente");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _logger.LogInformation("Usuario logeado");
                    return Ok(returnUrl ?? "/");

                }
                else
                {
                    return BadRequest(new
                    {
                        type = 0,
                        message = "Usuario y/o Contraseña Incorrectos"
                    });
                }
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(new
            {
                type = 0,
                message = "Usuario y/o Contraseña Incorrectos"
            });
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

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Register(string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation("User created a new account with password.");

        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
        //            await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            _logger.LogInformation("User created a new account with password.");
        //            return RedirectToLocal(returnUrl);
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        /// <summary>
        /// Deslogea del sistema
        /// </summary>
        /// <returns>Redirecciona al login despues de deslogear</returns>
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
                return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc");
            else
            {
                if (_signInManager.IsSignedIn(User) && User.IsInRole(ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE))
                {
                    await _signInManager.SignOutAsync();
                    if (ConstantHelpers.GENERAL.Authentication.SSO_ENABLED)
                        SignOut(CookieAuthenticationDefaults.AuthenticationScheme, /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc");

                    return RedirectToAction(nameof(AccountController.WelfareLogin), "Account");
                }

                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Deslogeo para el SSO
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
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

        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            string redirectUrl = Url.Action(nameof(ExternalResponse), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
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

            var currentUser = await _userManager.FindByIdAsync(info.ProviderKey);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

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

        /// <summary>
        /// Obtiene la vista inicial de olvido contraseña
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Envia un correo para restablecer la contraseña
        /// </summary>
        /// <param name="model">Modelo que contiene la solicitud de olvido contraseña</param>
        /// <returns>Retorna una vista o redirecciona</returns>
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

                    if (user == null)
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

                    _emailSender.SendEmailPasswordRecovery(Helpers.ConstantHelpers.Project.NAME, model.Email, GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet), callbackUrl);
                }
                else if (users.Count == 1)
                {
                    var user = users.FirstOrDefault();

                    if (user == null)
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

                    _emailSender.SendEmailPasswordRecovery(Helpers.ConstantHelpers.Project.NAME, model.Email, GeneralHelpers.GetApplicationRoute(ConstantHelpers.Solution.Intranet), callbackUrl);
                }
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Obtiene la vista de confirmación de olvido contraseña
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la vista para cambiar la contraseña
        /// </summary>
        /// <param name="code">Codigo enviado al correo del usuario para cambiar la contraseña</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("No cuenta con un código para reiniciar su contraseña.");
            }

            var message = "";
            if (TempData["message"] != null)
            {
                message = TempData["message"].ToString();
            }

            var model = new ResetPasswordViewModel { Code = code, Message = message };
            return View(model);
        }

        /// <summary>
        /// Cambia la clave del usuario
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para cambiar la clave del usuario</param>
        /// <returns>Redirecciona en caso de error o confirmación</returns>
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

        /// <summary>
        /// Obtiene la vista de confirmación de cambio de clave
        /// </summary>
        /// <returns>Retorna una vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("/acceso-denegado")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Busca un usuario de "departamento.academico" en caso no lo encuentre lo crea como gestor de secciones
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet]
        public async Task<IActionResult> CreateUserManagerSection()
        {
            var role = new ApplicationRole
            {
                Name = "Gestor de secciones",
            };
            var manager_section = await _userManager.FindByNameAsync("departamento.academico");

            //var r = await _userManager.AddToRoleAsync(manager_section, "Gestor de secciones");

            if (manager_section == null)
            {
                var address = new Address("es_MX");
                var name = new Name("es_MX");
                var user = new ApplicationUser { Address = address.StreetAddress(), Email = "departamento.academico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), UserName = "departamento.academico", Dni = "70345984", BirthDate = DateTime.Parse("1990-10-10") };
                var result = await _userManager.CreateAsync(user, "Enchufate.2018");

                await _userManager.AddToRoleAsync(user, "Gestor de secciones");
            }

            return Ok("Usuario creado");
        }


        [AllowAnonymous]
        public async Task<IActionResult> ExternalResponse(string returnurl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return RedirectToAction(nameof(Login), new { returnUrl = returnurl });

            var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                if (user.IsLockedOut)
                {
                    TempData["Error"] = true;
                    TempData["ErrorMessage"] = $"Usuario Bloqueado : {user.LockedOutReason}";
                    return RedirectToAction(nameof(Login), new { returnUrl = returnurl });
                }

                await _signInManager.SignInAsync(user, isPersistent : false);
                return RedirectToLocal(returnurl);
            }

            TempData["Error"] = true;
            TempData["ErrorMessage"] = $"ERROR : Intento Inválido";
            return RedirectToAction(nameof(Login), new { returnUrl = returnurl });
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
