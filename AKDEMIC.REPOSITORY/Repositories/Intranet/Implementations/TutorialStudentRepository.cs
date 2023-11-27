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
    public class TutorialStudentRepository : Repository<TutorialStudent>, ITutorialStudentRepository
    {
        public TutorialStudentRepository(AkdemicContext context) : base(context) { }
        public async Task<object> GetStudentTutorialDone(Guid eid)
        {
            var result = await _context.TutorialStudents.Where(x => x.TutorialId == eid).Select(x => new
            {
                fullname = x.Student.User.FullName,
                code = x.Student.User.UserName,
                career = x.Student.Career.Name,
                email = x.Student.User.Email,
                absent = x.Absent
            }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetStudentsByTutorialIdSelect2ClientSide(Guid tutorialId)
        {
            var result = await _context.TutorialStudents
                .Where(x => x.TutorialId == tutorialId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.StudentId,
                    Text = x.Student.User.FullName
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetChartReport(Guid eid)
        {
            var result = await _context.TutorialStudents
                .Where(x => x.TutorialId == eid)
                .GroupBy(x => new { x.Absent })
                .Select(x => new
                {
                    assist = "Ausentes",
                    assistants = x.Count(c => c.Absent == true),
                    absent = "Asistentes",
                    missing = x.Count(c => c.Absent == false)

                }).ToArrayAsync();

            return result;
        }
        public async Task<object> GetStudents(Guid id, string userId)
        {
            var result = await _context.TutorialStudents
            .Where(x => x.TutorialId == id && x.Tutorial.TeacherId == userId)
            .Select(x => new
            {
                code = x.Student.User.UserName,
                name = x.Student.User.FullName
            }).ToListAsync();

            return result;
        }

        public async Task<object> GetTutorialStudentByTutorialId(Guid eid)
        {
            var result = await _context.TutorialStudents.Where(x => x.TutorialId == eid).Select(x => new
            {
                id = x.StudentId,
                name = x.Student.User.FullName,
                email = x.Student.User.Email,
                assistance = x.Absent == true ? "checked" : ""
            }).ToListAsync();

            return result;
        }

        public async Task<TutorialStudent> GetByTutorialIdAndStudentId(Guid tutorialId, Guid studentId)
            => await _context.TutorialStudents.Where(x => x.TutorialId == tutorialId && x.StudentId == studentId).FirstOrDefaultAsync();
    }
}
