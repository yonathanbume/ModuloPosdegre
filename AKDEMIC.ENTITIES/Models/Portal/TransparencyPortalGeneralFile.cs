using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyPortalGeneralFile
    {
        public Guid Id { get; set; }

        public Guid TransparencyPortalGeneralId { get; set; }

        public string Name { get; set; }

        public bool IsLink { get; set; }

        public string Path { get; set; }

        public TransparencyPortalGeneral TransparencyPortalGeneral { get; set; }
    }
}
