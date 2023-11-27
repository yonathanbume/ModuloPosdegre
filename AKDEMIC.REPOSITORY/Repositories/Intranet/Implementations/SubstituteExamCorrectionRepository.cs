using AKDEMIC.CORE.Extensions;
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
    public class SubstituteExamCorrectionRepository : Repository<SubstituteExamCorrection>, ISubstituteExamCorrectionRepository
    {
        public SubstituteExamCorrectionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<SubstituteExamCorrection>> GetAll(string teacherId = null, Guid? termId = null)
        {
            var query = _context.SubstituteExamCorrections
                .Include(x => x.SubstituteExam.Student.User)
                .Include(x => x.SubstituteExam.Section.CourseTerm.Course)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (termId.HasValue)
                query = query.Where(x => x.SubstituteExam.Section.CourseTerm.TermId == termId.Value);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
        {
            Expression<Func<SubstituteExamCorrection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.SubstituteExam.Section.CourseTerm.Course.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.SubstituteExam.Section.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.SubstituteExam.Student.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.SubstituteExam.Student.User.FullName;
                    break;
                case "5":
                    //orderByPredicate = (x) => x.NewGrade;
                    break;
                default:
                    break;
            }

            var query = _context.SubstituteExamCorrections.Where(x => !x.ToPay)
                      .AsQueryable();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (termId.HasValue)
                query = query.Where(x => x.SubstituteExam.Section.CourseTerm.TermId == termId.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.SubstituteExam.Student.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.SubstituteExam.Student.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.SubstituteExam.Student.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.SubstituteExam.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.SubstituteExam.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.SubstituteExam.Section.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.SubstituteExam.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()));

            }

            if (state.HasValue) query = query.Where(x => x.State == state);

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.SubstituteExam.Student.User.UserName,
                    student = x.SubstituteExam.Student.User.FullName,
                    course = x.SubstituteExam.Section.CourseTerm.Course.Name,
                    section = x.SubstituteExam.Section.Code,
                    grade = x.NewGrade,
                    state = x.State,
                    observations = x.Observations,
                    file = x.FilePath,
                    prevGrade = x.OldGrade,
                    teacher = $"{x.Teacher.UserName} - {x.Teacher.FullName}",
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

        public async Task<SubstituteExamCorrection> GetByTeacherStudent(string teacherId, Guid studentId)
            => await _context.SubstituteExamCorrections.Where(x => x.TeacherId == teacherId && x.SubstituteExam.Student.Id == studentId).FirstOrDefaultAsync();
    }
}
