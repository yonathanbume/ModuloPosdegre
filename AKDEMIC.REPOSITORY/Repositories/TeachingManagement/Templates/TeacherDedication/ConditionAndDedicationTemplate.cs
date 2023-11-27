using System;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication
{
    public class ConditionAndDedicationTemplate
    {   
        public string id { get;  set; }
        public string names { get;  set; }
        public string paternalSurname { get;  set; }
        public string maternalSurname { get;  set; }
        public string userName { get;  set; }
        public string career { get;  set; }
        public string fullname { get;  set; }
        public string condition { get;  set; }
        public string dedication { get;  set; }
        public string email { get;  set; }
        public string phoneNumber { get;  set; }
        public DateTime? createdAt { get;  set; }
        public string category { get;  set; }
        public string AcademicDepartment { get; set; }
        public string DNI { get; set; }
    }
}
