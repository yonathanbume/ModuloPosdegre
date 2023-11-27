using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseUnitRepository : Repository<CourseUnit>, ICourseUnitRepository
    {
        public CourseUnitRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetCourseUnitsSelect2ClientSide(Guid courseId, Guid termId)
        {
            var result = await _context.CourseUnits
                .Where(cu => cu.CourseSyllabus.CourseId == courseId && cu.CourseSyllabus.TermId == termId)
                .Select(cu => new
                {
                    text = cu.Name,
                    children = cu.UnitActivities
                    .OrderBy(y=>y.Week).ThenBy(y=>y.Order)
                        .Select(ua => new
                        {
                            id = ua.Id,
                            text = $"S{ua.Week} - {ua.Name}"
                        }).ToList()
                }).OrderBy(x => x.text).ToArrayAsync();


            return result;
        }
        public async Task<object> GetCourseUnitsSelect2ClientSide2(Guid courseId, Guid termId)
        {
            var result = await _context.CourseUnits
                .Where(cu => cu.CourseSyllabus.CourseId == courseId && cu.CourseSyllabus.TermId == termId)
                .Select(cu => new
                {
                    text = cu.Name,
                    id = cu.Id
                }).ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<CourseUnit>> GetAllBySyllabusId(Guid syllabusId) =>
            await _context.CourseUnits.Where(x => x.CourseSyllabusId == syllabusId).Include(x => x.UnitActivities)
                .Include(x => x.UnitResources).ToListAsync();
        public async Task<IEnumerable<CourseUnit>> GetAllBySyllabusId2(Guid syllabusId) =>
            await _context.CourseUnits.Where(x => x.CourseSyllabusId == syllabusId).ToListAsync();

        public async Task<IEnumerable<CourseUnit>> GetCourseUnitProgressBySectionIdAndSyllabusId(Guid syllabusId,
            Guid sectionId) =>
            await _context.CourseUnits.Include(x => x.UnitActivities).ThenInclude(x => x.Classes).ThenInclude(x => x.Section).OrderBy(x => x.WeekNumberStart).Where(x => x.CourseSyllabusId == syllabusId)
                .Select(x => new CourseUnit
                {
                    Name = x.Name,
                    WeekNumberStart = x.WeekNumberStart,
                    WeekNumberEnd = x.WeekNumberEnd,
                    UnitActivities = x.UnitActivities.Select(ua => new UnitActivity
                    {
                        Id = ua.Id,
                        Name = ua.Name,
                        Week = ua.Week,
                        Order = ua.Order,
                        Classes = ua.Classes.Where(c => c.SectionId == sectionId)
                            .Select(c => new Class
                            {
                                StartTime = c.StartTime,
                                Id = c.Id
                            }).ToList()
                    }).ToList(),
                    UnitResources = x.UnitResources.Select(ur => new UnitResource
                    {
                        Name = ur.Name,
                        Week = ur.Week
                    }).ToList()
                }).ToListAsync();

        public async Task<IEnumerable<CourseUnitModelA>> GetAllAsModelA(Guid? courseId = null, Guid? termId = null)
        {
            var query = _context.CourseUnits.AsQueryable();
            var teachersQuery = _context.TeacherSections.AsNoTracking();

            if (courseId.HasValue)
            {
                query = query.Where(x => x.CourseSyllabus.CourseId == courseId.Value);
                teachersQuery = teachersQuery.Where(x => x.Section.CourseTerm.CourseId == courseId);
            }

            if (termId.HasValue)
            {
                query = query.Where(x => x.CourseSyllabus.TermId == termId.Value);
                teachersQuery = teachersQuery.Where(x => x.Section.CourseTerm.TermId == termId);
            }

            var teachers = await teachersQuery
                .Select(x => new
                {
                    x.Section.CourseTerm.CourseId,
                    x.TeacherId
                })
                .ToListAsync();

            var dataDb = await query
                .OrderBy(x => x.Number)
                .Select(x => new CourseUnitModelA
                {
                    Id = x.Id,
                    Name = x.Name,
                    WeekNumberStart = x.WeekNumberStart,
                    WeekNumberEnd = x.WeekNumberEnd,
                    SylabusId = x.CourseSyllabusId,
                    Number = x.Number,
                    CourseId = x.CourseSyllabus.CourseId,
                    AcademicProgressPercentage = x.AcademicProgressPercentage,
                    UnitActivities = x.UnitActivities.Select(ua => new CourseUnitModelAUnitActivity
                    {
                        Id = ua.Id,
                        Name = ua.Name,
                        Week = ua.Week,
                        Order = ua.Order
                    }).ToList(),
                    UnitResources = x.UnitResources.Select(ur => new CourseUnitModelAUnitResourse
                    {
                        Id = ur.Id,
                        Name = ur.Name,
                        Week = ur.Week
                    }).ToList(),
                }).ToListAsync();

            var model = dataDb
                .Select(x => new CourseUnitModelA
                {
                    Id = x.Id,
                    Name = x.Name,
                    Number = x.Number,
                    WeekNumberStart = x.WeekNumberStart,
                    WeekNumberEnd = x.WeekNumberEnd,
                    AcademicProgressPercentage = x.AcademicProgressPercentage,
                    SylabusId = x.SylabusId,
                    UnitActivities = x.UnitActivities.Select(ua => new CourseUnitModelAUnitActivity
                    {
                        Id = ua.Id,
                        Name = $"{ua.Order}. {ua.Name}",
                        Week = ua.Week,
                        Order = ua.Order
                    }).ToList(),
                    UnitResources = x.UnitResources.Select(ur => new CourseUnitModelAUnitResourse
                    {
                        Id = ur.Id,
                        Name = ur.Name,
                        Week = ur.Week
                    }).ToList(),
                    Teachers = teachers.Where(y => y.CourseId == x.CourseId).Select(y => y.TeacherId).ToList()
                })
                .ToList();

            return model;
        }

        public Task<bool> AnyByCourseSyllabusId(Guid courseSyllabusId)
            => _context.CourseUnits.AnyAsync(x => x.CourseSyllabusId == courseSyllabusId);

        public async Task<CourseUnit> GetInDateRangeBySyllabus(int weekNumberStart, int weekNumberEnd, Guid syllabusId, Guid? id = null)
        {
            var anotherUnit = await _context.CourseUnits
                .Where(x => x.CourseSyllabusId == syllabusId && x.Id != id)
                .FirstOrDefaultAsync(x =>
                    x.WeekNumberStart >= weekNumberStart && x.WeekNumberStart <= weekNumberEnd ||
                    x.WeekNumberEnd >= weekNumberStart && x.WeekNumberEnd <= weekNumberEnd);

            return anotherUnit;
        }

        public async Task<object> GetAsModelB(Guid id)
        {
            var resultDB = await _context.CourseUnits
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    sylabusId = x.CourseSyllabusId,
                    name = x.Name,
                    weekNumberStart = x.WeekNumberStart,
                    weekNumberEnd = x.WeekNumberEnd,
                    x.AcademicProgressPercentage,
                    equivalentUnits = false
                }).FirstOrDefaultAsync();

            if (await _context.CourseUnits.Where(x => x.CourseSyllabusId == resultDB.sylabusId).AllAsync(x => x.AcademicProgressPercentage == 0))
            {
                var result = new
                {
                    resultDB.id,
                    resultDB.sylabusId,
                    resultDB.name,
                    resultDB.weekNumberEnd,
                    resultDB.weekNumberStart,
                    resultDB.AcademicProgressPercentage,
                    equivalentUnits = true
                };

                return result;
                    
            }

            return resultDB;
        }

        public async Task<IEnumerable<CourseUnitGrades>> GetCourseUnitGradesByStudentIdAndSectionId(Guid studentId, Guid sectionId)
        {
            try
            {
                var studentSectionId = await _context.StudentSections.Where(x => x.StudentId == studentId && x.SectionId == sectionId).Select(x => x.Id).FirstOrDefaultAsync();
                var section = await _context.Sections.FindAsync(sectionId);
                var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();

                var grades = await _context.Grades
                    .Where(x => x.StudentSectionId == studentSectionId)
                    .Include(x => x.Evaluation)
                    .ThenInclude(x => x.CourseUnit)
                    .ToListAsync();

                var evaluations = await _context.Evaluations
                    .Where(x => x.CourseTermId == section.CourseTermId)
                    .ToListAsync();

                var courseunits = await _context.CourseUnits.Where(x => x.CourseSyllabus.CourseId == courseTerm.CourseId && x.CourseSyllabus.TermId == courseTerm.TermId).ToListAsync();

                var averages = grades.Where(x=>x.Evaluation != null && x.Evaluation.CourseUnit != null)
                    .GroupBy(x => x.Evaluation.CourseUnit)
                    .Select(x => new CourseUnitGrades
                    {
                        Number = x.Key.Number,
                        Name = x.Key.Name,
                        CourseUnitId = x.Key.Id,
                        Average = (int)Math.Round(x.Sum(y => (y.Value * y.Evaluation.Percentage) / evaluations.Where(z => z.CourseUnitId == x.Key.Id).Sum(z => z.Percentage)),0,MidpointRounding.AwayFromZero)
                    })
                    .OrderBy(x=>x.Number)
                    .ToArray();

                var result = courseunits
                    .Select(x => new CourseUnitGrades
                    {
                        Number = x.Number,
                        Name = x.Name,
                        CourseUnitId = x.Id,
                        Average = averages.Where(y=>y.CourseUnitId == x.Id).Select(x=>x.Average).FirstOrDefault()
                    })
                    .OrderBy(x => x.Number)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetQuantityCourseUnits(Guid courseId, Guid termId)
            => await _context.CourseUnits.Include(x=>x.Evaluations).Where(x => x.CourseSyllabus.CourseId == courseId && x.CourseSyllabus.TermId == termId).CountAsync();

        public async Task<List<CourseUnit>> GetCourseUnits(Guid courseId, Guid termId)
           => await _context.CourseUnits.Include(x => x.Evaluations).Where(x => x.CourseSyllabus.CourseId == courseId && x.CourseSyllabus.TermId == termId).ToListAsync();

        public async Task DeleteUnitWithData(Guid unitId)
        {
            var unit = await _context.CourseUnits.FindAsync(unitId);

            var activities = await _context.UnitActivities.Where(x => x.CourseUnitId == unit.Id).ToListAsync();
            _context.UnitActivities.RemoveRange(activities);

            var resources = await _context.UnitResources.Where(x => x.CourseUnitId == unit.Id).ToListAsync();
            _context.UnitResources.RemoveRange(resources);

            var evaluations = await _context.Evaluations.Where(x => x.CourseUnitId == unit.Id).ToListAsync();
            _context.Evaluations.RemoveRange(evaluations);

            _context.CourseUnits.Remove(unit);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalAcademicProgressPercentage(Guid courseSyllabusId, Guid? ignoredId = null)
            => await _context.CourseUnits.Where(x => x.CourseSyllabusId == courseSyllabusId && x.Id != ignoredId).SumAsync(x => x.AcademicProgressPercentage);
    }
}