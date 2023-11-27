using System;

namespace AKDEMIC.ENTITIES.Models.InstitutionalAgenda
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string DocumentOfIdentification { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}