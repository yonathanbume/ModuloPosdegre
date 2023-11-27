using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.INTRANET.Areas.Admin.Models.DebtManagementViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.DEBT_MANAGER)]
    [Area("Admin")]
    [Route("admin/gestor-de-deudas")]
    public class DebtManagementController : BaseController
    {
        private readonly IConceptService _conceptService;
        private readonly IPaymentService _paymentService;
        private readonly ISelect2Service _select2Service;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;
        private readonly IProcedureService _procedureService;
        private readonly IEnrollmentTurnService _enrollmentTurnService;
        private readonly ICareerEnrollmentShiftService _careerEnrollmentShiftService;
        private readonly IStudentUserProcedureService _studentUserProcedureService;
        private readonly IUserProcedureService _userProcedureService;

        public DebtManagementController(
            IUserService userService,
            IConceptService conceptService,
            IDataTablesService dataTablesService,
            IPaymentService paymentService,
            ISelect2Service select2Service,
            ITermService termService,
            IStudentService studentService,
            IProcedureService procedureService,
            IEnrollmentTurnService enrollmentTurnService,
            ICareerEnrollmentShiftService careerEnrollmentShiftService,
            IStudentUserProcedureService studentUserProcedureService,
            IUserProcedureService userProcedureService
        ) : base(userService, dataTablesService)
        {
            _conceptService = conceptService;
            _paymentService = paymentService;
            _select2Service = select2Service;
            _termService = termService;
            _studentService = studentService;
            _procedureService = procedureService;
            _enrollmentTurnService = enrollmentTurnService;
            _careerEnrollmentShiftService = careerEnrollmentShiftService;
            _studentUserProcedureService = studentUserProcedureService;
            _userProcedureService = userProcedureService;
        }

        /// <summary>
        /// Vista donde se detalla las deudas de los usuarios
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene los datos del usuario
        /// </summary>
        /// <param name="code">Nombre de usuario</param>
        /// <returns>Datos del usuario</returns>
        [HttpGet("usuario/{code}/valida")]
        public async Task<IActionResult> GetUserInfo(string code)
        {
            var user = await _userService.GetByUserName(code);

            if (user == null) return BadRequest("El usuario no existe");

            var result = new
            {
                code = user.NormalizedUserName,
                name = user.FullName,
                dni = user.Dni,
                type = 1
            };
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de conceptos
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de conceptos</returns>
        [HttpGet("conceptos/get")]
        public async Task<IActionResult> GetConcepts(string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _conceptService.GetAllConceptsDatatable(sentParameters, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de pagos exonerados
        /// </summary>
        /// <param name="code">Nombre de usuario</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de pagos exonerados</returns>
        [HttpGet("usuario/{code}/exonerados/get")]
        public async Task<IActionResult> GetExoneratedPayments(string code, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _paymentService.GetExoneratedPaymentsToUserDatatable(sentParameters, code, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de pagos pendientes del usuario
        /// </summary>
        /// <param name="code">Nombre de usuario</param>
        /// <returns>Listado de pagos pendientes</returns>
        [HttpGet("usuario/{code}/pendientes/get")]
        public async Task<IActionResult> GetPendingPayments(string code)
        {
            var user = await _userService.GetUserByClaim(User);

            var data = await _paymentService.GetPendingPayments(code);

            var result = data.Select(x => new
            {
                id = x.Id,
                type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "",
                concept = x.IsPartialPayment ? $"{x.Description} - Pago Parcial" : x.Description,
                totalamount = x.IsPartialPayment ? (x.Total - x.Payments.Sum(y => y.Total) * 1.00M) : x.Total * 1.00M,
                discount = x.Discount,
                isPartialPayment = x.IsPartialPayment,
                issueDate = x.IssueDate.ToLocalDateFormat(),
                isConcept = ConstantHelpers.PAYMENT.TYPES.CONCEPT == x.Type,
                user = string.IsNullOrEmpty(x.CreatedBy) ? "sistema" : x.CreatedBy,
                isCreator = user.UserName == x.CreatedBy
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Método para obtener el listado de usuarios tesoreros
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> Search(string term)
        {
            var sentParameters = _select2Service.GetRequestParameters();
            var result = await _userService.GetUsersTreasurySelect2(sentParameters, term);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Método para exonerar el pago
        /// </summary>
        /// <param name="id">Identificador del pago</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("usuario/tramites/exonerar")]
        public async Task<IActionResult> ExoneratePayment(Guid id)
        {
            var payment = await _paymentService.Get(id);

            if (payment.WasExonerated) return BadRequest("Esta deuda ya ha sido exonerada");

            //Concatenar -Exonerada en caso se requiera
            //payment.Description = $"{payment.Description} - Exonerado";

            //Cambiar el total si se requiere
            //payment.Total = 0;

            //Cambiar el estado a pagado ?
            payment.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
            payment.PaymentDate = DateTime.UtcNow;
            payment.WasExonerated = true;


            //Verificar si existe un tramite asociado al pago
            if (payment.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE && payment.EntityId.HasValue)
            {
                var userProcedure = await _userProcedureService.Get(payment.EntityId.Value);

                if (userProcedure != null)
                {
                    var procedure = await _procedureService.Get(userProcedure.ProcedureId);

                    if (userProcedure.StudentUserProcedureId.HasValue && procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC)
                    {
                        var studentUserProcedure = await _studentUserProcedureService.Get(userProcedure.StudentUserProcedureId.Value);

                        var result = await _studentService.ExecuteProcedureActivity(User, userProcedure, studentUserProcedure);
                    }

                    if (procedure.Score == ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC)
                    {
                        if (userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT)
                            userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED;

                    }
                    else
                    {
                        if (userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT)
                            userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED;
                    }

                    payment.WasBankPaymentUsed = true;
                }
            }


            await _paymentService.Update(payment);
            return Ok();
        }

        /// <summary>
        /// Método para asignar conceptos al alumno
        /// </summary>
        /// <param name="code">Nombre de usuario</param>
        /// <param name="conceptId">Identificador del concepto</param>
        /// <param name="description">Descripción</param>
        /// <returns>Código de estado HTTP</returns>
        //[HttpPost("usuario/{code}/conceptos/registrar")]
        //public async Task<IActionResult> CreatePayment(string code, Guid conceptId, string description)
        //{
        //    if (string.IsNullOrEmpty(description)) return BadRequest("Debe especificar una descripción");

        //    var user = await _userService.GetByUserName(code);
        //    var concept = await _conceptService.GetConcept(conceptId);

        //    var total = concept.Amount;
        //    var subtotal = total;
        //    var igvAmount = 0.00M;

        //    if (concept.IsTaxed)
        //    {
        //        subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
        //        igvAmount = total - subtotal;
        //    }

        //    var term = await _termService.GetActiveTerm();

        //    var payment = new Payment
        //    {
        //        Description = description,
        //        EntityId = concept.Id,
        //        Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.CONCEPT,
        //        UserId = user.Id,
        //        SubTotal = subtotal,
        //        IgvAmount = igvAmount,
        //        Discount = 0.00M,
        //        Total = total,
        //        ConceptId = concept.Id,
        //        TermId = term?.Id
        //    };

        //    await _paymentService.Insert(payment);

        //    return Ok(payment.Id);
        //}
        [HttpPost("usuario/conceptos/registrar")]
        public async Task<IActionResult> CreatePayment(ConceptViewModel model)
        {
            //if (string.IsNullOrEmpty(description)) return BadRequest("Debe especificar una descripción");

            var user = await _userService.GetByUserName(model.UserCode);
            var concept = await _conceptService.GetConcept(model.ConceptId);

            var total = model.Price;
            var subtotal = total;
            var igvAmount = 0.00M;

            if (concept.IsTaxed)
            {
                subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                igvAmount = total - subtotal;
            }

            var term = await _termService.GetActiveTerm();

            var payment = new Payment
            {
                Description = string.IsNullOrEmpty(model.Comment) ? concept.Description : $"{concept.Description} {model.Comment}",
                EntityId = concept.Id,
                Type = model.IsEnrollment ? ConstantHelpers.PAYMENT.TYPES.ENROLLMENT : ConstantHelpers.PAYMENT.TYPES.CONCEPT,
                UserId = user.Id,
                SubTotal = subtotal,
                IgvAmount = igvAmount,
                Discount = 0.00M,
                Total = total,
                ConceptId = concept.Id,
                TermId = term?.Id
            };

            await _paymentService.Insert(payment);

            return Ok(payment.Id);
        }

        [HttpPost("usuario/conceptos/eliminar")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var payment = await _paymentService.Get(id);
            await _paymentService.Delete(payment);
            return Ok();
        }
    }
}
