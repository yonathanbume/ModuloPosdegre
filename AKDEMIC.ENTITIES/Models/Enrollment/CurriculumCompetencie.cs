using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CurriculumCompetencie
    {
        [Key]
        public Guid CompetencieId { get; set; }
        [Key]
        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
        public Competencie Competencie { get; set; }
        public string Description { get; set; }
    }
}
