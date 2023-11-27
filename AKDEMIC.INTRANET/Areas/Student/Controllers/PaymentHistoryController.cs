using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Areas.Student.Models.PaymentHistoryViewModels;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/historial_de_pagos")]
    public class PaymentHistoryController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceDetailsService _invoiceDetailsService;

        public PaymentHistoryController(IUserService userService,
            IInvoiceService invoiceService,
            IPaymentService paymentService,
            IInvoiceDetailsService invoiceDetailsService) : base(userService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;
            _invoiceDetailsService = invoiceDetailsService;
        }

        /// <summary>
        /// Vista principal donde se listan los pagos realizados por el alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de los pagos realizados por el alumno logueado
        /// </summary>
        /// <returns>Listado de pagos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetPaymentHistoryByTerm()
        {
            var userId = GetUserId();
            var payments = await _paymentService.GetAllByUser(userId, ConstantHelpers.PAYMENT.STATUS.PAID);

            var result = payments.Select(x => new
            {
                type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type],
                code = x.ConceptId.HasValue ? x.Concept.Code : "-",
                concept = x.Description,
                import = x.Total,
                discount = x.Discount,
                issueDate = x.IssueDate.ToShortDateString(),
                paymentDate = x.PaymentDate.ToLocalDateFormat(),
                x.Id
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Método que obtiene el detalle del pago realizado
        /// </summary>
        /// <param name="id">Identificador del pago</param>
        /// <returns>Datos del pago</returns>
        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> GetDetailInvoice(Guid id)
        {
            var invoiceDetails = await _invoiceDetailsService.GetByInvoice(id);

            var result = invoiceDetails.Select(x => new
            {
                numDocument = x.Invoice.Number,
                concept = x.Description,
                amount = "S/ " + x.Total
            }).ToList();

            return Ok(result);
        }
    }
}
