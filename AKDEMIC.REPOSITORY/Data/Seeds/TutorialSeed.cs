using System;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TutorialSeed
    {
        public static Tutorial[] Seed(AkdemicContext context)
        {
            var term = context.Terms.Where(x => x.Status == 1).ToArray();
            var user = context.Users.Where(x => x.UserName == "docente").ToArray();            
            var sections = context.Sections.Where(x => x.TeacherSections.Any(ts => ts.Teacher.User == user[0]) && x.CourseTerm.TermId == term[0].Id).ToArray();
            var classrooms = context.Classrooms.ToArray();
            var result = new []
            {
                new Tutorial{ IsDictated = true, TeacherId = user[0].Id, SectionId = sections[0].Id , ClassroomId = classrooms[0].Id , StartTime = DateTime.ParseExact("22/09/2018 17:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime() , EndTime = DateTime.ParseExact("22/09/2018 19:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime()},
                new Tutorial{ IsDictated = false, TeacherId = user[0].Id , SectionId = sections[1].Id , ClassroomId = classrooms[0].Id , StartTime = DateTime.ParseExact("24/09/2018 17:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime() , EndTime = DateTime.ParseExact("24/09/2018 19:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime()}
                                                                                                                                                                      
            };
            return result;
            
        }


    }
}



