using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Admin.Models.DocumentFormatViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/constancias")]
    public class DocumentFormatController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IDocumentFormatService _documentFormatService;

        public DocumentFormatController(
            IDataTablesService dataTablesService,
            IDocumentFormatService documentFormatService
            )
        {
            _dataTablesService = dataTablesService;
            _documentFormatService = documentFormatService;
        }

        public IActionResult Index()
            => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _documentFormatService.GetDocumentFormatsDatatable(parameters);
            return Ok(result);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(DocumentFormatViewModel model)
        {
            if (await _documentFormatService.AnyByRecordType(model.Id))
                return BadRequest("El tipo de formato ya se encuentra registrado.");

            var entity = new AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat
            {
                Id = model.Id,
                Type = model.Type,
            };

            await _documentFormatService.Insert(entity);
            return Ok(entity.Id);
        }

        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(byte id)
        {
            var entity = await _documentFormatService.Get(id);

            if (entity is null)
            {
                ErrorToastMessage("No se encontró el documento.", "Error");
                return RedirectToAction(nameof(DocumentFormatController.Index), "DocumentFormat");
            }

            var model = new DocumentFormatViewModel
            {
                Id = id,
                Title = entity.Title,
                Content = entity.Content,
            };

            return View(model);
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Edit(DocumentFormatViewModel model)
        {
            var entity = await _documentFormatService.Get(model.Id);

            entity.Title = model.Title;
            entity.Content = model.Content;

            await _documentFormatService.Update(entity);
            return Ok();
        }

        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(byte id)
        {
            var entity = await _documentFormatService.Get(id);
            await _documentFormatService.Delete(entity);

            return Ok();
        }
    }
}
