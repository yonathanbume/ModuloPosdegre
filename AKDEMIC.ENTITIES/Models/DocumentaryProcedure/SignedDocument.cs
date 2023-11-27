using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class SignedDocument : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Html { get; set; }

        public bool IsSigned { get; set; }

        public ApplicationUser User { get; set; }
    }
}
