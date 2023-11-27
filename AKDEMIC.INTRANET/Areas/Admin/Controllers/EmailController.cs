using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.EmailViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-correos")]
    public class EmailController : BaseController
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEmailManagementService _emailManagementService;
        private readonly IDataTablesService _dataTablesService;

        public EmailController(
            IConfigurationService configurationService,
            IEmailManagementService emailManagementService,
            IDataTablesService dataTablesService

            )
        {
            _configurationService = configurationService;
            _emailManagementService = emailManagementService;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista donde se gestionan los correos de la universidad
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var model = new ConfigurationViewModel
            {
                Email_Host = await GetConfigurationByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_HOST),
                Email_Port = await GetConfigurationByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_PORT),
                Email_Password = await GetConfigurationByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_PASSWORD),
                Email_Sender = await GetConfigurationByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL),
                Multiple_Email = Convert.ToBoolean(await GetConfigurationByKey(ConstantHelpers.Configuration.Email.EMAIL_MULTIPLE_ENABLED))
            };

            return View(model);
        }

        /// <summary>
        /// Método para actualizar la configuración de envío de correos
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de configuración</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("actualizar-general")]
        public async Task<IActionResult> UpdateGeneralDetails(ConfigurationViewModel model)
        {
            var email_host = await _configurationService.GetByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_HOST);
            var emaiL_port = await _configurationService.GetByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_SMTP_PORT);
            var email_password = await _configurationService.GetByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL_PASSWORD);
            var email_sender = await _configurationService.GetByKey(ConstantHelpers.Configuration.Email.GENERAL_EMAIL);
            var email_multiple_enabled = await _configurationService.GetByKey(ConstantHelpers.Configuration.Email.EMAIL_MULTIPLE_ENABLED);

            email_password.Value = model.Email_Password;
            email_sender.Value = model.Email_Sender;
            email_host.Value = model.Email_Host;
            emaiL_port.Value = model.Email_Port;
            email_multiple_enabled.Value = model.Multiple_Email.ToString();

            await _configurationService.Update(email_host);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de correos de la universidad
        /// </summary>
        /// <param name="system">Sistema</param>
        /// <returns>Listado de correos</returns>
        [HttpGet("get-datatable")]
        public async Task<IActionResult> Get(int? system)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _emailManagementService.GetEmailManagementDatatable(parameters, system);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar un correo
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del correo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> Insert(EmailViewModel model)
        {
            var multiple_email_enabled = Convert.ToBoolean(await GetConfigurationByKey(ConstantHelpers.Configuration.Email.EMAIL_MULTIPLE_ENABLED));

            if (!multiple_email_enabled)
                return BadRequest($"No esta habilitada la opción de multiples correos.");

            if (await _emailManagementService.AnyBySytem(model.System))
                return BadRequest($"Ya existe un correo registrado para el sistema {ConstantHelpers.Solution.Values[model.System]}");

            if (model.Password != model.ConfirmPassword)
                return BadRequest("Las contraseñas son diferentes");

            var entity = new EmailManagement
            {
                Email = model.Email,
                Password = model.Password,
                System = model.System
            };

            await _emailManagementService.Insert(entity);
            return Ok();
        }

        /// <summary>
        /// Método para editar un correo
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del correo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> Edit(EmailViewModel model)
        {
            var multiple_email_enabled = Convert.ToBoolean(await GetConfigurationByKey(ConstantHelpers.Configuration.Email.EMAIL_MULTIPLE_ENABLED));

            if (!multiple_email_enabled)
                return BadRequest($"No esta habilitada la opción de multiples correos.");

            if (await _emailManagementService.AnyBySytem(model.System, model.Id))
                return BadRequest($"Ya existe un correo registrado para el sistema '{ConstantHelpers.Solution.Values[model.System]}'");

            var entity = await _emailManagementService.Get(model.Id.Value);

            entity.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password != model.ConfirmPassword)
                    return BadRequest("Las contraseñas son diferentes");

                entity.Email = model.Email;
                entity.Password = model.Password;
                entity.System = model.System;
            }

            await _emailManagementService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Método para eliminar un correo
        /// </summary>
        /// <param name="id">Identificador del correo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _emailManagementService.Get(id);
            await _emailManagementService.Delete(entity);
            return Ok();
        }

        /// <summary>
        /// Obtiene el valor de la variable de configuración
        /// </summary>
        /// <param name="key">Identificador de la configuración</param>
        /// <returns>Valor de la configuración</returns>
        [NonAction]
        public async Task<string> GetConfigurationByKey(string key)
        {
            var entity = await _configurationService.GetByKey(key);

            if (entity == null)
            {
                var configuration = new Configuration
                {
                    Key = key,
                    Value = ConstantHelpers.Configuration.Email.DEFAULT_VALUES[key]
                };

                await _configurationService.Insert(configuration);

                return configuration.Value;
            }

            return entity.Value;
        }

    }
}
