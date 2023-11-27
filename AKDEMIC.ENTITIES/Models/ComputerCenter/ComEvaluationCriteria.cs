using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComEvaluationCriteria
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; } 
        public Guid ComCourseId { get; set; }
        public ComCourse ComCourse { get; set; }


        public ICollection<ComGrades> Grades { get; set; }
    }
}