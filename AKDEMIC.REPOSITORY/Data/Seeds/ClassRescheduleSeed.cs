using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ClassRescheduleSeed
    {
        public static ClassReschedule[] Seed(AkdemicContext context)
        {
            var classes = context.Classes.ToList();
            var teacherSections = context.TeacherSections
                .Include(x => x.Section.Classes)
                .ToList();

            var result = new List<ClassReschedule>()
            {
                new ClassReschedule { ClassId = classes[0].Id, EndDateTime = classes[0].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[0].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[0].SectionId && x.Section.Classes.Contains(classes[0])).TeacherId },
                new ClassReschedule { ClassId = classes[1].Id, EndDateTime = classes[1].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[1].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[1].SectionId && x.Section.Classes.Contains(classes[1])).TeacherId },
                new ClassReschedule { ClassId = classes[2].Id, EndDateTime = classes[2].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[2].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[2].SectionId && x.Section.Classes.Contains(classes[2])).TeacherId },
                new ClassReschedule { ClassId = classes[3].Id, EndDateTime = classes[3].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[3].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[3].SectionId && x.Section.Classes.Contains(classes[3])).TeacherId },
                new ClassReschedule { ClassId = classes[4].Id, EndDateTime = classes[4].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[4].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[4].SectionId && x.Section.Classes.Contains(classes[4])).TeacherId },
                new ClassReschedule { ClassId = classes[5].Id, EndDateTime = classes[5].EndTime.AddDays(7), Justification = "No puedo asistir.", StartDateTime = classes[5].StartTime.AddDays(7), UserId = teacherSections.First(x => x.SectionId == classes[5].SectionId && x.Section.Classes.Contains(classes[5])).TeacherId }
            };

            return result.ToArray();
        }
    }
}
