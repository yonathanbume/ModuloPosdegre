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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseRecognitionRepository : Repository<CourseRecognition>, ICourseRecognitionRepository
    {
        public CourseRecognitionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetRecognitionAcademicHistoriesDatatable(Guid recognitionId)
        {
            var recognition = await _context.Recognitions.FirstOrDefaultAsync(x => x.Id == recognitionId);

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == recognition.StudentId && x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION && x.Validated)
                .Select(x => new
                {
                    x.Id,
                    x.CourseId,
                    x.Course.Code,
                    Course = x.Course.Name,
                    Term = x.Term.Name,
                    x.Grade,
                    x.Course.Credits,
                    EvaluationReport = x.EvaluationReportId.HasValue ? x.EvaluationReport.Code : "Sin Asignar"
                }).ToListAsync();

            var courseRecognitions = await _context.CoursesRecognition
                .Where(x => x.RecognitionId == recognitionId)
                .Select(x => x.CourseId)
                .ToListAsync();

            var data = academicHistories
                .Where(x => courseRecognitions.Contains(x.CourseId))
                .Select(x => x)
                .ToList();

            return data;
        }
    }
}
