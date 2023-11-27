using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher
{
    public class TeacherPlusAssitance
    {
        public string TeacherId { get; set; }
        public int Ind { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string UserName { get; set; }
        public byte Status { get; set; }
        public bool IsLate { get; set; }
        public bool IsAbsent { get; set; }
        public DateTime Time { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class TeacherPlusMonthlyAssitanceItemList
    {
        public string TeacherId { get; set; }
        public string Career { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }        
        public List<int> Months { get; set; }
    }
    public class TeacherPlusMonthlyAssitance
    {
        public List<TeacherPlusMonthlyAssitanceItemList> List{ get; set; }
        public List<WorkingDay> Assist { get; set; }
    }
}