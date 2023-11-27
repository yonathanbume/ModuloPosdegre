using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TeacherScheduleSeed
    {
        public static TeacherSchedule[] Seed(AkdemicContext context)
        {
            if (!context.ClassSchedules.Any())
            {
                return new TeacherSchedule[] { };
            }

            var classSchedules = context.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Section)
                .ThenInclude(x => x.CourseTerm)
                .ThenInclude(x => x.Term)
                .ToList();
            var teachers = context.Teachers
                .Include(x => x.User)
                .ToList();

            var result = new List<TeacherSchedule>()
            {
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H22" & x.Section.Code == "HU-01" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-01" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C56" & x.Section.Code == "IN-02" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "E33" & x.Section.Code == "IN-21" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C23" & x.Section.Code == "SW-35" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H21" & x.Section.Code == "HU-14" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C41" & x.Section.Code == "IN-04" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C42" & x.Section.Code == "CC-02" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-51" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C35" & x.Section.Code == "HU-21" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C48" & x.Section.Code == "IN-22" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B22" & x.Section.Code == "IN-23" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-01" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B33" & x.Section.Code == "HU-12" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B10" & x.Section.Code == "IN-05" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-03" & x.Section.CourseTerm.Term.Name == "2018-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId },

                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H22" & x.Section.Code == "HU-01" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-01" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C56" & x.Section.Code == "IN-02" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "E33" & x.Section.Code == "IN-21" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C23" & x.Section.Code == "SW-35" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H21" & x.Section.Code == "HU-14" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C41" & x.Section.Code == "IN-04" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C42" & x.Section.Code == "CC-02" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-51" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C35" & x.Section.Code == "HU-21" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C48" & x.Section.Code == "IN-22" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B22" & x.Section.Code == "IN-23" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-01" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B33" & x.Section.Code == "HU-12" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B10" & x.Section.Code == "IN-05" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-03" & x.Section.CourseTerm.Term.Name == "2018-1").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId },

                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H22" & x.Section.Code == "HU-01" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-01" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786523").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C56" & x.Section.Code == "IN-02" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786524").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "E33" & x.Section.Code == "IN-21" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C23" & x.Section.Code == "SW-35" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786525").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "H21" & x.Section.Code == "HU-14" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C41" & x.Section.Code == "IN-04" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786526").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C42" & x.Section.Code == "CC-02" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-51" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786527").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C35" & x.Section.Code == "HU-21" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C48" & x.Section.Code == "IN-22" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786528").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B22" & x.Section.Code == "IN-23" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B25" & x.Section.Code == "SW-01" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786529").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B33" & x.Section.Code == "HU-12" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B10" & x.Section.Code == "IN-05" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786530").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "C45" & x.Section.Code == "HU-02" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId },
                new TeacherSchedule { ClassScheduleId = classSchedules.FirstOrDefault(x => x.Classroom.Description == "B32" & x.Section.Code == "IN-03" & x.Section.CourseTerm.Term.Name == "2017-2").Id, TeacherId = teachers.FirstOrDefault(x => x.User.Dni == "79786531").UserId }
            };

            return result.ToArray();
        }
    }
}
