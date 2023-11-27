using AKDEMIC.ENTITIES.Models.LanguageCenter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class LanguageSectionSeed
    {
        public static LanguageSection[] Seed(AkdemicContext _context)
        {
            ENTITIES.Models.Generals.Teacher teacher = _context.Teachers.Include(x=>x.User).First(x=>x.User.UserName == "docente");

            List<LanguageSection> result = new List<LanguageSection>()
            {
                new LanguageSection{ Code = "G", TeacherId = teacher.UserId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),State = 1, LanguageLevelId = _context.LanguageLevels.Include(x=>x.LanguageCourse).First(x=>x.LanguageCourse.Name == "Ingles").Id,Vacancies = 10 },
                new LanguageSection{ Code = "F", TeacherId = teacher.UserId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),State = 1, LanguageLevelId = _context.LanguageLevels.Include(x=>x.LanguageCourse).First(x=>x.LanguageCourse.Name == "Frances").Id,Vacancies = 10 },
                new LanguageSection{ Code = "A", TeacherId = teacher.UserId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),State = 1, LanguageLevelId = _context.LanguageLevels.Include(x=>x.LanguageCourse).First(x=>x.LanguageCourse.Name == "Aleman").Id,Vacancies = 10 },
                new LanguageSection{ Code = "I", TeacherId = teacher.UserId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1),State = 1, LanguageLevelId = _context.LanguageLevels.Include(x=>x.LanguageCourse).First(x=>x.LanguageCourse.Name == "Italiano").Id,Vacancies = 10 }
            };
            return result.ToArray();
        }
    }
}
