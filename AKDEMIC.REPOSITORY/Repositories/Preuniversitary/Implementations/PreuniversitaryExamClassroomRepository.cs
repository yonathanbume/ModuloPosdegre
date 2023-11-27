using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryExamClassroomRepository : Repository<PreuniversitaryExamClassroom>, IPreuniversitaryExamClassroomRepository
    {
        public PreuniversitaryExamClassroomRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyClassroomByExam(Guid classroomId, Guid preuniversitaryExamId, Guid? ignoredId = null)
            => await _context.PreuniversitaryExamClassrooms.AnyAsync(y=>y.ClassroomId == classroomId && y.PreuniversitaryExamId == preuniversitaryExamId && y.Id != ignoredId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
        {
            var query = _context.PreuniversitaryExamClassrooms
                .Where(x=>x.PreuniversitaryExamId == preuniversitaryExamId)
                .OrderByDescending(x=>x.CreatedAt)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      Classroom = x.Classroom.Description,
                      x.ClassroomId,
                      x.Vacancies
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
