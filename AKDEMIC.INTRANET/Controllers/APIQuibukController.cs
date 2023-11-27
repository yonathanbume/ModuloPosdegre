using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AKDEMIC.ENTITIES.Models.Generals;
using Microsoft.AspNetCore.Identity;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.INTRANET.ViewModels.APIQuibukViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Controllers
{

    [Route("biblioteca")]
    public class APIQuibukController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        public static string theme = GeneralHelpers.GetInstitutionAbbreviation().ToLower();
        public static string logo_company = "/images/themes/" + theme.ToLower() + "/logo.png";
        public static string buttonColor = ConstantHelpers.THEME_PARAMETERS.COLORS[GeneralHelpers.GetInstitutionAbbreviation()];

        public APIQuibukController(
            IUserService userService,
            AkdemicContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger) : base(context, userManager)
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Obtiene los roles del usuario al logearse en biblioteca
        /// </summary>
        /// <param name="username">Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="passwordakdemic">Clave entre quibuk y akdemic</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("api/login/{username}/{password}/{passwordakdemic}")]
        public async Task<IActionResult> Index(string username, string password, string passwordakdemic)
        {
            if (passwordakdemic != ConstantHelpers.PASSWORDQUIBUK) return BadRequest();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = await _userService.GetByUserWeb(username);
                if (user != null) username = user.UserName;
            }

            if (user != null)
            {
                var mysqlPassword = ConvertHelpers.StringToMySQLPasswordHash(password);
                var mysqlOldPassword = ConvertHelpers.StringToMySQLOldPasswordHash(password);
                if (!string.IsNullOrEmpty(user.MysqlPasswordHash) && (user.MysqlPasswordHash == mysqlOldPassword || user.MysqlPasswordHash == mysqlPassword))
                {
                    var passwordHashed = _userManager.PasswordHasher.HashPassword(user, password);
                    user.PasswordHash = passwordHashed;
                    user.MysqlPasswordHash = null;
                    await _context.SaveChangesAsync();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            else return BadRequest();
        }

        /// <summary>
        /// Crea un nuevo usuario para quibuk
        /// </summary>
        /// <param name="model">Modelo que contiene el nuevo usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [AllowAnonymous, HttpPost("api/create/userquibuk")]
        public async Task<IActionResult> Create(UserQuibukViewModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.username,
                    Email = model.email,
                    NormalizedEmail = model.email.ToUpper(),
                    PhoneNumber = model.telefono,
                    Address = model.direccion,
                    Sex = (model.genero == "Masculino") ? ConstantHelpers.SEX.MALE : ConstantHelpers.SEX.FEMALE,
                    CivilStatus = model.estado_civil,
                    Dni = model.dni,
                    Name = model.nombres,
                    PaternalSurname = model.paterno,
                    MaternalSurname = model.materno,
                    BirthDate = model.fecha_nacimiento,
                    FullName = model.paterno + " " + model.materno + " " + model.nombres,
                    DocumentType = 2,
                    Document = model.dni,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.password);

                if (!result.Succeeded)
                    return BadRequest(result);

                switch (model.code_tipo_usuario)
                {
                    case "BIB":
                        await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.LIBRARIAN);
                        break;
                    case "ADMQK":
                        await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.QUIBUK_ADMIN);
                        break;
                    default:
                        await _userManager.AddToRoleAsync(user, ConstantHelpers.ROLES.OPAC_USER);
                        break;
                }

                await _context.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Actualiza la contraseña del usuario
        /// </summary>
        /// <param name="model">Modelo que contiene la nueva contraseña del usuario en Quibuk</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [AllowAnonymous, HttpPost("api/users/pwd")]
        public async Task<IActionResult> UpdatePassword(ResetPasswordQuibukViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.newPwd);
                var identityResult = await _userManager.UpdateAsync(user);

                if (!identityResult.Succeeded)
                    return BadRequest("Ocurrió un error en el servicio");

                return Ok(user.Id);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el periodo activo
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("api/terms")]
        public async Task<IActionResult> GetTerms()
        {
            try
            {
                var result = await _context.Terms
                             .Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                             .Select(x => new {
                                 Id = x.Id,
                                 StartDate = x.StartDate,
                                 EndDate = x.EndDate,
                             }).FirstAsync();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Envia un correo con para restablecer la contraseña de biblioteca
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para restablecer la contraseña</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [AllowAnonymous, HttpPost("api/forgotpassword/quibuk")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordQuibuk(ForgotPasswordQuibukViewnModel model) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    var url_biblioteca = "https://biblioteca.unica.edu.pe/password/email";
                    //var t = ConstantHelpers.
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        //return RedirectToAction(nameof(ForgotPasswordConfirmation));
                        return Redirect(url_biblioteca);
                    }

                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var code = "$2y$12$C3qyleFgIrxrupq.i8vST.mltYQEEk0j0XAuf3W8Mrve6MiBJHjnW";

                    var url_biblioteca_reset = $"https://biblioteca.unica.edu.pe/password/reset";
                    //var callbackUrl = Url.ResetPasswordCallbackLinkQuibuk(user.Id, code, Request.Scheme);
                    var urlToken = "";

                    urlToken = $"{url_biblioteca_reset}/{code}";

                    var systemurl = HttpContext.Request.Host.Value;
                    var callbackUrl = "<div class='example-emails margin-bottom-50'>" +
                      "<div width='100%' style='background:#ebedf2; padding: 50px 20px; color: #514d6a;'>" +
                          "<div style='max-width: 1000px; margin: 0px auto; font-size: 14px'>" +
                              "<table border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 20px'>" +
                                  "<tr>" +
                                      "<td style = 'vertical-align: top;'>" +
                                          "<img src='" + systemurl + logo_company + "' style='height:65px; margin-top:-17px' />" +
                                      "</td>" +
                                  "</tr>" +
                              "</table>" +
                              "<div style='padding: 40px 40px 20px 40px; background: #fff;'>" +
                                  "<table border= '0' cellpadding= '0' cellspacing= '0' style= 'width: 100%;'>" +
                                      "<tbody>" +
                                          "<tr>" +
                                              "<td>" +
                                                  "<div style='text-align: center'>" +
                                                      "<h4 style='margin-bottom: 20px; color: #24222f; font-weight: 600;font-size: 24px;margin-top: 15px;'>" +
                                                            "Estimado Usuario, has solicitado un cambio de contraseña en el sistema de Biblioteca" +
                                                      "</h4>" +
                                                  "</div>" +
                                                  "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                                      "Si no fuiste tú, comunicate con la oficina de tecnología de la Universidad." +
                                                  "</div>" +
                                                  "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                                      "Podrás recuperar tu contraseña al hacer click en el botón que te enviamos en este correo, <b>te recordamos que este es un mensaje automático y no es necesario responder a este correo.</b> " +
                                                  "</div>" +
                                                  "<div style='text-align: center'>" +
                                                      "<a href='" + urlToken + "' style='display:inline-block;padding:11px 30px;margin:20px 0px 30px;font-size:15px;color:#fff;background-color:#2c2e49;border-radius:5px;text-decoration:none'>" +
                                                          "Recuperar Contraseña" +
                                                      "</a>" +
                                                  "</div>" +
                                                  "<div style='text-align: center;color:#a09bb9'>" +
                                                  //"<p style='line-height:17px;margin:0px'> Powered by Oficina de Tecnologías de Información </p>" +
                                                  "</div>" +
                                              "</td>" +
                                          "</tr>" +
                                      "</tbody>" +
                                  "</table>" +
                              "</div>" +
                          "</div>" +
                      "</div>" +
                  "</div>";

                    _emailSender.SendEmailRecoverPasswordQuibuk(model.Email, "Solicitud de recuperacion de contraseña para " + user.Name, callbackUrl);
                }

                // If we got this far, something failed, redisplay form
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        /// <summary>
        /// Actualiza la información del usuario de Quibuk
        /// </summary>
        /// <param name="model">Modelo que contiene la información del usuario de Quibuk</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [AllowAnonymous, HttpPost("api/update/userquibuk")]
        public async Task<IActionResult> Update(UserQuibukViewModel model)
        {
            try
            {
                var entity = await _userManager.FindByIdAsync(model.Id);

                entity.Name = model.nombres;
                entity.PaternalSurname = model.paterno;
                entity.MaternalSurname = model.materno;
                entity.Dni = model.dni;
                entity.Document = model.dni;
                entity.BirthDate = model.fecha_nacimiento.ToUtcDateTime();
                entity.CivilStatus = model.estado_civil;
                entity.Sex = (model.genero == "Masculino") ? 1 : 2;
                entity.Email = model.email;
                entity.NormalizedEmail = model.email.ToUpper();
                entity.PhoneNumber = model.telefono;
                entity.Address = model.direccion;
                //entity.UserName = model.username;

                await _context.SaveChangesAsync();


                return Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        /// <summary>
        /// Elimina el usuario de quibuk
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("api/delete/userquibuk/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var entity = await _userManager.FindByIdAsync(id);

                await _userManager.DeleteAsync(entity);

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
