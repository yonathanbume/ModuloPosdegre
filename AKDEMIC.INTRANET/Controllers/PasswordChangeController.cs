// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.ViewModels.PasswordChangeViewModels;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenXmlPowerTools;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("cambiar-clave")]

    public class PasswordChangeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordChangeController(
            IUserService userService,
            IConfigurationService configurationService,
            UserManager<ApplicationUser> userManager
            )
        {
            _userService = userService;
            _configurationService = configurationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserByClaim(User);

            int.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.General.PASSWORD_EXPIRATION_DAYS), out var passwordExpirationDays);

            if (
                (user.FirstTime.HasValue && user.FirstTime.Value) ||
                (!user.PasswordChangeDate.HasValue) ||
                (passwordExpirationDays != 0 && user.PasswordChangeDate.HasValue && (System.DateTime.UtcNow - user.PasswordChangeDate.Value).TotalDays > passwordExpirationDays)
                )
            {
                return View();

            }
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        [HttpPost("actualizar")]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            var user = await _userService.GetUserByClaim(User);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(string.Join("; ", result.Errors.Select(x => x.Description).ToList()));

            user.FirstTime = false;
            user.PasswordChangeDate = DateTime.UtcNow;

            await _userService.Update(user);

            return Ok();
        }
    }
}
