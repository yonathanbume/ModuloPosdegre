using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class TutorialRepository : Repository<Tutorial>, ITutorialRepository
    {
        public TutorialRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetProgramTutorials()
        {
            var result = await _context.Tutorials.Select(x => new
            {
                teacher = x.Teacher.FullName,
                classroom = x.Classroom.Description,
                section = x.Section.Code,
                date = x.StartTime.ToLocalDateFormat(),
                StartTime = x.StartTime.ToLocalDateFormat(),
                endtime = x.EndTime.ToLocalDateFormat()
            }).ToListAsync();

            return result;
        }

        public async Task<object> GetDoneTutorialsStartAndEndDate(DateTime startDate, DateTime endDate)
        {
            var result = await _context.Tutorials.Where(x => x.IsDictated == true && startDate <= x.StartTime.Date && x.StartTime.Date <= endDate).Select(x => new
            {
                id = x.Id,
                teacher = x.Teacher.FullName,
                classroom = x.Classroom.Description,
                section = x.Section.Code,
                date = x.StartTime.ToLocalDateFormat(),
                StartTime = x.StartTime.ToLocalTimeFormat(),
                endtime = x.EndTime.ToLocalTimeFormat()
            }).ToListAsync();

            return result;
        }
        public async Task<object> GetTutorials(DateTime startDate, DateTime endDate, string userId)
        {
            var result = await _context.Tutorials
                .Where(t => t.TeacherId == userId)
                .Where(t => t.StartTime >= startDate && t.EndTime <= endDate)
                .Select(t => new
                {
                    id = t.Id,
                    title = $"{t.Section.CourseTerm.Course.Code}-{t.Section.CourseTerm.Course.Name} ({t.Section.Code})",
                    description = t.Classroom.Description,
                    allDay = false,
                    start = t.StartTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    end = t.EndTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    editable = t.StartTime > DateTime.UtcNow
             }).ToListAsync();

            return result;
        }
        public async Task<object> GetTutorialByIdAndUserId(Guid id, string userId)
        {
            var result = await _context.Tutorials
             .Where(x => x.Id.Equals(id))
             .Where(t => t.TeacherId == userId)
             .Select(x => new
             {
                 id = x.Id,
                 courseId = x.Section.CourseTerm.CourseId,
                 sectionId = x.SectionId,
                 classroomId = x.ClassroomId,
                 date = x.StartTime.ToLocalDateFormat(),
                 start = x.StartTime.ToLocalTimeFormat(),
                 end = x.EndTime.ToLocalTimeFormat(),
                 students = x.TutorialStudents.Select(s => s.StudentId).ToArray(),
                 editable = x.StartTime > DateTime.UtcNow
             }).FirstOrDefaultAsync();

            return result;
        }
        public async Task<Tutorial> AddAsync()
            => (await _context.Tutorials.AddAsync(new Tutorial())).Entity;

        public async Task<Tutorial> GetTutorialByIdAndUserIdEdit(Guid id, string userId)
            => await _context.Tutorials.FirstOrDefaultAsync(x =>
                x.Id == id && x.TeacherId == userId);

        protected virtual bool DateTimeConflict(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA < endB && startB < endA;
        public async Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime starTime, DateTime endTime)
            => await _context.Tutorials.AnyAsync(c => c.Id != id && c.ClassroomId == classroomId && DateTimeConflict(c.StartTime, c.EndTime, starTime, endTime));
        public async Task<Tutorial> GetConflictedTutorial(Guid id, string teacherId, DateTime starTime, DateTime endTime)
            => await _context.Tutorials.Include(c => c.Section.CourseTerm.Course)
                .FirstOrDefaultAsync(c =>
                    c.Id != id && c.TeacherId == teacherId &&
                    DateTimeConflict(c.StartTime, c.EndTime, starTime, endTime));

        public async Task<Tutorial> GetWithDataById(Guid id)
            => await _context.Tutorials.Include(x => x.Section.CourseTerm.Term).Include(x => x.TutorialStudents).FirstOrDefaultAsync(x => x.Id.Equals(id));
        public void Remove(Tutorial tutorial)
            => _context.Tutorials.Remove(tutorial);
        public async Task<object> GetTutorialsByDatesAndTeacherId(DateTime startDate, DateTime endDate, string teacherId)
        {
            var result = await _context.Tutorials.Where(x => startDate <= x.StartTime.Date && x.StartTime.Date <= endDate && x.TeacherId == teacherId).Select(x => new
            {
                id = x.Id,
                teacher = x.Teacher.FullName,
                classroom = x.Classroom.Description,
                section = x.Section.Code,
                date = x.StartTime.ToLocalDateFormat(),
                StartTime = x.StartTime.ToLocalTimeFormat(),
                endtime = x.EndTime.ToLocalTimeFormat()
            }).ToListAsync();

            return result;
        }
    }
}
