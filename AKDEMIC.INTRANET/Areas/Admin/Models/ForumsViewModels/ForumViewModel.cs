using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ForumsViewModels
{
    public class ForumViewModel
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        [Required]
        public Guid[] Careers { get; set; }
    }
}