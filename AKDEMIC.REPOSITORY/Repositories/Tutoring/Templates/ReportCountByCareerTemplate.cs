using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates
{
    public class ReportCountByCareerTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Tutors { get; set; }
        public int DataTutors { get; set; }
        public int TutoringStudents { get; set; }
        public int DataTutoringStudents { get; set; }
        public int SingleSessions { get; set; }
        public int GroupSessions1 { get; set; }
        public int GroupSessions2 { get; set; }
        public int GroupSessions3 { get; set; }
        public int GroupSessions4 { get; set; }
        public int GroupSessions5 { get; set; }
        public int GroupSessions6 { get; set; }
        public int GroupSessions7 { get; set; }
        public int GroupSessions8 { get; set; }
        public int GroupSessions9 { get; set; }
        public int GroupSessions10 { get; set; }
        public int GroupSessions11 { get; set; }
        public int GroupSessions12 { get; set; }
        public int GroupSessions13 { get; set; }
        public int Referred { get; set; }
        public int Attended { get; set; }
    }
}
