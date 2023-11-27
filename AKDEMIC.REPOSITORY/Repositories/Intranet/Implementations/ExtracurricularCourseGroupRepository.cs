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
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtracurricularCourseGroupRepository : Repository<ExtracurricularCourseGroup>, IExtracurricularCourseGroupRepository
    {
        public ExtracurricularCourseGroupRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ExtracurricularCourseGroup, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Term.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ExtracurricularCourse.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Teacher.User.FullName;
                    break;
                default:
                    break;
            }

            var query = _context.ExtracurricularCourseGroups
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()) || x.ExtracurricularCourse.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    course = x.ExtracurricularCourse.Name,
                    courseId = x.ExtracurricularCourseId,
                    term = x.Term.Name,
                    termId = x.TermId,
                    code = x.Code,
                    teacher = x.Teacher.User.FullName,
                    teacherId = x.TeacherId
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public override async Task<ExtracurricularCourseGroup> Get(Guid id)
            => await _context.ExtracurricularCourseGroups
                .Include(x => x.ExtracurricularCourse)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<ExtracurricularCourseGroup>> GetAll(string teacherId = null)
        {
            var query = _context.ExtracurricularCourseGroups
                .Include(x => x.Teacher.User)
                .Include(x => x.ExtracurricularCourse)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            return await query.ToListAsync();
        }

        public async Task<ExtracurricularCourseGroup> GetByCode(string code)
            => await _context.ExtracurricularCourseGroups
                .Where(x => x.Code == code)
                .FirstOrDefaultAsync();

        public async Task<ExtracurricularCourseGroup> GetWithIncludes(Guid id)
        {
            return await _context.ExtracurricularCourseGroups.Include(x=>x.Teacher.User).Where(x => x.Id == id).FirstOrDefaultAsync();
            
        }
    }
}
