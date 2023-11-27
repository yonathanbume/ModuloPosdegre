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
    public class GradeRecoveryRepository : Repository<GradeRecovery>, IGradeRecoveryRepository
    {
        public GradeRecoveryRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<GradeRecovery>> GetByGradeRecoveryExamId(Guid gradeRecoveryExamId)
            => await _context.GradeRecoveries.Where(x => x.GradeRecoveryExamId == gradeRecoveryExamId).Include(x => x.Student.User).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDatatable(DataTablesStructs.SentParameters parameters, Guid gradeRecoveryExamId, string searchValue)
        {
            var query = _context.GradeRecoveries.Where(x => x.GradeRecoveryExamId == gradeRecoveryExamId).AsQueryable();

            Expression<Func<GradeRecovery, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
                default:
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
            }

            var exam = await _context.GradeRecoveryExams.Where(x => x.Id == gradeRecoveryExamId).FirstOrDefaultAsync();

            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY).FirstOrDefaultAsync();

            if (confi is null)
            {
                confi = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY]
                };
            }

            var evaluationTypeAssigned = Guid.Parse(confi.Value);

            var queryGrades = _context.Grades
                .AsNoTracking();

            if (evaluationTypeAssigned != Guid.Empty)
            {
                queryGrades = queryGrades.Where(x => x.Evaluation.EvaluationTypeId == evaluationTypeAssigned);
            }

            var grades = await queryGrades
                .Include(x => x.Evaluation)
                .Where(x => x.StudentSection.SectionId == exam.SectionId)
                .Select(x => new
                {
                    x.Value,
                    evaluation = x.Evaluation.Name,
                    x.Evaluation.Percentage,
                    x.StudentSection.StudentId
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x =>
                    x.Student.User.FullName.Contains(searchValue) ||
                    x.Student.User.UserName.Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.UserName,
                    x.Student.User.FullName,

                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.UserName,
                    x.FullName,
                    minGrade = grades.Any(y => y.StudentId == x.StudentId) ? grades.Where(y => y.StudentId == x.StudentId).OrderBy(y => y.Value).ThenByDescending(x=>x.Percentage).Select(y => y.Value.ToString()).FirstOrDefault() : "Sin nota",
                    evaluation = grades.Any(y => y.StudentId == x.StudentId) ? grades.Where(y => y.StudentId == x.StudentId).OrderBy(y => y.Value).ThenByDescending(x => x.Percentage).Select(y => y.evaluation).FirstOrDefault() : "Sin evalaución",
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };


        }

        public async Task<object> GetAssignedStudentsWithData(Guid gradeRecoveryExamId)
        {
            var query = _context.GradeRecoveries.Where(x => x.GradeRecoveryExamId == gradeRecoveryExamId).AsQueryable();

            var exam = await _context.GradeRecoveryExams.Where(x => x.Id == gradeRecoveryExamId).FirstOrDefaultAsync();

            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY).FirstOrDefaultAsync();

            if (confi is null)
            {
                confi = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY]
                };
            }

            var evaluationTypeAssigned = Guid.Parse(confi.Value);

            var queryGrades = _context.Grades
                .AsNoTracking();

            if (evaluationTypeAssigned != Guid.Empty)
            {
                queryGrades = queryGrades.Where(x => x.Evaluation.EvaluationTypeId == evaluationTypeAssigned);
            }

            var grades = await queryGrades
              .Include(x => x.Evaluation)
              .Where(x => x.StudentSection.SectionId == exam.SectionId)
              .Select(x => new
              {
                  x.Id,
                  x.Value,
                  evaluation = x.Evaluation.Name,
                  x.Evaluation.Percentage,
                  x.StudentSection.StudentId
              })
              .ToListAsync();

            var dataDB = await query
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.StudentId,
                    x.UserName,
                    x.FullName,
                    evalutionIdToChange = grades.Where(y => y.StudentId == x.StudentId).OrderBy(y => y.Value).ThenByDescending(y=>y.Percentage).Select(y => y.Id).FirstOrDefault(),
                    minGrade = grades.Any(y => y.StudentId == x.StudentId) ? grades.Where(y => y.StudentId == x.StudentId).OrderBy(y => y.Value).ThenByDescending(y => y.Percentage).Select(y => y.Value.ToString()).FirstOrDefault() : "Sin nota",
                    evaluation = grades.Any(y => y.StudentId == x.StudentId) ? grades.Where(y => y.StudentId == x.StudentId).OrderBy(y => y.Value).ThenByDescending(y => y.Percentage).Select(y => y.evaluation).FirstOrDefault() : "Sin evalaución",
                })
                .ToList();

            return data;
        }


        public async Task<object> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId)
        {
            var query = _context.GradeRecoveries.Where(x => x.GradeRecoveryExamId == gradeRecoveryExamId).AsQueryable();

            var data = await query
                   .Select(x => new
                   {
                       x.StudentId,
                       x.Student.User.UserName,
                       x.Student.User.FullName,
                       x.PrevFinalScore,
                       x.ExamScore,
                       evaluation = x.Grade.Evaluation.Name
                   })
                   .ToListAsync();

            return data;
        }
    }
}
