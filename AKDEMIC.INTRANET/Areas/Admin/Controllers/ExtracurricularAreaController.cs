// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularAreaViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/areas-extracurriculares")]
    public class ExtracurricularAreaController : BaseController
    {
        private readonly IExtracurricularAreaService _extracurricularAreaService;

        public ExtracurricularAreaController(IDataTablesService dataTablesService,
            IExtracurricularAreaService extracurricularAreaService) : base(dataTablesService)
        {
            _extracurricularAreaService = extracurricularAreaService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> GetActivities(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _extracurricularAreaService.GetDataDatatable(sentParameters, search);
            return Ok(result);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create([Bind(Prefix = "Add")] ExtracurricularAreaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Revise la información ingresada");

            var area = new ExtracurricularArea
            {
                Name = model.Name,
                Type = model.Type
            };

            await _extracurricularAreaService.Insert(area);
            return Ok();
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Update([Bind(Prefix = "Edit")] ExtracurricularAreaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Revise la información ingresada");

            if (!model.Id.HasValue)
                return BadRequest("No se encontró el área indicada");

            var area = await _extracurricularAreaService.Get(model.Id.Value);
            area.Name = model.Name;
            area.Type = model.Type;

            await _extracurricularAreaService.Update(area);
            return Ok();
        }

        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _extracurricularAreaService.DeleteById(id);
            return Ok();
        }
    }
}
