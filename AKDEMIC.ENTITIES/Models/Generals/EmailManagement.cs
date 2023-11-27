using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class EmailManagement : ITimestamp
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        public int System { get; set; }
    }
}
