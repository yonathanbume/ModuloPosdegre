using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class SectionEvaluationRepository : Repository<SectionEvaluation>, ISectionEvaluationRepository
    {
        public SectionEvaluationRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid? sectionId)
        {
            var query = _context.SectionEvaluations
                .Where(x=>x.Evaluation.CourseTerm.CourseId == courseId && x.Evaluation.CourseTerm.TermId == termId)
                .AsNoTracking();

            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId);

            int recordsFiltered = await query.CountAsync();
            var equivalentUnits = await query.AllAsync(x => x.Evaluation.CourseUnit.AcademicProgressPercentage == 0);

            var data = await query
              .OrderBy(x => x.Evaluation.CourseUnit.Number).ThenBy(x => x.Evaluation.CourseUnit.Name).ThenBy(x => x.Evaluation.Week)
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Evaluation.Name,
                    percentage = $"{x.Percentage}%",
                    unitName = x.Evaluation.CourseUnit.Name,
                    week = x.Evaluation.Week.HasValue ? $"Semana {x.Evaluation.Week}" : "No Asignado",
                    group = new
                    {
                        name = x.Evaluation.CourseUnitId.HasValue ? (!equivalentUnits ? $"{x.Evaluation.CourseUnit.Name} - PORCENTAJE : {x.Evaluation.CourseUnit.AcademicProgressPercentage}%" : $"{x.Evaluation.CourseUnit.Name}") : "EVALUACIONES DISPONIBLES",
                        id = x.Evaluation.CourseUnitId
                    }
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
