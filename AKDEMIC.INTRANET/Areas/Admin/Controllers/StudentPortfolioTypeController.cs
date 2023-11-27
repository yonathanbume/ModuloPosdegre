// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.StudentPortfolioTypeModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/tipos-portafolio")]
    public class StudentPortfolioTypeController : BaseController
    {
        private readonly IStudentPortfolioTypeService _studentPortfolioTypeService;
        public StudentPortfolioTypeController(IDataTablesService dataTablesService,
            IStudentPortfolioTypeService studentPortfolioTypeService) : base(dataTablesService)
        {
            _studentPortfolioTypeService = studentPortfolioTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentPortfolioTypeService.GetDataDatatable(sentParameters, search);
            return Ok(result);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TypeViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Por favor, revise la información");

            var category = new StudentPortfolioType
            {
                Name = model.Name,
                DependencyId = model.DependencyId.HasValue && model.DependencyId != System.Guid.Empty ? model.DependencyId : null,
                Type = model.Type,
                CanUploadStudent = model.CanUploadStudent
            };

            await _studentPortfolioTypeService.Insert(category);
            return Ok();
        }

        [HttpPost("actualizar")]
        public async Task<IActionResult> Update(TypeViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Por favor, revise la información");

            var type = await _studentPortfolioTypeService.Get(model.Id);

            type.Name = model.Name;
            type.DependencyId = model.DependencyId.HasValue && model.DependencyId != System.Guid.Empty ? model.DependencyId : null;
            type.CanUploadStudent = model.CanUploadStudent;
            //type.Type = model.Type;

            await _studentPortfolioTypeService.Update(type);
            return Ok();
        }

        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _studentPortfolioTypeService.DeleteById(id);
            return Ok();
        }
    }
}
