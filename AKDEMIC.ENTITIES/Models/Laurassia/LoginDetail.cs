using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class LoginDetail : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string LoginId { get; set; }
        public string Connection { get; set; }
        public string Segment { get; set; }
        public Guid? Sectionid { get; set; }
        public string UserId { get; set; }
        public  Login Login { get; set; }
    }
}
