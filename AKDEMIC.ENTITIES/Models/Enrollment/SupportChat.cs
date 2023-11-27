using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class SupportChat
    {
        public Guid Id { get; set; }
        public string EmisorId { get; set; }
        public string ReceptorId { get; set; }
        public ApplicationUser Emisor { get; set; }
        public ApplicationUser Receptor { get; set; }       
        public ICollection<SupportMessage> Mensaje { get; set; }
    }
}
