using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencySubMenu
    {
        public Guid Id { get; set; }
        public byte Menu { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public ICollection<TransparencySubMenuFile> TransparencySubMenuFiles { get; set; }
    }
}
