using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class PaymentToValidateRepository : Repository<PaymentToValidate>, IPaymentToValidateRepository
    {
        private readonly IStudentRepository _studentRepository;
        public PaymentToValidateRepository(AkdemicContext context, IStudentRepository studentRepository) : base(context)
        {
            _studentRepository = studentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<PaymentToValidate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                //case "0":
                //    orderByPredicate = (x) => x.Description;
                //    break;
                //case "2":
                //    orderByPredicate = (x) => x.Dependency.Name;
                //    break;
                default:
                    break;
            }

            var query = _context.PaymentToValidates
                .Where(x => !x.EntityLoadFormat.IsPostgraduateFormat)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                    || x.User.FullName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.ConceptId.HasValue ? x.Description : "-",
                    amount = x.Amount,
                    user = x.User.UserName,
                    name = x.User.FullName,
                    date = x.Date.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserPaymentsDatatable(int paymentToValidateId)
        {
            var validateActiveTermPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.EconomicManagement.VALIDATE_ONLY_ACTIVE_TERM_PAYMENTS));
            var paymentToValidate = await _context.PaymentToValidates.FindAsync(paymentToValidateId);

            var query = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING && x.UserId == paymentToValidate.UserId)
                .AsNoTracking();

            if (validateActiveTermPayments)
                query = query.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (paymentToValidate.ConceptId.HasValue)
                query = query.Where(x => x.ConceptId == paymentToValidate.ConceptId);

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    description = x.IsPartialPayment ? $"{x.Description} - Pago Parcial" : x.Description,
                    amount = x.IsPartialPayment ? (x.Total - x.Payments.Sum(y => y.Total)) : x.Total,
                    date = x.IssueDate.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task ProcessAllPayments(ClaimsPrincipal user)
        {
            var validateActiveTermPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.EconomicManagement.VALIDATE_ONLY_ACTIVE_TERM_PAYMENTS));

            var paymentsToValidate = await _context.PaymentToValidates
                .Include(x => x.User).ToListAsync();

            var query = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
                .AsQueryable();

            if (validateActiveTermPayments)
                query = query.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var pendingPayments = await query.ToListAsync();

            var concepts = await _context.Concepts
                .Select(x => new
                {
                    x.Id,
                    x.IsDividedAmount,
                    x.DependencyId,
                    x.ConceptDistribution,
                    ConceptDistributionDetails = x.ConceptDistribution.ConceptDistributionDetails.ToList()
                }).ToListAsync();

            var incomes = new List<Income>();
            var paymentsToRemove = new List<PaymentToValidate>();
            foreach (var item in paymentsToValidate)
            {
                if (item.User.UserName == "72497248")
                {
                    var entro = true;
                }

                var payments = pendingPayments.Where(x => x.UserId == item.UserId).ToList();

                if (item.ConceptId.HasValue)
                    payments = pendingPayments.Where(x => x.ConceptId == item.ConceptId && x.UserId == item.UserId).ToList();

                if (payments.Sum(x => x.Total) == item.Amount)
                {
                    foreach (var payment in payments)
                    {
                        payment.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
                        payment.PaymentDate = item.Date;
                        payment.IsBankPayment = true;
                        payment.WasBankPaymentUsed = true;
                        payment.OperationCodeB = item.SecuenceCode;
                        payment.CurrentAccountId = item.CurrentAccountId;
                        payment.BankAgentCode = item.BankAgentCode;
                        payment.BankCashierCode = item.BankCashierCode;
                        payment.BankCondition = item.BankCondition;
                        payment.EntityLoadFormatId = item.EntityLoadFormatId;

                        var concept = concepts.FirstOrDefault(x => x.Id == payment.ConceptId);
                        if (concept != null)
                        {
                            if (!concept.IsDividedAmount)
                            {
                                //Si no se divide entonces el monto es completo 100%
                                incomes.Add(new Income
                                {
                                    PaymentId = payment.Id,
                                    DependencyId = concept.DependencyId,
                                    Date = item.Date,
                                    Amount = payment.Total,
                                    Invoice = item.SecuenceCode,
                                    Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                    Description = payment.Description,
                                    IsBankPayment = true,
                                    CurrentAccountId = payment.CurrentAccountId
                                });
                            }
                            else
                            {
                                //Por cada division de concepto creamos un income
                                foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                                {
                                    if (conceptDistributionDetail.IsUnit)
                                    {
                                        incomes.Add(new Income
                                        {
                                            PaymentId = payment.Id,
                                            DependencyId = concept.DependencyId,
                                            Date = item.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = item.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description}",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                    else
                                    {
                                        incomes.Add(new Income
                                        {
                                            PaymentId = payment.Id,
                                            DependencyId = conceptDistributionDetail.DependencyId.Value,
                                            Date = item.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = item.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description} ( {concept.ConceptDistribution.Name} )",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                }
                            }
                        }

                        if (payment.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE)
                        {
                            var userProcedure = await _context.UserProcedures.FindAsync(payment.EntityId.Value);

                            if (userProcedure != null)
                            {
                                var procedure = await _context.Procedures.FindAsync(userProcedure.ProcedureId);

                                if (userProcedure.StudentUserProcedureId.HasValue && procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC)
                                {
                                    var studentUserProcedure = await _context.StudentUserProcedures.FindAsync(userProcedure.StudentUserProcedureId.Value);
                                    var result = await _studentRepository.ExecuteProcedureActivity(user, userProcedure, studentUserProcedure);
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
                            }

                            payment.WasBankPaymentUsed = true;
                        }
                    }

                    paymentsToRemove.Add(item);
                }
            }

            _context.PaymentToValidates.RemoveRange(paymentsToRemove);
            await _context.Incomes.AddRangeAsync(incomes);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<bool, string>> ProcessUserPayments(int paymentToValidateId, List<Guid> payments, ClaimsPrincipal user)
        {
            var paymentToValidate = await _context.PaymentToValidates.FindAsync(paymentToValidateId);
            var validateActiveTermPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.EconomicManagement.VALIDATE_ONLY_ACTIVE_TERM_PAYMENTS));

            var query = _context.Payments
                .Where(x => x.UserId == paymentToValidate.UserId && payments.Contains(x.Id))
                .AsQueryable();

            if (validateActiveTermPayments)
                query = query.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var pendingPayments = await query
                .Include(x => x.Concept)
                .Include(x => x.Payments)
                .ToListAsync();

            var concepts = await _context.Concepts
                .Select(x => new
                {
                    x.Id,
                    x.IsDividedAmount,
                    x.DependencyId,
                    x.ConceptDistribution,
                    ConceptDistributionDetails = x.ConceptDistribution.ConceptDistributionDetails.ToList()
                }).ToListAsync();

            var incomes = new List<Income>();
            if (pendingPayments.Count == 0)
                return new Tuple<bool, string>(false, "Debe seleccionar por lo menos una deuda");

            if (paymentToValidate.ConceptId.HasValue && pendingPayments.Any(x => x.ConceptId != paymentToValidate.ConceptId))
                return new Tuple<bool, string>(false, "El saldo a favor solo puede usarse en deudas del mmismo concepto");

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (pendingPayments.Count > 1)
            {
                if (pendingPayments.Sum(x => x.Total - x.Payments.Sum(y => y.Total)) > paymentToValidate.Amount)
                    return new Tuple<bool, string>(false, "El monto de las deudas excede el monto a validar");

                foreach (var payment in pendingPayments)
                {
                    payment.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
                    payment.PaymentDate = paymentToValidate.Date;

                    if (!payment.IsPartialPayment)
                    {
                        payment.IsBankPayment = true;
                        payment.WasBankPaymentUsed = true;
                        payment.OperationCodeB = paymentToValidate.SecuenceCode;
                        payment.CurrentAccountId = paymentToValidate.CurrentAccountId;
                        payment.BankAgentCode = paymentToValidate.BankAgentCode;
                        payment.BankCashierCode = paymentToValidate.BankCashierCode;
                        payment.BankCondition = paymentToValidate.BankCondition;
                        payment.EntityLoadFormatId = paymentToValidate.EntityLoadFormatId;

                        var concept = concepts.FirstOrDefault(x => x.Id == payment.ConceptId);
                        if (concept != null)
                        {
                            if (!concept.IsDividedAmount)
                            {
                                //Si no se divide entonces el monto es completo 100%
                                incomes.Add(new Income
                                {
                                    Payment = payment,
                                    DependencyId = concept.DependencyId,
                                    Date = paymentToValidate.Date,
                                    Amount = payment.Total,
                                    Invoice = paymentToValidate.SecuenceCode,
                                    Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                    Description = payment.Description,
                                    IsBankPayment = true,
                                    CurrentAccountId = payment.CurrentAccountId
                                });
                            }
                            else
                            {
                                //Por cada division de concepto creamos un income
                                foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                                {
                                    if (conceptDistributionDetail.IsUnit)
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = payment,
                                            DependencyId = concept.DependencyId,
                                            Date = paymentToValidate.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description}",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                    else
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = payment,
                                            DependencyId = conceptDistributionDetail.DependencyId.Value,
                                            Date = paymentToValidate.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description} ( {concept.ConceptDistribution.Name} )",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var childPayment = new Payment
                        {
                            ParentPaymentId = payment.Id,

                            ConceptId = payment.ConceptId,
                            Description = $"{payment.Description} - Pago Parcial",
                            PaymentDate = paymentToValidate.Date,
                            EntityId = payment.EntityId,
                            UserId = payment.UserId,
                            Quantity = payment.Quantity,
                            Type = payment.Type,
                            IsPartialPayment = true,
                            Status = ConstantHelpers.PAYMENT.STATUS.PAID,
                            CurrentAccountId = paymentToValidate.CurrentAccountId.HasValue ? paymentToValidate.CurrentAccountId : payment.CurrentAccountId,
                            InvoiceId = null,
                            TermId = term != null ? term.Id : (Guid?)null,
                            IsBankPayment = true,
                            WasBankPaymentUsed = true,
                            OperationCodeB = paymentToValidate.SecuenceCode,
                            BankAgentCode = paymentToValidate.BankAgentCode,
                            BankCashierCode = paymentToValidate.BankCashierCode,
                            BankCondition = paymentToValidate.BankCondition,
                            EntityLoadFormatId = paymentToValidate.EntityLoadFormatId
                        };

                        childPayment.Total = payment.Total - payment.Payments.Sum(x => x.Total);
                        if (!payment.ConceptId.HasValue || payment.Concept.IsTaxed)
                            childPayment.SubTotal = Math.Round(childPayment.Total / (1.00M + ConstantHelpers.PAYMENT.IGV), 2, MidpointRounding.AwayFromZero);
                        else
                            childPayment.SubTotal = childPayment.Total;

                        childPayment.IgvAmount = childPayment.Total - childPayment.SubTotal;

                        await _context.Payments.AddAsync(childPayment);

                        var concept = concepts.FirstOrDefault(x => x.Id == payment.ConceptId);
                        if (concept != null)
                        {
                            if (!concept.IsDividedAmount)
                            {
                                //Si no se divide entonces el monto es completo 100%
                                incomes.Add(new Income
                                {
                                    Payment = childPayment,
                                    DependencyId = concept.DependencyId,
                                    Date = paymentToValidate.Date,
                                    Amount = childPayment.Total,
                                    Invoice = paymentToValidate.SecuenceCode,
                                    Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                    Description = childPayment.Description,
                                    IsBankPayment = true,
                                    CurrentAccountId = payment.CurrentAccountId
                                });
                            }
                            else
                            {
                                //Por cada division de concepto creamos un income
                                foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                                {
                                    if (conceptDistributionDetail.IsUnit)
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = childPayment,
                                            DependencyId = concept.DependencyId,
                                            Date = paymentToValidate.Date,
                                            Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{childPayment.Description}",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                    else
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = childPayment,
                                            DependencyId = conceptDistributionDetail.DependencyId.Value,
                                            Date = paymentToValidate.Date,
                                            Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{childPayment.Description} ( {concept.ConceptDistribution.Name} )",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                }
                            }
                        }

                        paymentToValidate.Amount -= childPayment.Total;
                    }

                    if (payment.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE)
                    {
                        var userProcedure = await _context.UserProcedures.FindAsync(payment.EntityId.Value);

                        if (userProcedure != null)
                        {
                            var procedure = await _context.Procedures.FindAsync(userProcedure.ProcedureId);

                            if (userProcedure.StudentUserProcedureId.HasValue && procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC)
                            {
                                var studentUserProcedure = await _context.StudentUserProcedures.FindAsync(userProcedure.StudentUserProcedureId.Value);
                                var result = await _studentRepository.ExecuteProcedureActivity(user, userProcedure, studentUserProcedure);
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
                        }

                        payment.WasBankPaymentUsed = true;
                    }
                }

                paymentToValidate.Amount -= pendingPayments.Sum(x => x.Total - x.Payments.Sum(y => y.Total));
            }
            else
            {
                var payment = pendingPayments.FirstOrDefault();
                var concept = concepts.FirstOrDefault(x => x.Id == payment.ConceptId);

                var totalAmount = payment.IsPartialPayment ? payment.Total - payment.Payments.Sum(y => y.Total) : payment.Total;

                if (paymentToValidate.Amount >= totalAmount)
                {
                    payment.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
                    payment.PaymentDate = paymentToValidate.Date;

                    if (!payment.IsPartialPayment)
                    {
                        payment.IsBankPayment = true;
                        payment.WasBankPaymentUsed = true;
                        payment.OperationCodeB = paymentToValidate.SecuenceCode;
                        payment.CurrentAccountId = paymentToValidate.CurrentAccountId;
                        payment.BankAgentCode = paymentToValidate.BankAgentCode;
                        payment.BankCashierCode = paymentToValidate.BankCashierCode;
                        payment.BankCondition = paymentToValidate.BankCondition;
                        payment.EntityLoadFormatId = paymentToValidate.EntityLoadFormatId;

                        if (concept != null)
                        {
                            if (!concept.IsDividedAmount)
                            {
                                //Si no se divide entonces el monto es completo 100%
                                incomes.Add(new Income
                                {
                                    Payment = payment,
                                    DependencyId = concept.DependencyId,
                                    Date = paymentToValidate.Date,
                                    Amount = payment.Total,
                                    Invoice = paymentToValidate.SecuenceCode,
                                    Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                    Description = payment.Description,
                                    IsBankPayment = true,
                                    CurrentAccountId = payment.CurrentAccountId
                                });
                            }
                            else
                            {
                                //Por cada division de concepto creamos un income
                                foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                                {
                                    if (conceptDistributionDetail.IsUnit)
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = payment,
                                            DependencyId = concept.DependencyId,
                                            Date = paymentToValidate.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description}",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                    else
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = payment,
                                            DependencyId = conceptDistributionDetail.DependencyId.Value,
                                            Date = paymentToValidate.Date,
                                            Amount = payment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{payment.Description} ( {concept.ConceptDistribution.Name} )",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                }
                            }
                        }

                        paymentToValidate.Amount -= payment.Total;
                    }
                    else
                    {
                        var childPayment = new Payment
                        {
                            ParentPaymentId = payment.Id,

                            ConceptId = payment.ConceptId,
                            Description = $"{payment.Description} - Pago Parcial",
                            PaymentDate = paymentToValidate.Date,
                            EntityId = payment.EntityId,
                            UserId = payment.UserId,
                            Quantity = payment.Quantity,
                            Type = payment.Type,
                            IsPartialPayment = true,
                            Status = ConstantHelpers.PAYMENT.STATUS.PAID,
                            CurrentAccountId = paymentToValidate.CurrentAccountId.HasValue ? paymentToValidate.CurrentAccountId : payment.CurrentAccountId,
                            InvoiceId = null,
                            TermId = term != null ? term.Id : (Guid?)null,
                            IsBankPayment = true,
                            WasBankPaymentUsed = true,
                            OperationCodeB = paymentToValidate.SecuenceCode,
                            BankAgentCode = paymentToValidate.BankAgentCode,
                            BankCashierCode = paymentToValidate.BankCashierCode,
                            BankCondition = paymentToValidate.BankCondition,
                            EntityLoadFormatId = paymentToValidate.EntityLoadFormatId,
                        };

                        childPayment.Total = payment.Total - payment.Payments.Sum(x => x.Total);
                        if (!payment.ConceptId.HasValue || payment.Concept.IsTaxed)
                            childPayment.SubTotal = Math.Round(childPayment.Total / (1.00M + ConstantHelpers.PAYMENT.IGV), 2, MidpointRounding.AwayFromZero);
                        else
                            childPayment.SubTotal = childPayment.Total;

                        childPayment.IgvAmount = childPayment.Total - childPayment.SubTotal;

                        await _context.Payments.AddAsync(childPayment);

                        if (concept != null)
                        {
                            if (!concept.IsDividedAmount)
                            {
                                //Si no se divide entonces el monto es completo 100%
                                incomes.Add(new Income
                                {
                                    Payment = childPayment,
                                    DependencyId = concept.DependencyId,
                                    Date = paymentToValidate.Date,
                                    Amount = childPayment.Total,
                                    Invoice = paymentToValidate.SecuenceCode,
                                    Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                    Description = childPayment.Description,
                                    IsBankPayment = true,
                                    CurrentAccountId = payment.CurrentAccountId
                                });
                            }
                            else
                            {
                                //Por cada division de concepto creamos un income
                                foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                                {
                                    if (conceptDistributionDetail.IsUnit)
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = childPayment,
                                            DependencyId = concept.DependencyId,
                                            Date = paymentToValidate.Date,
                                            Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{childPayment.Description}",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                    else
                                    {
                                        incomes.Add(new Income
                                        {
                                            Payment = childPayment,
                                            DependencyId = conceptDistributionDetail.DependencyId.Value,
                                            Date = paymentToValidate.Date,
                                            Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                            Invoice = paymentToValidate.SecuenceCode,
                                            Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                            Description = $"{childPayment.Description} ( {concept.ConceptDistribution.Name} )",
                                            IsBankPayment = true,
                                            CurrentAccountId = payment.CurrentAccountId
                                        });
                                    }
                                }
                            }
                        }

                        paymentToValidate.Amount -= childPayment.Total;
                    }

                    if (payment.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE)
                    {
                        var userProcedure = await _context.UserProcedures.FindAsync(payment.EntityId.Value);

                        if (userProcedure != null)
                        {
                            var procedure = await _context.Procedures.FindAsync(userProcedure.ProcedureId);

                            if (userProcedure.StudentUserProcedureId.HasValue && procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC)
                            {
                                var studentUserProcedure = await _context.StudentUserProcedures.FindAsync(userProcedure.StudentUserProcedureId.Value);
                                var result = await _studentRepository.ExecuteProcedureActivity(user, userProcedure, studentUserProcedure);
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
                        }

                        payment.WasBankPaymentUsed = true;
                    }
                }
                else
                {
                    payment.IsPartialPayment = true;

                    var childPayment = new Payment
                    {
                        ParentPaymentId = payment.Id,

                        ConceptId = payment.ConceptId,
                        Description = $"{payment.Description} - Pago Parcial",
                        PaymentDate = paymentToValidate.Date,
                        EntityId = payment.EntityId,
                        UserId = payment.UserId,
                        Quantity = payment.Quantity,
                        Type = payment.Type,
                        IsPartialPayment = true,
                        Status = ConstantHelpers.PAYMENT.STATUS.PAID,
                        CurrentAccountId = paymentToValidate.CurrentAccountId.HasValue ? paymentToValidate.CurrentAccountId : payment.CurrentAccountId,
                        InvoiceId = null,
                        TermId = term != null ? term.Id : (Guid?)null,
                        IsBankPayment = true,
                        WasBankPaymentUsed = true,
                        OperationCodeB = paymentToValidate.SecuenceCode,
                        BankAgentCode = paymentToValidate.BankAgentCode,
                        BankCashierCode = paymentToValidate.BankCashierCode,
                        BankCondition = paymentToValidate.BankCondition,
                        EntityLoadFormatId = paymentToValidate.EntityLoadFormatId,
                    };

                    childPayment.Total = paymentToValidate.Amount;
                    if (!payment.ConceptId.HasValue || payment.Concept.IsTaxed)
                        childPayment.SubTotal = Math.Round(childPayment.Total / (1.00M + ConstantHelpers.PAYMENT.IGV), 2, MidpointRounding.AwayFromZero);
                    else
                        childPayment.SubTotal = childPayment.Total;

                    childPayment.IgvAmount = childPayment.Total - childPayment.SubTotal;

                    var partialPayments = payment.Payments.Sum(x => x.Total) + childPayment.Total;
                    if (partialPayments >= payment.Total)
                    {
                        payment.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
                        payment.PaymentDate = paymentToValidate.Date;
                    }

                    await _context.Payments.AddAsync(childPayment);

                    if (concept != null)
                    {
                        if (!concept.IsDividedAmount)
                        {
                            //Si no se divide entonces el monto es completo 100%
                            incomes.Add(new Income
                            {
                                Payment = childPayment,
                                DependencyId = concept.DependencyId,
                                Date = paymentToValidate.Date,
                                Amount = childPayment.Total,
                                Invoice = paymentToValidate.SecuenceCode,
                                Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                Description = childPayment.Description,
                                IsBankPayment = true,
                                CurrentAccountId = payment.CurrentAccountId
                            });
                        }
                        else
                        {
                            //Por cada division de concepto creamos un income
                            foreach (var conceptDistributionDetail in concept.ConceptDistributionDetails)
                            {
                                if (conceptDistributionDetail.IsUnit)
                                {
                                    incomes.Add(new Income
                                    {
                                        Payment = childPayment,
                                        DependencyId = concept.DependencyId,
                                        Date = paymentToValidate.Date,
                                        Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                        Invoice = paymentToValidate.SecuenceCode,
                                        Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                        Description = $"{childPayment.Description}",
                                        IsBankPayment = true,
                                        CurrentAccountId = payment.CurrentAccountId
                                    });
                                }
                                else
                                {
                                    incomes.Add(new Income
                                    {
                                        Payment = childPayment,
                                        DependencyId = conceptDistributionDetail.DependencyId.Value,
                                        Date = paymentToValidate.Date,
                                        Amount = childPayment.Total * conceptDistributionDetail.Weight / 100.0M,
                                        Invoice = paymentToValidate.SecuenceCode,
                                        Type = ConstantHelpers.Treasury.Income.Type.INCOME,
                                        Description = $"{childPayment.Description} ( {concept.ConceptDistribution.Name} )",
                                        IsBankPayment = true,
                                        CurrentAccountId = payment.CurrentAccountId
                                    });
                                }
                            }
                        }
                    }

                    paymentToValidate.Amount -= childPayment.Total;
                }
            }

            if (paymentToValidate.Amount <= 0)
                _context.PaymentToValidates.Remove(paymentToValidate);

            await _context.Incomes.AddRangeAsync(incomes);
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalPaymentsDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<PaymentToValidate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                //case "0":
                //    orderByPredicate = (x) => x.Description;
                //    break;
                //case "2":
                //    orderByPredicate = (x) => x.Dependency.Name;
                //    break;
                default:
                    break;
            }

            var query = _context.PaymentToValidates
                .Where(x => x.EntityLoadFormat.IsPostgraduateFormat)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ExternalUser.DocumentNumber.ToUpper().Contains(search.ToUpper())
                    || x.ExternalUser.FullName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.ConceptId.HasValue ? x.Description : "-",
                    amount = x.Amount,
                    user = x.ExternalUser.DocumentNumber,
                    name = x.ExternalUser.FullName,
                    date = x.Date.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }


    }
}
