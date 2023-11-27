using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.InstitutionalAgenda
{
    public class MenuOption
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ValidUrl { get; set; }
        public bool ToShow { get; set; }

        public IEnumerable<AgendaEvent> AgendaEvents { get; set; }
    }
}