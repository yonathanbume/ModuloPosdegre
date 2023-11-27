using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryExamTeacherRepository : Repository<PreuniversitaryExamTeacher>, IPreuniversitaryExamTeacherRepository
    {
        public PreuniversitaryExamTeacherRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyUserByExam(Guid preuniversitaryExamId, string userId)
            => await _context.PreuniversitaryExamTeachers.AnyAsync(x => x.PreuniversitaryExamId == preuniversitaryExamId && x.UserId == userId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
        {
            var query = _context.PreuniversitaryExamTeachers
                .Where(x => x.PreuniversitaryExamId == preuniversitaryExamId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      Classroom = x.PreuniversitaryExamClassroomId.HasValue ? x.PreuniversitaryExamClassroom.Classroom.Description : "-",
                      x.User.UserName,
                      x.User.FullName,
                      x.UserId
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

        public async Task AssignTeachersRandomly(Guid preuniversitaryExamId)
        {
            var classrooms = await _context.PreuniversitaryExamClassrooms.Where(x => x.PreuniversitaryExamId == preuniversitaryExamId).ToListAsync();
            var teachers = await _context.PreuniversitaryExamTeachers.Where(x => x.PreuniversitaryExamId == preuniversitaryExamId).ToListAsync();

            var index = 0;

            foreach (var teacher in teachers)
            {
                teacher.PreuniversitaryExamClassroomId = classrooms[index].Id;
                index++;
                if (index >= classrooms.Count) index = 0;
            }

            await _context.SaveChangesAsync();
        }
    }
}
