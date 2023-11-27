using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DEGREE.Areas.Admin.Models.ConfigurationViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/configuracion")]
    public class ConfigurationController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly ISelect2Service _select2Service;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ConfigurationController(
            IDataTablesService dataTablesService,
            IConfigurationService configurationService,
            IOptions<CloudStorageCredentials> storageCredentials
            ) : base(configurationService)
        {
            _storageCredentials = storageCredentials;
            _dataTablesService = dataTablesService;
        }

        /// <summary>
        /// Vista principal donde se cargan las variables de configuracion del sistema
        /// </summary>
        /// <returns>Retorna la vista</returns>
        public async Task<IActionResult> Index()
        {
            var template = await _configurationService.GetConfigurationTemplateAsync();

            var loginBackgroundImagePath = await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.DegreeManagement.GRAD_LOGIN_BACKGROUND_IMAGE);

            var model = new ConfigurationViewModel()
            {
                ConceptBachelorAutomatic = template.ConceptBachelorAutomatic,
                ConceptBachelorRequested = template.ConceptBachelorRequested,
                ConceptStaticTypeBachelor = template.ConceptStaticTypeBachelor,
                ConceptTitleDegreeProfessionalExperience = template.ConceptTitleDegreeProfessionalExperience,
                ConceptTitleDegreeSufficiencyExam = template.ConceptTitleDegreeSufficiencyExam,
                ConceptTitleDegreeSupportTesis = template.ConceptTitleDegreeSupportTesis,
                StaticTypeSystemIntegrated = template.StaticTypeSystemIntegrated,
                TupaBachelorAutomatic = template.TupaBachelorAutomatic,
                TupaBachelorRequested = template.TupaBachelorRequested,
                TupaStaticTypeBachelor = template.TupaStaticTypeBachelor,
                TupaTitleDegreeProfessionalExperience = template.TupaTitleDegreeProfessionalExperience,
                TupaTitleDegreeSufficiencyExam = template.TupaTitleDegreeSufficiencyExam,
                TupaTitleDegreeSupportTesis = template.TupaTitleDegreeSupportTesis,
                MethodTypeRegistryPatternCreation = template.MethodTypeRegistryPatternCreation,

                DegreeLoginBackgroundImagePath = loginBackgroundImagePath

            };
            return View(model);
        }

        /// <summary>
        /// Método para actualizar las variables de configuración del sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna un OK o BadRequest</returns>
        [HttpPost("actualizar")]
        public async Task<IActionResult> UpdateConfigurations(ConfigurationViewModel model)
        {
            var template = new ConfigurationTemplate()
            {
                ConceptBachelorAutomatic = model.ConceptBachelorAutomatic,
                ConceptBachelorRequested = model.ConceptBachelorRequested,
                ConceptStaticTypeBachelor = model.ConceptStaticTypeBachelor,
                ConceptTitleDegreeProfessionalExperience = model.ConceptTitleDegreeProfessionalExperience,
                ConceptTitleDegreeSufficiencyExam = model.ConceptTitleDegreeSufficiencyExam,
                ConceptTitleDegreeSupportTesis = model.ConceptTitleDegreeSupportTesis,
                TupaBachelorAutomatic = model.TupaBachelorAutomatic,
                TupaBachelorRequested = model.TupaBachelorRequested,
                TupaStaticTypeBachelor = model.TupaStaticTypeBachelor,
                TupaTitleDegreeProfessionalExperience = model.TupaTitleDegreeProfessionalExperience,
                TupaTitleDegreeSufficiencyExam = model.TupaTitleDegreeSufficiencyExam,
                TupaTitleDegreeSupportTesis = model.TupaTitleDegreeSupportTesis,
                MethodTypeRegistryPatternCreation = model.MethodTypeRegistryPatternCreation

            };

            await _configurationService.UpdateConfiguration(template);

            if (model.DegreeLoginBackgroundImage != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                var imagePath = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DegreeManagement.GRAD_LOGIN_BACKGROUND_IMAGE);

                if (!string.IsNullOrEmpty(imagePath))
                    await storage.TryDelete(imagePath, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION);

                var fileUrl = await storage.UploadFile(model.DegreeLoginBackgroundImage.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION,
                    Path.GetExtension(model.DegreeLoginBackgroundImage.FileName));

                await _configurationService.UpdateConfigurationValue(ConstantHelpers.Configuration.DegreeManagement.GRAD_LOGIN_BACKGROUND_IMAGE, fileUrl);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene la lista de tramites asignados a grados
        /// </summary>
        /// <returns>Retorna un Ok</returns>
        [HttpGet("obtener-lista-tramites-grados")]
        public async Task<IActionResult> GetDegreeProcedures()
        {
            var result = await _configurationService.GetProceduresSelect2();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la lista de conceptos
        /// </summary>
        /// <returns>Retorna un OK</returns>
        [HttpGet("obtener-lista-conceptos")]
        public async Task<IActionResult> GetConcepts()
        {
            var result = await _configurationService.GetConceptsSelect2();
            return Ok(result);
        }
    }
}
