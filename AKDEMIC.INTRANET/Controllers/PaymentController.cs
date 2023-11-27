// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.ViewModels.ProcedureViewModels;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("pagos")]
    public class PaymentController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IDataTablesService _dataTablesService;

        public PaymentController(
            IUserService userService,
            IPaymentService paymentService,
            IDataTablesService dataTablesService
            )
        {
            _userService = userService;
            _paymentService = paymentService;
            _dataTablesService = dataTablesService;
        }

        [HttpPost("validar-pago")]
        public async Task<IActionResult> ValidatePayment(PaymentViewModel model)
        {
            var user = await _userService.GetUserByClaim(User);
            var date = ConvertHelpers.DatepickerToDatetime(model.PaymentDate);
            var payment = await _paymentService.GetPaymentByOperationCodeToValidateProcedure(user.Id, date, model.OperationCode, model.Total, false);

            if (payment is null)
                return BadRequest("No se encontraron pagos con los datos ingresados.");

            return Ok(payment);
        }

        [HttpGet("get-pagos-no-usados/datatable")]
        public async Task<IActionResult> GetUnusedPaymentsDatatable(decimal? minAmount, Guid? conceptId = null)
        {
            var user = await _userService.GetUserByClaim(User);
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _paymentService.GetUnusedPaymentsDatatable(parameters, user.Id, minAmount, conceptId);
            return Ok(result);
        }
    }
}
