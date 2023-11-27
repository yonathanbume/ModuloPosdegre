// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("comunidad")]
    public class CommunityController : BaseController
    {
        public CommunityController()
        {

        }

        public IActionResult Index() => View();
    }
}
