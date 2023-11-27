// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AKDEMIC.CORE.Helpers;
using AKDEMIC.INTRANET.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/evaluaciones-especiales")]
    public class SpecialEvaluationController : BaseController
    {
        public SpecialEvaluationController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
