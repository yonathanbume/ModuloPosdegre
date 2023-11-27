using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Answer
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public Question Question { get; set; }

        public ICollection<AnswerByUser> AnswerByUsers { get; set; }
    }
}
