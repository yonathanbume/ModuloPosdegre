using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class ReportTeacherClassTemplate : IEquatable<ReportTeacherClassTemplate>
    {
        public string career { get; set; }
        public string teacher { get; set; }
        public string course { get; set; }
        public string section { get; set; }
        public int scheduledClasses { get; set; }
        public int dictatedClasses { get; set; }
        public int reScheduledClasses { get; set; }
        public string classroom { get; set; }
        public string schedule { get; set; }
        public int students { get; set; }
        public string subject { get; set; }
        public string cycle { get; set; }
        public byte academicyear { get; set; }
        public DateTime start { get; set; }
        public bool IsDictated { get; set; }
        public string DictatedDate { get; set; }
        public string VirtualClass { get; set; }

        public bool Equals(ReportTeacherClassTemplate other)
        {
            if (career == other.career &&
                teacher == other.teacher &&
                course == other.course &&
                section == other.section &&
                scheduledClasses == other.scheduledClasses &&
                dictatedClasses == other.dictatedClasses &&
                reScheduledClasses == other.reScheduledClasses)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashcareer = career == null ? 0 : career.GetHashCode();
            int hashteacher = teacher == null ? 0 : teacher.GetHashCode();
            int hashcourse = course == null ? 0 : course.GetHashCode();
            int hashsection = section == null ? 0 : section.GetHashCode();
            int hashscheduledclass = scheduledClasses == 0 ? 0 : scheduledClasses.GetHashCode();
            int hashdictatedclass = dictatedClasses == 0 ? 0 : dictatedClasses.GetHashCode();
            int hashrescheduledclass = reScheduledClasses == 0 ? 0 : reScheduledClasses.GetHashCode();

            return hashcareer ^ hashteacher ^ hashcourse ^ hashsection ^ hashscheduledclass ^ hashdictatedclass ^ hashrescheduledclass;
        }
    }
}
