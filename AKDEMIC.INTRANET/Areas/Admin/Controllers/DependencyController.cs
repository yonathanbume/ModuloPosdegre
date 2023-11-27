using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Areas.Admin.Models.DependencyViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.CORE.Structs;
using System.IO;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.DEPENDENCY + "," + ConstantHelpers.ROLES.DOCUMENT_RECEPTION + "," + ConstantHelpers.ROLES.OFFICE + "," + ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/dependencias")]
    public class DependencyController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _cloudStorageCredentials;
        private readonly IDataTablesService _dataTablesService;
        private readonly ISelect2Service _select2Service;
        private readonly IDependencyService _dependencyService;
        private readonly IConfigurationService _configurationService;

        public DependencyController(
            IOptions<CloudStorageCredentials> cloudStorageCredentials,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            IDependencyService dependencyService,
            IConfigurationService configurationService)
        {
            _cloudStorageCredentials = cloudStorageCredentials;
            _dataTablesService = dataTablesService;
            _select2Service = select2Service;
            _dependencyService = dependencyService;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Vista donde se gestionan las dependencias
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de dependencias
        /// </summary>
        /// <returns>Listado de dependencias</returns>
        [Route("get")]
        public async Task<IActionResult> GetDependencies()
        {
            var result = await _dependencyService.GetDependencies();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de dependencias
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Listado de dependencias</returns>
        [Route("datatable/get")]
        public async Task<IActionResult> GetDependenciesDatatable(string searchValue, string userId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            DataTablesStructs.ReturnedData<ENTITIES.Models.DocumentaryProcedure.Dependency> result;

            if (userId != null)
            {
                result = await _dependencyService.GetDependenciesDatatableByUser(sentParameters, userId, searchValue);
            }
            else
            {
                result = await _dependencyService.GetDependenciesDatatable(sentParameters, searchValue);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de dependencias para usarlo en select
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Listado de dependencias</returns>
        [Route("select2/get")]
        public async Task<IActionResult> GetDependenciesSelect2(string searchValue)
        {
            var requestParameters = _select2Service.GetRequestParameters();
            var result = await _dependencyService.GetDependenciesSelect2(requestParameters, searchValue ?? requestParameters.SearchTerm);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene los detalles de la dependencia
        /// </summary>
        /// <param name="did">Identificador de la dependencia</param>
        /// <returns>Datos de la dependencia</returns>
        [Route("{did}/get")]
        public async Task<IActionResult> GetDependency(Guid did)
        {
            var result = await _dependencyService.Get(did);

            return Ok(result);
        }

        /// <summary>
        /// Método para crear una dependencia
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva dependencia</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("crear/post")]
        [HttpPost]
        public async Task<IActionResult> CreateDependency(DependencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dependencyAny = await _dependencyService.AnyDependencyByName(model.Name);

            if (dependencyAny)
            {
                return BadRequest("El nombre de la dependencia ya existe");
            }

            var dependency = new ENTITIES.Models.DocumentaryProcedure.Dependency
            {
                UserId = model.UserId,
                Acronym = model.Acronym,
                Name = model.Name,
                Signature = model.Signature
            };

            if (model.SignatureFile != null)
            {
                var fileName = model.SignatureFile.FileName;

                if (model.SignatureFile.Length > CORE.Helpers.ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{fileName}' excede el límite de {CORE.Helpers.ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC / 1024 / 1024}MB");
                }

                if (!model.SignatureFile.HasContentType(CORE.Helpers.ConstantHelpers.DOCUMENTS.MIME_TYPE.IMAGE.GENERIC))
                {
                    return BadRequest($"El contenido del archivo '{fileName}' es inválido");
                }

                var cloudStorageService = new CloudStorageService(_cloudStorageCredentials);

                dependency.Signature = await cloudStorageService.UploadFile(model.SignatureFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.DEPENDENCY_SIGNATURE,
                    Path.GetExtension(fileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _dependencyService.Insert(dependency);

            return Ok();
        }

        /// <summary>
        /// Método para editar una dependencia
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la dependencia</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("editar/post")]
        [HttpPost]
        public async Task<IActionResult> UpdateDependency(DependencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dependency = await _dependencyService.Get(model.Id);
            var dependencyAny = await _dependencyService.AnyDependencyByName(model.Name);

            if (dependencyAny && dependency.Name != model.Name)
            {
                return BadRequest("El nombre de la dependencia ya existe");
            }

            dependency.UserId = model.UserId;
            dependency.Acronym = model.Acronym;
            dependency.Name = model.Name;

            if (model.SignatureFile != null)
            {
                var fileName = model.SignatureFile.FileName;

                if (model.SignatureFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC)
                {
                    return BadRequest($"El tamaño del archivo '{fileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.IMAGE.GENERIC / 1024 / 1024}MB");
                }

                if (!model.SignatureFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.IMAGE.GENERIC))
                {
                    return BadRequest($"El contenido del archivo '{fileName}' es inválido");
                }

                var cloudStorageService = new CloudStorageService(_cloudStorageCredentials);
                //var tmpFileName = Path.GetFileName(dependency.Signature);

                if (!string.IsNullOrEmpty(dependency.Signature))
                    await cloudStorageService.TryDelete(dependency.Signature, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.DEPENDENCY_SIGNATURE);

                dependency.Signature = await cloudStorageService.UploadFile(model.SignatureFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.DEPENDENCY_SIGNATURE,
                    Path.GetExtension(model.SignatureFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _dependencyService.Update(dependency);

            return Ok();
        }


        /// <summary>
        /// Método para eliminar una dependencia
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la dependencia</param>
        /// <returns>Código de estado HTTp</returns>
        [Route("eliminar/post")]
        [HttpPost]
        public async Task<IActionResult> DeleteDependency(DependencyViewModel model)
        {
            var dependency = await _dependencyService.Get(model.Id);

            if (!string.IsNullOrEmpty(dependency.Signature))
            {
                var cloudStorageService = new CloudStorageService(_cloudStorageCredentials);
                var fileName = Path.GetFileName(dependency.Signature);

                await cloudStorageService.TryDelete(fileName, ConstantHelpers.CONTAINER_NAMES.DEPENDENCY_SIGNATURE);
            }

            await _dependencyService.Delete(dependency);

            return Ok();
        }
    }
}
