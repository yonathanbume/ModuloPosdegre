using System;

namespace AKDEMIC.ENTITIES.Models.InstitutionalAgenda
{
    public class Inscription
    {
        public Guid Id { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string DocumentOfIdentification { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Guid AgendaEventId { get; set; }
        public AgendaEvent AgendaEvent { get; set; }
    }
}