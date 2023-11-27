using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeRectificationRepository : Repository<GradeRectification>, IGradeRectificationRepository
    {
        public GradeRectificationRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<GradeRectification>> GetAll(string teacherId = null, Guid? termId = null)
        {
            var query = _context.GradeRectifications
                .Include(x => x.Grade.StudentSection.Student.User)
                .Include(x => x.Grade.StudentSection.Section.CourseTerm.Course)
                .Include(x => x.Grade.Evaluation)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (termId.HasValue)
                query = query.Where(x => x.Grade.Evaluation.CourseTerm.TermId == termId.Value);

            return await query.ToListAsync();
        }
        public async Task<bool> AnySubstituteexams(Guid studentId, Guid courseId)
            => await _context.SubstituteExams.AnyAsync(x => x.StudentId == studentId && x.CourseTerm.CourseId == courseId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
        {
            Expression<Func<GradeRectification, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Term.Name : x.SubstituteExam.CourseTerm.Term.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Teacher.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Course.Name : x.SubstituteExam.CourseTerm.Course.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Grade != null ? x.Grade.StudentSection.Student.User.UserName : x.SubstituteExam.Student.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Grade != null ? x.Grade.StudentSection.Student.User.FullName : x.SubstituteExam.Student.User.FullName;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Evaluation != null ? x.Evaluation.Name : "Sustitutorio";
                    break;
                case "6":
                    orderByPredicate = (x) => x.GradePrevious;
                    break;
                case "7":
                    orderByPredicate = (x) => x.GradeNew;
                    break;
                case "8":
                    orderByPredicate = (x) => x.State;
                    break;
                case "9":
                    orderByPredicate = (x) => x.Type;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.GradeRectifications
                      .AsQueryable();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (termId.HasValue)
                query = query.Where(x => x.Grade.Evaluation.CourseTerm.TermId == termId.Value);

            if (state.HasValue) query = query.Where(x => x.State == state);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Term.Name.ToUpper().Contains(searchValue.ToUpper()) : x.SubstituteExam.CourseTerm.Term.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Teacher.FullName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()) : x.SubstituteExam.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade != null ? x.Grade.StudentSection.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) : x.SubstituteExam.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade != null ? x.Grade.StudentSection.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) : x.SubstituteExam.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    term = x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Term.Name : x.SubstituteExam.CourseTerm.Term.Name,
                    teacher = $"{x.Teacher.UserName} - {x.Teacher.FullName}",
                    course = x.Grade != null ? x.Grade.StudentSection.Section.CourseTerm.Course.Name : x.SubstituteExam.CourseTerm.Course.Name,
                    code = x.Grade != null ? x.Grade.StudentSection.Student.User.UserName : x.SubstituteExam.Student.User.UserName,
                    student = x.Grade != null ? x.Grade.StudentSection.Student.User.FullName : x.SubstituteExam.Student.User.FullName,
                    evaluation = x.Evaluation != null ? x.Evaluation.Name : "Sustitutorio",
                    gradePrevious = x.GradePrevious,
                    gradeNew = x.GradeNew,
                    state = ConstantHelpers.GRADERECTIFICATION.STATE.VALUES[x.State],
                    type = ConstantHelpers.GRADERECTIFICATION.TYPE.VALUES[x.Type],
                    date = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "-"
                })
                .ToListAsync();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<bool> AnyByEvaluation(Guid evaluationId)
            => await _context.GradeRectifications.AnyAsync(x => x.EvaluationId == evaluationId);
    }
}
