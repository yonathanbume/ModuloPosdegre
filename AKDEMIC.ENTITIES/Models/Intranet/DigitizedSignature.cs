using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DigitizedSignature : Entity , ITimestamp
    {
        public Guid Id { get; set; }
        public string UrlSignature { get; set; }
    }
}
