using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("Homework")]
    public class Homework : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateBegin { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }
        [Required]
        public byte Attempts { get; set; }
        public bool IsRecovery { get; set; } = false;
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
        public bool Show { get; set; } = true;
        public bool IsGroup { get; set; } = false;
        public byte RubricType { get; set; } = 0;
        public byte FileExtension { get; set; } = 0;
        public bool Turnitin { get; set; } = false;
        public ICollection<RubricItem> RubricItems { get; set; }
        public ICollection<HomeworkStudent> HomeworkStudent { get; set; }
        public ICollection<HomeworkFile> HomeworkFiles { get; set; }
        public ICollection<RecoveryHomeworkStudent> RecoveryHomeworkStudents { get; set; }
    }
}
