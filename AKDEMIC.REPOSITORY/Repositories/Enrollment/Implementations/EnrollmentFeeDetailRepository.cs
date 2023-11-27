using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.BachelorTypeConfiguration;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentFeeDetailRepository : Repository<EnrollmentFeeDetail>, IEnrollmentFeeDetailRepository
    {
        public EnrollmentFeeDetailRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task DeleteAllByEnrollmentFeeId(Guid enrollmentFeeId)
        {
            var result = await _context.EnrollmentFeeDetails.Where(x => x.EnrollmentFeeId == enrollmentFeeId).ToListAsync();

            _context.EnrollmentFeeDetails.RemoveRange(result);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<bool, string>> GenerateFeePayments(bool allPayments = false)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return new Tuple<bool, string>(false, "No hay periodo activo para generar las cuotas");

            var query = _context.EnrollmentFeeDetails
                .Where(x => x.TermId == term.Id && !x.WasGenerated)
                .AsQueryable();

            if (!allPayments)
                query = query.Where(x => x.IssueDate <= DateTime.UtcNow);

            var details = await query
                .Include(x => x.EnrollmentFee)
                .ThenInclude(x => x.EnrollmentFeeTerm)
                .ToListAsync();

            var students = await _context.Students
                .FilterActiveStudents()
                .Where(x => x.EnrollmentFeeId.HasValue)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.EnrollmentFeeId
                })
                .ToListAsync();

            var concepts = await _context.Concepts
                .Select(x => new
                {
                    x.Id,
                    x.IsTaxed,
                    x.Description,
                    x.CurrentAccountId
                }).ToListAsync();

            var payments = new List<Payment>();

            foreach (var item in details)
            {
                var concept = concepts.FirstOrDefault(x => x.Id == item.EnrollmentFee.EnrollmentFeeTerm.ConceptId);
                var feeStudents = students.Where(x => x.EnrollmentFeeId == item.EnrollmentFeeId);

                foreach (var student in feeStudents)
                {
                    var total = item.Amount;
                    var subtotal = total;
                    var igv = 0.00M;

                    if (concept.IsTaxed)
                    {
                        subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
                        igv = total - subtotal;
                    }

                    payments.Add(new Payment
                    {
                        Description = $"{concept.Description} - Cuota {item.FeeNumber}",
                        SubTotal = subtotal,
                        IgvAmount = igv,
                        Discount = 0.00M,
                        Total = total,
                        EntityId = item.Id,
                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = concept.Id,
                        TermId = term.Id,
                        CurrentAccountId = concept.CurrentAccountId,
                        IssueDate = item.IssueDate,
                        DueDate = item.DueDate
                    });
                }

                item.WasGenerated = true;
            }

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> GeneratePayments(Guid enrollmentFeeDetailId)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return new Tuple<bool, string>(false, "No hay periodo activo para generar las cuotas");

            var detail = await _context.EnrollmentFeeDetails
                .Where(x => x.Id == enrollmentFeeDetailId)
                .Include(x => x.EnrollmentFee)
                .ThenInclude(x => x.EnrollmentFeeTerm)
                .FirstOrDefaultAsync();

            if (detail.EnrollmentFee.EnrollmentFeeTerm.TermId != term.Id) return new Tuple<bool, string>(false, "El periodo de la cuota es diferente del periodo activo");
            if (detail.IssueDate > DateTime.UtcNow) return new Tuple<bool, string>(false, "No se encuentra en la fecha para generar la deuda");
            if (detail.WasGenerated) return new Tuple<bool, string>(false, "Las cuotas ya fueron generadas");        

            var students = await _context.Students
                .Where(x => x.EnrollmentFeeId == detail.EnrollmentFeeId)
                .Select(x => new
                {
                    x.Id,
                    x.UserId
                }).ToListAsync();

            var concept = await _context.Concepts
                .Where(x => x.Id == detail.EnrollmentFee.EnrollmentFeeTerm.ConceptId)
                .Select(x => new
                {
                    x.Id,
                    x.IsTaxed,
                    x.Description,
                    x.CurrentAccountId
                }).FirstOrDefaultAsync();

            var payments = new List<Payment>();

            foreach (var studentPayment in students)
            {
                var total = detail.Amount;
                var subtotal = total;
                var igv = 0.00M;

                if (concept.IsTaxed)
                {
                    subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
                    igv = total - subtotal;
                }

                payments.Add(new Payment
                {
                    Description = $"{concept.Description} - Cuota {detail.FeeNumber}",
                    SubTotal = subtotal,
                    IgvAmount = igv,
                    Discount = 0.00M,
                    Total = total,
                    EntityId = detail.Id,
                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                    UserId = studentPayment.UserId,
                    ConceptId = concept.Id,
                    TermId = term.Id,
                    CurrentAccountId = concept.CurrentAccountId,
                    IssueDate = detail.IssueDate,
                    DueDate = detail.DueDate
                });
            }

            detail.WasGenerated = true;

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public async Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeAndTerm(Guid enrollmentFeeId, Guid termId)
        {
            var result = await _context.EnrollmentFeeDetails.Where(x => x.EnrollmentFeeId == enrollmentFeeId && x.TermId == termId).ToListAsync();
            return result;
        }

        public async Task<List<EnrollmentFeeDetail>> GetAllByEnrollmentFeeTermId(Guid enrollmentFeeTermId)
        {
            var result = await _context.EnrollmentFeeDetails.Where(x => x.EnrollmentFee.EnrollmentFeeTermId == enrollmentFeeTermId).ToListAsync();
            return result;
        }

        public async Task<object> GetDataDatatable(Guid enrollmentFeeId)
        {
            var query = _context.EnrollmentFeeDetails
                .Where(x => x.EnrollmentFeeId == enrollmentFeeId)
                .AsNoTracking();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    number = x.FeeNumber,
                    issueDate = x.IssueDate.ToLocalDateFormat(),
                    dueDate = x.DueDate.ToLocalDateFormat(),
                    amount = x.Amount,
                })
                .ToListAsync();

            return data;
        }

    }
}
