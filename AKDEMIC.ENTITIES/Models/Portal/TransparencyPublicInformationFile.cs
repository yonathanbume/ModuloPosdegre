using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyPublicInformationFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }
        public bool IsLink { get; set; }
        public Guid TransparencyPublicInformationId { get; set; }

        public TransparencyPublicInformation TransparencyPublicInformation { get; set; }
    }
}
