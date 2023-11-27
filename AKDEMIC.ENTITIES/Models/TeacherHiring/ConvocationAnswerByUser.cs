using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationAnswerByUser
    {
        public Guid Id { get; set; }
        public Guid ConvocationQuestionId { get; set; }
        public ConvocationQuestion ConvocationQuestion { get; set; }
        public string AnswerDescription { get; set; }
        public Guid? ConvocationAnswerId { get; set; }
        public ConvocationAnswer ConvocationAnswer { get; set; }
        public Guid ApplicantTeacherId { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
    }
}
