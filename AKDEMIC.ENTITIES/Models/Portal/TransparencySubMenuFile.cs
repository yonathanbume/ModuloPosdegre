using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencySubMenuFile
    {
        public Guid Id { get; set; }
        public Guid TransparencySubMenuId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public TransparencySubMenu TransparencySubMenu { get; set; }
    }
}
