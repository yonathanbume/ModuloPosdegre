// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/portafolio")]
    public class StudentPortfolioController : BaseController
    {
        private readonly IStudentPortfolioService _studentPortfolioService;
        private readonly IUserService _userService;
        private readonly IStudentPortfolioTypeService _studentPortfolioTypeService;
        private readonly IStudentService _studentService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public StudentPortfolioController(
            IStudentPortfolioService studentPortfolioService,
            IUserService userService,
            IStudentPortfolioTypeService studentPortfolioTypeService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IStudentService studentService
            )
        {
            _studentPortfolioService = studentPortfolioService;
            _userService = userService;
            _studentPortfolioTypeService = studentPortfolioTypeService;
            _studentService = studentService;
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index()
            => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable()
        {
            var user = await _userService.GetUserByClaim(User);
            var student = await _studentService.GetStudentByUser(user.Id);
            var result = await _studentPortfolioService.GetStudentPortfolioDatatable(student.Id, canUploadStudent: true);
            return Ok(result);
        }

        [HttpPost("subir")]
        public async Task<IActionResult> UploadPortfolio(IFormFile file, Guid typeId)
        {
            var user = await _userService.GetUserByClaim(User);
            var student = await _studentService.GetStudentByUser(user.Id);

            var portfolio = await _studentPortfolioService.Get(student.Id, typeId);

            if (portfolio == null)
            {
                portfolio = new StudentPortfolio
                {
                    StudentId = student.Id,
                    StudentPortfolioTypeId = typeId,
                };

                if (file != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    portfolio.File = await storage.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.STUDENT_PORTFOLIO,
                    Path.GetExtension(file.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                }

                await _studentPortfolioService.Insert(portfolio);
            }
            else
            {
                if (portfolio.IsValidated)
                    return BadRequest("El documento ha sido validado y no puede ser reemplazado.");

                if (file != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    portfolio.File = await storage.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.STUDENT_PORTFOLIO,
                    Path.GetExtension(file.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
                }

                await _studentPortfolioService.Update(portfolio);
            }

            return Ok();
        }
    }
}
