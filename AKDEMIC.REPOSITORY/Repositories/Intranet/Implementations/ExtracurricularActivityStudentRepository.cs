using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
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
    public class ExtracurricularActivityStudentRepository : Repository<ExtracurricularActivityStudent>, IExtracurricularActivityStudentRepository
    {
        public ExtracurricularActivityStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ExtracurricularActivityStudent>> GetAll()
            => await _context.ExtracurricularActivityStudents
                .Include(x => x.Student.User)
                .Include(x => x.ExtracurricularActivity)
                .ToListAsync();

        public async Task<List<ExtracurricularActivityStudent>> GetAllByStudent(Guid studentId)
            => await _context.ExtracurricularActivityStudents.Where(x => x.StudentId == studentId)
            .Include(x => x.ExtracurricularActivity.ExtracurricularArea)
            .Include(x => x.ExtracurricularActivity.Term)
            .ToListAsync();

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string search)
        {
            var query = _context.ExtracurricularActivityStudents
                .OrderByDescending(x => x.RegisterDate)
                .AsNoTracking();

            if (termId.HasValue)
                query = query.Where(x => x.ExtracurricularActivity.TermId == termId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.ExtracurricularActivity.Credits,
                    ActivityCode = x.ExtracurricularActivity.Code,
                    Activity = x.ExtracurricularActivity.Name,
                    Name = x.Student.User.FullName
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
    }
}
