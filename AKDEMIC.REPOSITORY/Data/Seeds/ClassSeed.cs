using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ClassSeed
    {
        public static Class[] Seed(AkdemicContext context)
        {
            var classes = new List<Class>();
            var sections = context.Sections
                .Include(x => x.CourseTerm.Term)
                .Include(x => x.StudentSections)
                .Include(x => x.ClassSchedules)
                .Where(x => x.StudentSections.Any())
                .ToList();

            for (var i = 0; i < sections.Count; i++)
            {
                var section = sections[i];
                var classSchedules = section.ClassSchedules.ToList();
                var term = section.CourseTerm.Term;
                var weeks = Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);

                for (var j = 0; j < weeks; j++)
                {
                    var classNumber = 1;

                    for (var k = 0; k < classSchedules.Count; k++)
                    {
                        var classSchedule = classSchedules[k];
                        var @class = new Class()
                        {
                            ClassNumber = classNumber++,
                            WeekNumber = (j + 1),
                            StartTime = term.ClassStartDate.ToDefaultTimeZone().Date.AddDays(j * 7).AddDays(classSchedule.WeekDay).Add(classSchedule.StartTime.ToLocalTimeSpanUtc()).ToUniversalTime(),
                            EndTime = term.ClassStartDate.ToDefaultTimeZone().Date.AddDays(j * 7).AddDays(classSchedule.WeekDay).Add(classSchedule.EndTime.ToLocalTimeSpanUtc()).ToUniversalTime(),
                            ClassroomId = classSchedule.ClassroomId,
                            ClassScheduleId = classSchedule.Id,
                            SectionId = classSchedule.SectionId
                        };

                        classes.Add(@class);
                    }
                }
            }

            var c = classes.FirstOrDefault(x => x.EndTime == DateTime.Parse("2018-09-06 02:00:00.0000000") &&
                                                     x.StartTime == DateTime.Parse("2018-09-06 00:00:00.0000000"));
            if (c != null)
                c.UnitActivityId = context.UnitActivities.FirstOrDefault(x =>
                    x.Week == 1 && x.Order == 1 && x.CourseUnit.CourseSyllabus.Course.Name == "Programación I" &&
                    x.CourseUnit.CourseSyllabus.Term.Name == "2018-2")?.Id;

            var result = classes;
            return result.ToArray();
        }
    }
}
