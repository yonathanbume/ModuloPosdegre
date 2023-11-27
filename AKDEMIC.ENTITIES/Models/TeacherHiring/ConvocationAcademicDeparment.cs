using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationAcademicDeparment
    {
        public Guid Id { get; set; }
        public Guid AcademicDepartmentId { get; set; }
        public Guid ConvocationId { get; set; }
        public AcademicDepartment AcademicDepartment { get; set; }
        public Convocation Convocation { get; set; }
        public int Vacancies { get; set; }
    }
}
