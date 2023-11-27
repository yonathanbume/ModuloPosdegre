using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyPortalGeneral
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public ICollection<TransparencyPortalGeneralFile> Files { get; set; }
    }
}
