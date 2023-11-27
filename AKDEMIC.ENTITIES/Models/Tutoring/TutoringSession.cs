using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringSession
    {
        public Guid Id { get; set; }

        public Guid TermId { get; set; }

        public Term Term { get; set; }

        [Required]
        public string TutorId { get; set; }
        public string Topic { get; set; } //Tema, asunto
        public string Observations { get; set; }

        public Guid ClassroomId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? SendTime { get; set; }

        public bool IsDictated { get; set; } = false;
        public string urlGroup { get; set; }
        public string UrlFileSession { get; set; }
        public string UrlConference { get; set; }
        public Tutor Tutor { get; set; }

        public Classroom Classroom { get; set; }

        public ICollection<TutoringSessionStudent> TutoringSessionStudents { get; set; }

        public ICollection<TutoringSessionProblem> TutoringSessionProblems { get; set; }
    }
}
