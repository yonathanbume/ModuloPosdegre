using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Forum : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(150)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(200)")]
        public string Description { get; set; }
        public bool Active { get; set; } = true;

        public byte System { get; set; }
        public bool CanUploadFile { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public ICollection<ForumCareer> ForumCareers { get; set; }
    }
}