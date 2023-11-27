using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class UnitActivityRepository : Repository<UnitActivity>, IUnitActivityRepository
    {
        public UnitActivityRepository(AkdemicContext context) : base(context) { }

        async Task<IEnumerable<UnitActivity>> IUnitActivityRepository.GetAllByCourseUnitAsync(Guid courseUnitId)
        {
            var units = await _context.UnitActivities
                .Where(x => x.CourseUnitId == courseUnitId)
                .ToListAsync();

            return units;
        }

        async Task<object> IUnitActivityRepository.GetAsModelA(Guid id)
        {
            var result = await _context.UnitActivities
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    unitId = x.CourseUnitId,
                    name = x.Name,
                    week = x.Week,
                    order = x.Order
                }).FirstOrDefaultAsync();

            return result;
        }

        async Task<object> IUnitActivityRepository.GetUnitActivitiesByCourseTermIdAndSectionId(Guid courseTermId, Guid sectionId)
        {
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == courseTermId).Select(x => new { x.CourseId, x.TermId }).FirstOrDefaultAsync();

            var classes = await _context.Classes
                .Where(x => x.SectionId == sectionId)
                .Select(x => new
                {
                    x.UnitActivityId,
                    x.IsDictated,
                    x.DictatedDate,
                    ClassStudents = x.ClassStudents.ToList()
                })
                .ToListAsync();

            var resultDB = await _context.UnitActivities
                .Include(ua => ua.CourseUnit)
                .Where(ua => ua.CourseUnit.CourseSyllabus.CourseId == courseTerm.CourseId && ua.CourseUnit.CourseSyllabus.TermId == courseTerm.TermId)
                .Select(ua => new
                {
                    id = ua.Id,
                    name = ua.CourseUnit.Name,
                    name2 = ua.Name,
                }).ToListAsync();

            var result = resultDB
                .Select(ua => new
                {
                    ua.id,
                    topic = $"{ua.name} - {ua.name2}",
                    isDictated = classes.Any(x => x.UnitActivityId == ua.id),
                    date = classes.Any(x => x.UnitActivityId == ua.id) ? (classes.FirstOrDefault(x => x.UnitActivityId == ua.id).DictatedDate.HasValue ? classes.FirstOrDefault(x => x.UnitActivityId == ua.id).DictatedDate.Value.ToLocalTime().ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture) : "---") : "---",
                    attendancePercentage = classes.Any(x => x.UnitActivityId == ua.id) ? $"{Math.Round(classes.FirstOrDefault(x => x.UnitActivityId == ua.id).ClassStudents.Where(y=>!y.IsAbsent).Count() * 100M / classes.FirstOrDefault(x => x.UnitActivityId == ua.id).ClassStudents.Count(),2,MidpointRounding.AwayFromZero)}%" : "-",
                    assists = classes.Any(x => x.UnitActivityId == ua.id) ? $"{classes.FirstOrDefault(x => x.UnitActivityId == ua.id).ClassStudents.Where(y => !y.IsAbsent).Count()}" : "-",
                }).ToList();

            return result;
        }
    }
}