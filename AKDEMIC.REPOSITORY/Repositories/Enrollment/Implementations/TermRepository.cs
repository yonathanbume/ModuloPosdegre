using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class TermRepository : Repository<Term>, ITermRepository
    {
        public TermRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Term>> GetTermsByStudentSections(Guid studentId)
            => await _context.Terms.Where(x => x.CourseTerms.Any(ct => ct.Sections.Any(s => s.StudentSections.Any(ss => ss.StudentId == studentId)))).ToListAsync();

        public async Task<IEnumerable<Term>> GetTermsByStudentSectionsAndAcademicHistories(Guid studentId)
            => await _context.Terms.Where(x => x.CourseTerms.Any(ct => ct.Sections.Any(s => s.StudentSections.Any(ss => ss.StudentId == studentId))) || x.AcademicHistories.Any(y=>y.StudentId == studentId)).ToListAsync();

        #region PRIVATE
        private async Task<Select2Structs.ResponseParameters> GetTermsSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Term, Select2Structs.Result>> selectPredicate, string searchValue = null)
        {
            var query = _context.Terms
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        private async Task<Select2Structs.ResponseParameters> GetTermsByCourseIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Term, Select2Structs.Result>> selectPredicate, Guid? courseId = null, string searchValue = null)
        {
            var query = _context.Terms
                .Where(x => x.CourseTerms.Any(y => y.CourseId == courseId))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        #endregion

        #region PUBLIC
        public async Task<Select2Structs.ResponseParameters> GetTermsSelect2(Select2Structs.RequestParameters requestParameters)
        {
            var active = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            return await GetTermsSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
                Selected = active.Id == x.Id
            });
        }

        public async Task<Select2Structs.ResponseParameters> GetTermsByCourseIdSelect2(Select2Structs.RequestParameters requestParameters, Guid? courseId = null, string searchValue = null)
        {
            return await GetTermsByCourseIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, courseId, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetTermsSelect2ClientSide(bool OnlyTermWithInstitutionalWelfareSurvey)
        {
            var query = _context.Terms.AsQueryable();
            if (OnlyTermWithInstitutionalWelfareSurvey)
            {
                query = query.Where(x => x.InstitutionalWelfareAnswerByStudents.Count > 0);
            }
            var result = await query
                .OrderByDescending(x => x.Name)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name,
                    Selected = x.Status == ConstantHelpers.TERM_STATES.ACTIVE
                })
            .ToArrayAsync();

            return result;
        }
        public async Task<IEnumerable<Select2Structs.Result>> GetLastTermsSelect2ClientSide(int? yearDifference)
        {
            var currentYear = DateTime.UtcNow.Year;
            var firstYear = currentYear - yearDifference;

            var result = await _context.Terms
                .Where(x=>x.Year >= firstYear)
                .OrderByDescending(x => x.Name)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name,
                    Selected = x.Status == ConstantHelpers.TERM_STATES.ACTIVE
                })
            .ToArrayAsync();

            return result;
        }

        public async Task<bool> ExistActiveTerm()
        {
            return await _context.Terms.AnyAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
        }

        public async Task<Term> GetActiveTerm()
        {
            return await _context.Terms
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
        }

        public async Task<Term> GetLastTerm(bool? includeSummer = true)
        {

            var query = _context.Terms.AsNoTracking();

            if (includeSummer.HasValue && !includeSummer.Value)
                query = query.Where(x => !x.IsSummer);

            var term = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED);
            
            return term;
        }

        public async Task<Term> GetTermByDateTime(DateTime dateTime)
        {
            return await _context.Terms.FirstOrDefaultAsync(x => x.StartDate <= dateTime && x.EndDate >= dateTime && x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
        }

        public async Task<IEnumerable<int>> GetTermYears()
        {
            var result = await _context.Terms
                .Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE)
                .Select(x => x.Year)
                .OrderByDescending(x => x)
                .Distinct()
                .ToListAsync();

            return result;
        }

        public async Task<bool> IsActiveTermInTime()
        {
            var enabled = await _context.Terms.AnyAsync(x => x.ClassStartDate < DateTime.UtcNow);
            return enabled;
        }

        public async Task<Term> GetReservation()
        {
            var term = await _context.Terms
                    .Where(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE)
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Number)
                    .FirstOrDefaultAsync();

            return term;
        }

        public async Task<object> GetSelect2Terms(int? year = null)
        {
            var terms = _context.Terms.AsNoTracking();

            if (year != null)
                terms = terms.Where(x => x.Year == year);

            var result = await terms
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .Select(
                 x => new
                 {
                     id = x.Id,
                     text = x.Name
                 }).ToListAsync();

            return result;
        }

        public async Task<object> GetTermsFinishedSelect2ClientSide()
        {
            var result = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .Select(x => new
                {
                    id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetTermsByStudentIdSelect2ClientSide(Guid studentId)
        {
            var result = await _context.Terms
                .OrderByDescending(x=>x.Year).ThenByDescending(x=>x.Number)
                .Where(x => x.CourseTerms.Any(y => y.Sections.Any(z => z.StudentSections.Any(g => g.StudentId == studentId))))
                .Select(x => new
                {
                    x.Id,
                    Text = x.Name,
                    Selected = x.Status == ConstantHelpers.TERM_STATES.ACTIVE
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetTermsFinishedSelect2ClientSideByClassStartDate()
        {
            var result = await _context.Terms
                .Where(x => x.ClassStartDate <= DateTime.UtcNow)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetTermsWithStatus(int status)
        {
            var result = await _context.Terms
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name,
                    active = x.Status == status
                }).OrderByDescending(x => x.text).ToListAsync();

            return result;
        }
        public async Task<List<Term>> GetUserCodePrefixByTermId(Guid termId)
        {
            var result = await _context.Terms.Where(t => t.Id == termId).ToListAsync();
            return result;
        }

        public async Task<List<Term>> GetTermsStatus(int status)
        {
            var result = await _context.Terms
                 .Where(t => t.Status == status).ToListAsync();

            return result;
        }

        public async Task<Guid> GetIdFirst()
        {
            var result = (await _context.Terms.FirstOrDefaultAsync()).Id;
            return result;
        }

        public async Task<bool> GetTermByName(string name)
        {
            return await _context.Terms.AnyAsync(x => x.Name == name);
        }
        public async Task<List<Term>> GetTermByIdList(Guid termId)
        {
            var result = await _context.Terms.Where(x => x.Id == termId).ToListAsync();

            return result;
        }

        public async Task<Term> GetFirst()
        {
            var Term = await _context.Terms.FirstOrDefaultAsync();

            return Term;
        }
        public async Task<Term> GetByStatus(int status)
            => await _context.Terms.Where(x => x.Status == 1).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<Term, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Status);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.ClassStartDate);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.ClassEndDate);
                    break;
                default:
                    break;
            }

            var query = _context.Terms.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsTotal = await query.CountAsync();

            var terms = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var data = terms
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    state = EnumHelpers.GetDescription((ConstantHelpers.TermState)Enum.ToObject(typeof(ConstantHelpers.TermState), x.Status)),
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    classStartDate = x.ClassStartDate.ToLocalDateFormat(),
                    classEndDate = x.ClassEndDate.ToLocalDateFormat()
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> IsInvalidDateRange(Term term)
        {
            if (term.Id == Guid.Empty)
                return await _context.Terms.AnyAsync(t => (t.StartDate <= term.StartDate && term.StartDate <= t.EndDate) || (t.StartDate <= term.EndDate && term.EndDate <= t.EndDate) || (term.StartDate <= t.StartDate && t.StartDate <= term.EndDate) || (term.StartDate <= t.EndDate && t.EndDate <= term.EndDate));

            else
            {
                var test = await _context.Terms.FirstOrDefaultAsync(t => t.Id != term.Id && ((t.StartDate <= term.StartDate && term.StartDate <= t.EndDate) || (t.StartDate <= term.EndDate && term.EndDate <= t.EndDate) || (term.StartDate <= t.StartDate && t.StartDate <= term.EndDate) || (term.StartDate <= t.EndDate && t.EndDate <= term.EndDate)));
                return await _context.Terms.AnyAsync(t => t.Id != term.Id && ((t.StartDate <= term.StartDate && term.StartDate <= t.EndDate) || (t.StartDate <= term.EndDate && term.EndDate <= t.EndDate) || (term.StartDate <= t.StartDate && t.StartDate <= term.EndDate) || (term.StartDate <= t.EndDate && t.EndDate <= term.EndDate)));
            }
        }

        public async Task<object> GetPastTermsSelect2ClientSide()
        {
            var result = await _context.Terms
                .Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalStudentsEnrolled(Guid termId, Guid? campusId = null, ClaimsPrincipal user = null)
        {
            var qry = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId
                && y.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsNoTracking();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var careers = _context.Careers.Where(x => x.CareerDirectorId == userId).Select(x => x.Id).ToHashSet();
                qry = qry.Where(x => careers.Contains(x.CareerId));
            }

            if (campusId.HasValue && campusId != Guid.Empty) qry = qry.Where(x => x.CampusId == campusId);

            var result = await qry.CountAsync();
            return result;
        }

        #endregion
    }
}