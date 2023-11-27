using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ClassStudentSeed
    {
        public static ClassStudent[] Seed(AkdemicContext context)
        {
            var classStudents = new List<ClassStudent>();
            var classes = context.Classes
                .Include(x => x.ClassSchedule.Section.StudentSections)
                .ToList();
            var student = context.Students
                .Include(x => x.User)
                .First(x => x.User.UserName == "alumno");

            for (var i = 0; i < classes.Count; i++)
            {
                var @class = classes[i];
                var studentSections = @class.ClassSchedule.Section.StudentSections.ToList();

                for (var j = 0; j < studentSections.Count; j++)
                {
                    var studentSection = studentSections[j];
                    var classStudent = new ClassStudent()
                    {
                        ClassId = @class.Id,
                        StudentId = studentSection.StudentId,
                        IsAbsent = false
                    };

                    classStudents.Add(classStudent);
                }
            }

            classStudents.Where(x => x.StudentId == student.Id).ElementAt(0).IsAbsent = true;
            classStudents.Where(x => x.StudentId == student.Id).ElementAt(5).IsAbsent = true;

            var result = classStudents.ToList();

            return result.ToArray();
        }
    }
}
