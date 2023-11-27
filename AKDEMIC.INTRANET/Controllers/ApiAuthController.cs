// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.ViewModels.ApiAuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AKDEMIC.INTRANET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ApiAuthController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> RequestToken([FromBody] LoginViewModel model)
        {
            var clientId = _configuration.GetSection("ApiTokenManagement")["ClientId"];
            var secret = _configuration.GetSection("ApiTokenManagement")["ClientSecret"];

            if (model.ClientId == clientId && model.ClientSecret == secret)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null) return NotFound();

                return Ok(await _userManager.CheckPasswordAsync(user, model.Password));
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] string username)
        {
            var clientId = _configuration.GetSection("ApiTokenManagement")["ClientId"];
            var secret = _configuration.GetSection("ApiTokenManagement")["ClientSecret"];

            return Ok(username);
        }
    }
}
