using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.TeachingResearch
{
    public class ConvocationQuestion
    {
        public Guid Id { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public bool IsRequired { get; set; }
        [Required]
        public byte Type { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        public ICollection<ConvocationAnswer> Answers { get; set; }
        public ICollection<ConvocationAnswerByUser> AnswersByUsers { get; set; }
    }
}
