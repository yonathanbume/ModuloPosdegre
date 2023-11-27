using Microsoft.AspNetCore.Http;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.INTRANET.ViewModels.ForumViewModels
{
    public class TopicViewModel
    {
        public Guid Id { get; set; } 
        public string CurrentUserId { get; set; }
        public Guid PostCitedId { get; set; }
        public Guid TopicId { get; set; }
        public ApplicationUser CurrentUser { get; set; } 

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(300)")]
        public string Message { get; set; }

        [Display(Name = "Archivo adjunto", Prompt = "Archivo adjunto")]
        public IFormFile File { get; set; }

        public Guid UserId { get; set; }

        public Guid ForumId { get; set; }

        public ApplicationUser User { get; set; }

        public Forum Forum { get; set; } 
        public IEnumerable<Post> Posts { get; set; } 
    }
}
