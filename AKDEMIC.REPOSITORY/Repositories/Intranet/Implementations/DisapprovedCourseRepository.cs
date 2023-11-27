using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DisapprovedCourseRepository : Repository<DisapprovedCourse>, IDisapprovedCourseRepository
    {
        public DisapprovedCourseRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<int> LoadDisapprovedCoursesJob(string careerCode)
        {
            var newDisapprovedCourses = new List<DisapprovedCourse>();
            var career = await _context.Careers.FirstOrDefaultAsync(x => x.Code == careerCode);

            var oldDisapprovedCourses = await _context.DisapprovedCourses
                .Where(x => x.Student.CareerId == career.Id)
                .ToListAsync();
            _context.DisapprovedCourses.RemoveRange(oldDisapprovedCourses);

            var students = await _context.Students
                .Where(x => x.CareerId == career.Id)
                .ToListAsync();

            var studentsHash = students.Select(x => x.Id).ToHashSet();

            var academicHistories = await _context.AcademicHistories
                .Where(x => studentsHash.Contains(x.StudentId))
                .Include(x => x.Term)
                .ToListAsync();

            var disapprovedCourses = academicHistories
                .GroupBy(x => new { x.StudentId, x.CourseId })
                .Where(x => x.All(y => !y.Approved))
                .Select(x => new
                {
                    x.Key.StudentId,
                    x.Key.CourseId,
                    x.OrderByDescending(y => y.Term.Year).ThenBy(y => y.Term.Number).FirstOrDefault().TermId,
                    x.OrderByDescending(y => y.Term.Year).ThenBy(y => y.Term.Number).FirstOrDefault().Try
                })
                .ToList();

            foreach (var item in disapprovedCourses)
            {
                var disapprovedCourse = new DisapprovedCourse
                {
                    CourseId = item.CourseId,
                    StudentId = item.StudentId,
                    LastTry = (byte)item.Try,
                    TermId = item.TermId
                };

                newDisapprovedCourses.Add(disapprovedCourse);
            }

            await _context.DisapprovedCourses.AddRangeAsync(newDisapprovedCourses);
            await _context.SaveChangesAsync();
            return disapprovedCourses.Count;
        }

    }
}
