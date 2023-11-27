using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Post : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? PostCitedId { get; set; }
        public Guid TopicId { get; set; }        
        public string PathFile { get; set; }
        public string FileName { get; set; }
        public int Level { get; set; }



        [Required]
        [Column(TypeName = "VARCHAR(1000)")]
        public string Message { get; set; }

        public ApplicationUser User { get; set; }
        public Post PostCited { get; set; }
        public Topic Topic { get; set; }
    }
}