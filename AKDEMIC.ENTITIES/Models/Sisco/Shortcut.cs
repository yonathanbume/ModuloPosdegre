using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class Shortcut
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UrlDirection { get; set; }
        public string Type { get; set; }

        public ICollection<SubShortcut> SubShortcuts { get; set; }
    }
    
}
