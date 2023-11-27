using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class SupportOfficeUser
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public SupportOffice SupportOffice { get; set; }

        public Guid SupportOfficeId { get; set; }

        public ICollection<TutoringSessionStudent> TutoringSessionStudents { get; set; }

        public ICollection<TutoringAttendance> TutoringAttendances { get; set; }
    }
}
