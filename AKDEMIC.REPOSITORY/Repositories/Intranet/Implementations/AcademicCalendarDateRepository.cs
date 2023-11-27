using AKDEMIC.CORE.Exceptions;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AcademicCalendarDateRepository : Repository<AcademicCalendarDate>, IAcademicCalendarDateRepository
    {
        public AcademicCalendarDateRepository(AkdemicContext context) : base(context) { }

        public async Task<List<AcademicCalendarDate>> GetAcademicCalendarDateForActiveTerm()
        {
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            if (term == null)
            {
                throw new XMLHttpRequestException("No existen periodos activos");
            }

            var queue = _context.AcademicCalendarDates
                .Where(x => x.TermId == term.Id)
                .Select(x => new AcademicCalendarDate
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartFormattedDate = x.StartDate.ToLocalDateTimeFormat(),
                    EndFormattedDate = x.EndDate.ToLocalDateTimeFormat(),
                    Term = new Term
                    {
                        Name = x.Term.Name
                    }
                })
                .AsQueryable();

            return await queue.ToListAsync();
        }

        public async Task<object> GetAcademicCalendarDates(Guid termId, Term term)
        {
            var result = await _context.AcademicCalendarDates
            .Where(a => a.TermId == termId)
            .OrderBy(a => a.StartDate)
            .Select(a => new
            {
                name = a.Name,
                date = a.IsRange ? $"{a.StartDate:dd/MM/yyyy} - {a.EndDate:dd/MM/yyyy}" : $"{a.StartDate:dd/MM/yyyy}",
                procedure = a.ProcedureId.HasValue ? a.Procedure.Name : "---",
                id = a.Id,
                dateSort = a.StartDate,
                editable = true
            }).ToListAsync();


            result.Add(new
            {
                name = "Inicio del Periodo Académico",
                date = $"{term.StartDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.StartDate,
                editable = false
            });

            result.Add(new
            {
                name = "Fin del Periodo Académico",
                date = $"{term.EndDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.EndDate,
                editable = false
            });

            result.Add(new
            {
                name = "Matrícula de Alumnos",
                date = $"{term.EnrollmentStartDate:dd/MM/yyyy} - {term.EnrollmentEndDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.EnrollmentStartDate,
                editable = false
            });

            result.Add(new
            {
                name = "Matrícula Extemporánea",
                date = $"{term.ComplementaryEnrollmentStartDate:dd/MM/yyyy} - {term.ComplementaryEnrollmentEndDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.ComplementaryEnrollmentStartDate,
                editable = false
            });

            result.Add(new
            {
                name = "Rectificación de Matrícula",
                date = $"{term.RectificationStartDate:dd/MM/yyyy} - {term.RectificationEndDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.RectificationStartDate,
                editable = false
            });

            result.Add(new
            {
                name = "Inicio de Clases",
                date = $"{term.ClassStartDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.ClassStartDate,
                editable = false
            });

            result.Add(new
            {
                name = "Fin de Clases",
                date = $"{term.ClassEndDate:dd/MM/yyyy}",
                procedure = "---",
                id = term.Id,
                dateSort = term.ClassEndDate,
                editable = false
            });

            result = result.OrderBy(x => x.dateSort).ThenBy(x => x.name).ToList();

            return result;
        }

        public async Task<object> GetDateTerm(Guid termId)
        {
            var calendarDetail = await _context.AcademicCalendarDates
                .Where(a => a.Id == termId)
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    isRange = a.IsRange,
                    startDate = $"{a.StartDate:dd/MM/yyyy}",
                    endDate = $"{a.EndDate:dd/MM/yyyy}",
                    procedureId = a.ProcedureId,
                    procedure = a.ProcedureId.HasValue ? a.Procedure.Name : ""
                }).FirstAsync();

            return calendarDetail;
        }

        public async Task<List<AcademicCalendarDate>> GetAcademicCalendarDatesForTerm(Guid termId)
        {
            var result = await _context.AcademicCalendarDates
                .Where(x => x.TermId == termId)
                .ToListAsync();

            return result;
        }

    }
}
