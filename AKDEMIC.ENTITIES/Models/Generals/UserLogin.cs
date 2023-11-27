using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class UserLogin
    {
        public string UserId { get; set; }
        public byte System { get; set; }
        public DateTime FirstLogin { get; set; }
        public DateTime LastLogin { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public ApplicationUser User { get; set; }
    }
}
