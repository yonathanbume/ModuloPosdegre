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
    public class DeferredExamStudentRepository : Repository<DeferredExamStudent>, IDeferredExamStudentRepository
    {
        public DeferredExamStudentRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid deferredExamId, string search)
        {
            var query = _context.DeferredExamStudents
                .Where(x=>x.DeferredExamId == deferredExamId)
                .OrderBy(x=>x.Student.User.FullName)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    username  = x.Student.User.UserName,
                    fullName = x.Student.User.FullName,
                    termStatus = x.DeferredExam.Section.CourseTerm.Term.Status,
                    grade = !x.Grade.HasValue ? "Sin calificar" : x.Grade.ToString(),
                    status = x.Status
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<DeferredExamStudent>> GetAll(Guid deferredExamId)
            => await _context.DeferredExamStudents.Where(x => x.DeferredExamId == deferredExamId).ToListAsync();
    }
}
