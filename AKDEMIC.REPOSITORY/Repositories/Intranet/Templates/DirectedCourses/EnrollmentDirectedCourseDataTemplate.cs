﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DirectedCourses
{
    public class EnrollmentDirectedCourseDataTemplate
    {
        public List<DirectedCourseDataTemplate> StudentEnrollmentDirectedCourse{ get; set; }
        public int CurriculumCode { get; set; }
    }

    public class DirectedCourseDataTemplate
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public byte AcademicYear { get; set; }
        public int StudentDirectedCount { get; set; }
    }
}
