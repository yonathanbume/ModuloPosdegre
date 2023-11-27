using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermRepository : Repository<ApplicationTerm>, IApplicationTermRepository
    {
        public ApplicationTermRepository(AkdemicContext context) : base(context) { }

        public async Task<string> GetApplicationTermName(int status)
        {
            var model = await _context.ApplicationTerms
                .Where(a => a.Status == status)
                .Select(a => a.Term.Name)
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<object> GetTermsVacant()
        {
            var applicationterms = await _context.ApplicationTerms
                .Include(x => x.Term)
                .OrderByDescending(x => x.Term.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Term.Name
                })
                .ToListAsync();

            return applicationterms;
        }

        public async Task<ApplicationTerm> GetWithTerm(Guid id)
        {
            return await _context.ApplicationTerms
                .Where(x => x.Id == id)
                .Include(x => x.Term)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationTerm>> GetAllApplicationTermsWithData(Guid? termId = null)
        {
            var result = _context.ApplicationTerms
                .Include(x => x.Term).AsQueryable();

            if (termId != null)
                result = result.Where(x => x.TermId == termId);

            return await result.ToListAsync();
        }

        public async Task<object> GetAdmissionTerms()
        {
            var result = await _context.ApplicationTerms
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Term.Name
                }).OrderByDescending(x => x.text).ToListAsync();

            return result;
        }
        public async Task<ApplicationTerm> GetActiveTerm()
        {
            var active = await _context.ApplicationTerms
                .Include(x => x.Term)
                .OrderByDescending(x => x.Term.Year)
                .ThenByDescending(x => x.Term.Number)
                .ThenBy(x => x.Name)
                .FirstOrDefaultAsync(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE);

            return active;
        }
        public async Task<ApplicationTerm> GetLastTerm()
        {
            var term = await _context.ApplicationTerms
                .OrderByDescending(x => x.Term.Year)
                .ThenByDescending(x => x.Term.Number)
                .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED);
            return term;
        }

        public async Task<object> GetPostulantsApplication(Guid termId)
        {
            var applicationTerm = await _context.ApplicationTerms.Where(x => x.TermId == termId).FirstOrDefaultAsync();
            var result = await _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTerm.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    Dni = x.Document,
                    Career = x.Career.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetApplicationTermWithTerm()
        {
            var result = await _context.ApplicationTerms
                .OrderByDescending(x => x.Term.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Term.Name} - {x.Name}"
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ApplicationTerm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Term.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StartDate.ToLocalDateFormat();
                    break;
                case "3":
                    orderByPredicate = (x) => x.EndDate.ToLocalDateFormat();
                    break;
                //4 y 5 nullables
                case "6":
                    orderByPredicate = (x) => x.InscriptionStartDate.ToLocalDateFormat();
                    break;
                case "7":
                    orderByPredicate = (x) => x.InscriptionEndDate.ToLocalDateFormat();
                    break;
                case "8":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    //orderByPredicate = (x) => x.Term.Name;
                    break;
            }

            var query = _context.ApplicationTerms
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search.Normalize().ToUpper();

                query = query.Where(x => x.Term.Name.ToUpper().Contains(normalizedSearch)
                || x.Name.ToUpper().Contains(normalizedSearch));
            }

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).ThenByDescending(x => x.StartDate)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                //.OrderBy(x => x.Term.Name)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    termId = x.TermId,
                    term = x.Term.Name,
                    status = x.Status,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    extraStartDate = x.ExtraStartDate.HasValue ? x.ExtraStartDate.ToLocalDateTimeFormat() : "",
                    extraEndDate = x.ExtraEndDate.HasValue ? x.ExtraEndDate.ToLocalDateTimeFormat() : "",
                    inscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    inscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    publicationDate = x.PublicationDate.ToLocalDateFormat(),
                    canDelete = (x.AdmissionExamApplicationTerms == null || x.AdmissionExamApplicationTerms.Count() == 0) &&
                                (x.ApplicationTermAdmissionTypes == null || x.ApplicationTermAdmissionTypes.Count() == 0) &&
                                (x.Postulants == null || x.Postulants.Count() == 0) &&
                                (x.ApplicationTermManagers == null || x.ApplicationTermManagers.Count() == 0),
                    reinscriptionDays = x.ReinscriptionDays,
                    allowReinscription = x.ReinscriptionDays > 0
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetApplicationTermObject(Guid id)
        {
            var result = await _context.ApplicationTerms
                .Where(x => x.Id.Equals(id))
                .Select(x => new
                {
                    id = x.Id,
                    termId = x.TermId,
                    status = x.Status,
                    name = x.Name,
                    startDate = x.StartDate.ToString("dd/MM/yyyy"),
                    endDate = x.EndDate.ToString("dd/MM/yyyy"),
                    inscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    inscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    publicationDate = x.PublicationDate.ToString("dd/MM/yyyy"),
                    extraStartDate = x.ExtraStartDate.HasValue ? x.ExtraStartDate.Value.ToString("dd/MM/yyyy") : "",
                    extraEndDate = x.ExtraEndDate.HasValue ? x.ExtraEndDate.Value.ToString("dd/MM/yyyy") : "",
                }).FirstAsync();

            return result;
        }
        public async Task<bool> AnyAsync(Guid termId)
            => await _context.ApplicationTerms.AnyAsync(x => x.TermId.Equals(termId));
        public async Task<ApplicationTerm> AddApp()
        {
            return (await _context.AddAsync(new ApplicationTerm())).Entity;
        }
        public async Task<bool> AnyAsyncByFieldAndId(Guid termId, Guid id)
            => await _context.ApplicationTerms.AnyAsync(x => x.TermId.Equals(termId) && !x.Id.Equals(id));
        public async Task<bool> AnyAsyncByTermDate(DateTime startDate, DateTime endDate)
            => await _context.ApplicationTerms.AnyAsync(t =>
                            (t.StartDate <= startDate && startDate <= (t.ExtraEndDate == null ? t.EndDate : t.ExtraEndDate.Value)/*t.EndDate*/) ||
                            (t.StartDate <= endDate && endDate <= (t.ExtraEndDate == null ? t.EndDate : t.ExtraEndDate.Value)/*t.EndDate*/) ||
                            (startDate <= t.StartDate && t.StartDate <= endDate) ||
                            (startDate <= t.EndDate && t.EndDate <= endDate));
        public async Task<bool> AnyAsyncByTermDateById(Guid termId, DateTime startDate, DateTime endDate)
            => await _context.ApplicationTerms.AnyAsync(t => t.Id != termId &&
                            ((t.StartDate <= startDate && startDate <= (t.ExtraEndDate == null ? t.EndDate : t.ExtraEndDate.Value)/*t.EndDate*/) ||
                            (t.StartDate <= endDate && endDate <= (t.ExtraEndDate == null ? t.EndDate : t.ExtraEndDate.Value)/*t.EndDate*/) ||
                            (startDate <= t.StartDate && t.StartDate <= endDate) ||
                            (startDate <= t.EndDate && t.EndDate <= endDate)));
        public async Task<object> GetActiveApplicationTerms()
        {
            var resultclient = await _context.ApplicationTerms
                .Where(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE)
               .Select(x => new
               {
                   id = x.Id,
                   termName = x.Term.Name,
                   appTermName = x.Name
               })
               .ToListAsync();

            var result = resultclient
                .Select(x => new
                {
                    x.id,
                    text = $"{x.termName} - {x.appTermName}"
                })
                .OrderByDescending(x => x.text)
                .ToList();
            return result;
        }

        public async Task<object> GetAvailableInscriptionApplicationTerms()
        {
            var resultclient = await _context.ApplicationTerms
                .Where(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE
                && ((x.InscriptionStartDate <= DateTime.UtcNow && DateTime.UtcNow <= x.InscriptionEndDate)
                || (x.ExtraStartDate <= DateTime.UtcNow && DateTime.UtcNow <= x.ExtraEndDate)))
               .Select(x => new
               {
                   id = x.Id,
                   termName = x.Term.Name,
                   appTermName = x.Name
               })
               .ToListAsync();

            var result = resultclient
                .Select(x => new
                {
                    x.id,
                    text = $"{x.termName} - {x.appTermName}"
                })
                .OrderByDescending(x => x.text)
                .ToList();

            return result;
        }

        public async Task<List<Career>> GetCareers(Guid applicationTermId)
        {
            var careers = await _context.Careers
                .Where(x => x.CareerApplicationTerms.Any(y => y.ApplicationTermId == applicationTermId))
                .ToListAsync();

            return careers;
        }
    }
}
