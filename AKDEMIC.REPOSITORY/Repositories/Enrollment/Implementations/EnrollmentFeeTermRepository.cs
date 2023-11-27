using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentFeeTermRepository : Repository<EnrollmentFeeTerm>, IEnrollmentFeeTermRepository
    {
        public EnrollmentFeeTermRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<EnrollmentFeeTerm> GetByScaleAndTerm(Guid studentScaleId, Guid termId, Guid? campusId = null, Guid? careerId = null, Guid? conceptId = null)
        {
            var query = _context.EnrollmentFeeTerms
                .Where(x => x.StudentScaleId == studentScaleId && x.TermId == termId)
                .AsQueryable();

            if (campusId.HasValue && campusId != Guid.Empty)
                query = query.Where(x => x.CampusId == campusId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (conceptId.HasValue && conceptId != Guid.Empty)
                query = query.Where(x => x.ConceptId == conceptId);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid campusId, Guid? careerId = null, string search = null)
        {
            Expression<Func<EnrollmentFeeTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Campus.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StudentScale.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Concept.Amount;
                    break;
                default:
                    break;
            }

            var query = _context.EnrollmentFeeTerms
                .Where(x => x.TermId == termId && x.CampusId == campusId)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.StudentScale.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    termId = x.TermId,
                    campusId = x.CampusId,
                    campus = x.Campus.Name,
                    careerId = x.CareerId,
                    career = x.Career.Name,
                    scaleId = x.StudentScaleId,
                    scale = x.StudentScale.Name,
                    conceptId = x.ConceptId,
                    //amount = x.Concept.Amount,
                    amount = x.Total,
                    //fees = x.Details.Count,
                    //details = x.Details
                    //.Select(y => new
                    //{
                    //    id = y.Id,
                    //    issueDate = y.IssueDate.ToDateFormat(),
                    //    dueDate = y.DueDate.ToDateFormat(),
                    //    amount = y.Amount,
                    //    generated = y.WasGenerated,
                    //    available = y.IssueDate <= DateTime.UtcNow
                    //}).ToList()
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }


        public async Task DeleteWithAllFees(Guid enrollmentFeeTermId)
        {
            var details = await _context.EnrollmentFeeDetails.Where(x => x.EnrollmentFee.EnrollmentFeeTermId == enrollmentFeeTermId).ToListAsync();
            _context.EnrollmentFeeDetails.RemoveRange(details);

            var fees = await _context.EnrollmentFees.Where(x => x.EnrollmentFeeTermId == enrollmentFeeTermId).ToListAsync();
            _context.EnrollmentFees.RemoveRange(fees);

            var term = await _context.EnrollmentFeeTerms.FindAsync(enrollmentFeeTermId);
            _context.EnrollmentFeeTerms.Remove(term);
            await _context.SaveChangesAsync();
        }
    }
}
