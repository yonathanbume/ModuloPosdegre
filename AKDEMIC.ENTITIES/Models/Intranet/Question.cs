using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Question
    {
        public Guid Id { get; set; }

        public Guid SurveyItemId { get; set; }
        public SurveyItem SurveyItem { get; set; }

        [Required]
        public int Type { get; set; } 

        [Required]
        [StringLength(500)]
        public String Description { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<AnswerByUser> AnswerByUsers { get; set; }

    }
}
