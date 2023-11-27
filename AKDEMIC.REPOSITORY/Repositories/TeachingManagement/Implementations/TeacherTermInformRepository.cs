using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TeacherTermInformRepository : Repository<TeacherTermInform>, ITeacherTermInformRepository
    {
        public TeacherTermInformRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetTermInformsChart(int? type = null, ClaimsPrincipal user = null)
        {
            var teachers = _context.Teachers
                                    .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    teachers = teachers.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            var query = _context.TeacherTermInforms
                            .AsNoTracking();

            if (type != null)
            {
                query = query.Where(x => x.TermInform.Type == type);
            }

            query = query.Where(x => teachers.Any(y => y.UserId == x.TeacherId));

            int finalTerm = 0;
            int midTerm = 0;

            finalTerm = await query.Where(x => x.TermInform.Type == ConstantHelpers.TERM_INFORM.TYPE.ENDTERM).CountAsync();
            midTerm = await query.Where(x => x.TermInform.Type == ConstantHelpers.TERM_INFORM.TYPE.HALFTERM).CountAsync();

            var result = new
            {
                categories = new string[2] { ConstantHelpers.TERM_INFORM.TYPE.VALUES[ConstantHelpers.TERM_INFORM.TYPE.ENDTERM], ConstantHelpers.TERM_INFORM.TYPE.VALUES[ConstantHelpers.TERM_INFORM.TYPE.HALFTERM] },
                data = new int[2] { finalTerm, midTerm }
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermInformsDatatable(DataTablesStructs.SentParameters parameters, int? type = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name); break;
            }

            var query = _context.Teachers
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            var teacherInforms = _context.TeacherTermInforms
                            .AsQueryable();

            if (type != null)
            {
                teacherInforms = teacherInforms.Where(x => x.TermInform.Type == type);
            }

            query = query.Where(x => teacherInforms.Any(y => y.TeacherId == x.UserId));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    userName = x.User.UserName,
                    name = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    career = x.Career.Name,
                })
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        async Task<object> ITeacherTermInformRepository.GetChartReportDataByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId)
        {
            //var query = _context.TeacherTermInforms
            //    .Include(x => x.TermInform.Section)
            //    .Select(x => new
            //    {
            //        x.TermInform.TermId,
            //        x.TermInform.Section.CourseTermId,
            //        x.TermInform.Type,
            //        x.State
            //    })
            //    .AsQueryable();

            //if (termId.HasValue)
            //{
            //    query = query.Where(x => x.TermId == termId);
            //}

            //if (courseTermId.HasValue)
            //{
            //    query = query.Where(x => x.CourseTermId == courseTermId);
            //}

            //var result = await query.ToListAsync();

            //List<string> categories = new List<string>();
            //List<int> data = new List<int>();

            //if (state.HasValue)
            //{
            //    query = query.Where(x => x.State == state);

            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.MIDTERM]}");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.MIDTERM));

            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.FINALTERM]}");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.FINALTERM));
            //}
            //else
            //{
            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.MIDTERM]} - A tiempo");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.MIDTERM && x.State == ConstantHelpers.TERM_INFORM.STATES.EARLY));

            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.MIDTERM]} - Tarde");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.MIDTERM && x.State == ConstantHelpers.TERM_INFORM.STATES.LATE));


            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.FINALTERM]} - A tiempo");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.FINALTERM && x.State == ConstantHelpers.TERM_INFORM.STATES.EARLY));

            //    categories.Add($"{ConstantHelpers.TERM_INFORM.TYPES.VALUES[ConstantHelpers.TERM_INFORM.TYPES.FINALTERM]} - Tarde");
            //    data.Add(result.Count(x => x.Type == ConstantHelpers.TERM_INFORM.TYPES.FINALTERM && x.State == ConstantHelpers.TERM_INFORM.STATES.LATE));
            //}

            //return new { categories, data };
            return null;
        }

        async Task<object> ITeacherTermInformRepository.GetChartReportDatatableByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId)
        {
            //var query = _context.TeacherTermInforms
            //    .Include(x => x.TermInform.Section.CourseTerm.Course)
            //    .Include(x => x.Teacher)
            //    .AsQueryable();

            //if (termId.HasValue)
            //{
            //    query = query.Where(x => x.TermInform.TermId == termId);
            //}

            //if (courseTermId.HasValue)
            //{
            //    query = query.Where(x => x.TermInform.Section.CourseTermId == courseTermId);
            //}

            //if (state.HasValue)
            //{
            //    query = query.Where(x => x.State == state);
            //}

            //var result = await query.Select(x => new
            //{
            //    id = x.Id,
            //    typeId = x.TermInform.Type,
            //    stateId = x.State,
            //    course = x.TermInform.Section.CourseTerm.Course.FullName,
            //    section = x.TermInform.Section.Code,
            //    //type = ConstantHelpers.TERM_INFORM.TYPES.VALUES[x.TermInform.Type],
            //    //state = ConstantHelpers.TERM_INFORM.STATES.VALUES[x.State],
            //    teacher = x.Teacher.FullName,
            //    url = x.Url,
            //    uploadDate = x.CreatedAt.ToLocalDateTimeFormat()
            //})
            //.ToListAsync();

            //return result;
            return null;
        }
    }
}