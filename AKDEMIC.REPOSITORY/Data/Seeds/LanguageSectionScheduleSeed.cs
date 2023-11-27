using AKDEMIC.ENTITIES.Models.LanguageCenter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class LanguageSectionScheduleSeed
    {
        public static LanguageSectionSchedule[] Seed(AkdemicContext _context)
        {
            List<LanguageSection> sections = _context.LanguageSections.ToList();
            List<LanguageSectionSchedule> result = new List<LanguageSectionSchedule>()
            {
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("07:00"),EndTime = TimeSpan.Parse("09:00"),WeekDay = 0,LanguageSectionId = sections.ElementAt(0).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("07:00"),EndTime = TimeSpan.Parse("09:00"),WeekDay = 2,LanguageSectionId = sections.ElementAt(0).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("07:00"),EndTime = TimeSpan.Parse("09:00"),WeekDay = 4,LanguageSectionId = sections.ElementAt(0).Id },

                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("10:00"),EndTime = TimeSpan.Parse("12:00"),WeekDay = 0,LanguageSectionId = sections.ElementAt(1).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("10:00"),EndTime = TimeSpan.Parse("12:00"),WeekDay = 2,LanguageSectionId = sections.ElementAt(1).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("10:00"),EndTime = TimeSpan.Parse("12:00"),WeekDay = 4,LanguageSectionId = sections.ElementAt(1).Id },

                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("14:00"),EndTime = TimeSpan.Parse("17:00"),WeekDay = 5,LanguageSectionId = sections.ElementAt(2).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("14:00"),EndTime = TimeSpan.Parse("17:00"),WeekDay = 6,LanguageSectionId = sections.ElementAt(2).Id },

                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("08:00"),EndTime = TimeSpan.Parse("11:00"),WeekDay = 1,LanguageSectionId = sections.ElementAt(3).Id },
                new LanguageSectionSchedule { StartTime = TimeSpan.Parse("08:00"),EndTime = TimeSpan.Parse("11:00"),WeekDay = 3,LanguageSectionId = sections.ElementAt(3).Id },
            };
            return result.ToArray();
        }
    }
}
