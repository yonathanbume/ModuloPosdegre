using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Survey 
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public byte State { get; set; }

        public byte Type { get; set; }

        public int System { get; set; }

        public DateTime PublicationDate { get; set; } 
        public DateTime FinishDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsRequired { get; set; }
        public bool IsAnonymous { get; set; }

        public ICollection<SurveyItem> SurveyItems { get; set; }
        public ICollection<SurveyUser> SurveyUsers { get; set; }
        //public bool Active { get; set; } = true;

        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
    }
}
