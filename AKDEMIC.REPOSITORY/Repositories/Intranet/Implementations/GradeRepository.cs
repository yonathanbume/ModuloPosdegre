using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<int> CountByFilter(Guid? evaluationId = null)
        {
            var query = _context.Grades.AsQueryable();

            if (evaluationId.HasValue)
                query = query.Where(x => x.EvaluationId == evaluationId.Value);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Grade>> GetAll(Guid? studentSectionId = null, Guid? studentId = null, Guid? sectionId = null)
        {
            var query = _context.Grades
                .Include(x => x.Evaluation.CourseUnit)
                .AsNoTracking();

            if (studentSectionId.HasValue)
                query = query.Where(x => x.StudentSectionId == studentSectionId.Value);

            if (studentId.HasValue)
                query = query.Where(x => x.StudentSection.StudentId == studentId.Value);

            if (sectionId.HasValue)
                query = query.Where(x => x.StudentSection.SectionId == sectionId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Grade>> GetGradesByStudentSectionId(Guid studentSectionId)
        {
            return await _context.Grades.Where(x => x.StudentSectionId == studentSectionId).Include(x => x.Evaluation).ToListAsync();
        }

        public async Task<List<Grade>> GetGradesBySectionId(Guid sectionId)
            => await _context.Grades.Include(x => x.Evaluation.EvaluationType).Include(x => x.StudentSection.Student).Where(x => x.StudentSection.SectionId == sectionId).ToListAsync();

        public async Task<Select2Structs.ResponseParameters> GetStudentsBySectionAndEvaluation(Select2Structs.RequestParameters requestParameters, Guid sectionId, Guid? evaluationId = null, string searchValue = null)
        {
            if (evaluationId.HasValue)
            {
                var query = _context.Grades.Where(x => x.EvaluationId == evaluationId).AsNoTracking();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                    {
                        searchValue = $"\"{searchValue}*\"";
                        query = query.Where(x => EF.Functions.Contains(x.StudentSection.Student.User.FullName, searchValue));
                    }
                    else
                        query = query.Where(x => x.StudentSection.Student.User.FullName.Contains(searchValue));
                }

                return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"{x.StudentSection.Student.User.UserName} - {x.StudentSection.Student.User.FullName} - {x.Value}"

                }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE); 
            }
            else
            {
                var query = _context.SubstituteExams.Where(x => x.SectionId == sectionId).AsNoTracking();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                    {
                        searchValue = $"\"{searchValue}*\"";
                        query = query.Where(x => EF.Functions.Contains(x.Student.User.FullName, searchValue));
                    }
                    else
                        query = query.Where(x => x.Student.User.FullName.Contains(searchValue));
                }

                return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"{x.Student.User.UserName} - {x.Student.User.FullName} - {x.ExamScore}"

                }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE); 
            }
        }

        public async Task<object> GetStudentGradesDatatable(Guid studentSectionId)
        {
            var studentSection = await _context.StudentSections
                .Where(x => x.Id == studentSectionId)
                .Select(x => new {
                    x.Id,
                    x.Section.CourseTermId
                }).FirstOrDefaultAsync();

            var evaluations = await _context.Evaluations
                .Where(x => x.CourseTermId == studentSection.CourseTermId)
                .ToListAsync();

            var grades = await _context.Grades
                .Where(x => x.StudentSectionId == studentSectionId)
                .ToListAsync();

            var data = evaluations
                .OrderBy(x => x.Week)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    week = x.Week,
                    name = x.Name,
                    grade = grades.Any(y => y.EvaluationId == x.Id) ? grades.FirstOrDefault(y => y.EvaluationId == x.Id).Attended ? grades.FirstOrDefault(y => y.EvaluationId == x.Id).Value.ToString() : "NR" : "--"
                }).ToList();

            var recordsTotal = data.Count;

            return data;
        }

        public async Task<List<Grade>> GetGradesByStudentAndTerm(Guid studentId, Guid termId)
        {
            return await _context.Grades
                .Where(x => x.StudentSection.StudentId == studentId && x.StudentSection.Section.CourseTerm.TermId == termId)
                .ToListAsync();
        }
    }
}