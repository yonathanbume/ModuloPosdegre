using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationComitee
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public ApplicationUser User { get; set; }
    }
}
