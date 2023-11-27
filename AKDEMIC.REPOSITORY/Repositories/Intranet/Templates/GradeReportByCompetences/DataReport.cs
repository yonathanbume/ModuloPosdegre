using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences
{
    public class DataReport
    {
        public string CompetenceName { get; set; }
        public decimal FinalResult { get; set; }
        public List<RowChild> RowChilds { get; set; }
    }

    public class DataReport2
    {
        public string CompetenceName { get; set; }
        public Guid CompetenceId { get; set; }
        public List<RangeLevel> RangeLevels { get; set; }
        public List<string> CompetencesNames { get; set; }
    }

    public class DataReport3
    {
        public string CompetenceName { get; set; }
        public string StudentFullName { get; set; }
        public List<RowChild3> RowChilds { get; set; }
    }



    public class RowChild
    {
        public string CourseName { get; set; }
        public string CourseSection { get; set; }
        public decimal Average { get; set; }
        public decimal Credits { get; set; }
        public Guid CourseId { get; set; }
        public Guid SectionId { get; set; }
    }

    public class RowChild2 {
        public string CompetenceName { get; set; }
        public string StudentFullName { get; set; }
        public string CourseName { get; set; }
        public Guid StudentId { get; set; }
        public decimal Average { get; set; }
        public decimal Credits { get; set; }
        public string UserName { get; set; }
        public Guid CourseId { get; set; }
        public decimal Grade { get; set; }
    }

    public class RowChild3
    {
        public string CompetenceName { get; set; }
        public string StudentFullName { get; set; }
        public string UserName { get; set; }
        public Guid StudentId { get; set; }
        public decimal Average { get; set; }
        public decimal AverageInt { get; set; }
        public int Type { get; set; }
        public Guid CompetenceId { get; set; }
        public string CourseName { get; set; }
        public decimal Credits { get; set; }
    }

    public class RowChild4 
    {
        public string StudentFullName { get; set; }
        public Guid StudentId { get; set; }
        public Guid CompetenceId { get; set; }
        public string CompetenceName { get; set; }
        public decimal Average { get; set; }
        public string UserName { get; set; }
        public decimal Credits { get; set; }
        public Guid CourseId { get; set; }

    }

    public class StudentFormat
    {
        public string FullName { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }


    public class RangeLevel {
        public string Name { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public int Total { get; set; }
        public int Type { get; set; }
        public List<int> Array { get; set; }
    }

    public class Final
    {
        public List<Competencie> ListCompetencies { get; set; }
        public List<RowChild4> List { get; set; }
    }



}
