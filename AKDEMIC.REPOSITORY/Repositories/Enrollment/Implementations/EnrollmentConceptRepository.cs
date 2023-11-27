using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentConceptRepository : Repository<EnrollmentConcept>, IEnrollmentConceptRepository
    {
        public EnrollmentConceptRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> Any(byte type, Guid conceptId, byte? condition = null)
        //=> await _context.EnrollmentConcepts.AnyAsync(x => x.Type == type && x.ConceptId == conceptId
        //&& condition == x.Condition);
        {
            var query = _context.EnrollmentConcepts
                .Where(x => x.Type == type && x.ConceptId == conceptId)
                .AsQueryable();

            if (condition.HasValue && condition != 0)
                query = query.Where(x => x.Condition == condition);
            else query = query.Where(x => !x.Condition.HasValue);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyConceptToReplace(byte type, Guid conceptToReplaceId, Guid careerId)
        {
            var query = _context.EnrollmentConcepts
                .Where(x => x.Type == type && x.ConceptToReplaceId == conceptToReplaceId && x.CareerId == careerId)
                .AsNoTracking();

            return await query.AnyAsync();
        }

        public async Task<bool> AnyByCareerAndAdmissionType(byte type, Guid? careerId, Guid? admissionTypeId, byte? condition = null)
        {
            var query = _context.EnrollmentConcepts
                .Where(x => x.Type == type)
                .AsQueryable();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);
            else query = query.Where(x => !x.CareerId.HasValue);

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);
            else query = query.Where(x => !x.AdmissionTypeId.HasValue);

            if (condition.HasValue && condition != 0)
                query = query.Where(x => x.Condition == condition);
            else query = query.Where(x => !x.Condition.HasValue);

            return await query.AnyAsync();
        }

        public async Task<List<EnrollmentConcept>> GetByTypeIncludeConcept(byte type)
        {
            return await _context.EnrollmentConcepts
                               .Where(x => x.Type == type)
                               .Include(x => x.Concept)
                               .ToListAsync();
        }

        public async Task<List<EnrollmentConcept>> GetAllIncludeConcept()
        {
            return await _context.EnrollmentConcepts
                .Include(x => x.Concept)
                .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchValue = null)
        {
            Expression<Func<EnrollmentConcept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Concept.Description;
                    break;
            }

            var query = _context.EnrollmentConcepts
                //.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT
                //|| x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT)
                .AsNoTracking();

            if (type.HasValue) query = query.Where(x => x.Type == type.Value);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Concept.Code,
                    name = x.Concept.Description,
                    amount = x.Concept.Amount,
                    conceptToReplace = x.ConceptToReplaceId.HasValue ? x.ConceptToReplace.Description : "",
                    career = x.CareerId.HasValue ? x.Career.Name : "",
                    type = ConstantHelpers.EnrollmentConcept.Type.VALUES[x.Type],
                    condition = x.Condition.HasValue && ConstantHelpers.Student.Condition.VALUES.ContainsKey(x.Condition.Value) ? ConstantHelpers.Student.Condition.VALUES[x.Condition.Value] : "-",
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolmentDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<EnrollmentConcept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Concept.Description;
                    break;
            }

            var query = _context.EnrollmentConcepts
                .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ENROLLMENT_CONCEPT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ENROLLMENT_CONCEPT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTRA_ENROLLMENT_CONCEPT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.EXONERATED_COURSE_CONCEPT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.LESS_THAN_TWELVE_CREDITS_ENROLLMENT
                || x.Type == ConstantHelpers.EnrollmentConcept.Type.UNBEATEN_ENROLLMENT_CONCEPT)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    account = x.Concept.Classifier.Code,
                    name = x.Concept.Description,
                    type = ConstantHelpers.EnrollmentConcept.Type.VALUES[x.Type],
                    career = x.CareerId.HasValue ? x.Career.Name : "---",
                    admissionType = x.AdmissionTypeId.HasValue ? x.AdmissionType.Name : "---"
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
    }
}
