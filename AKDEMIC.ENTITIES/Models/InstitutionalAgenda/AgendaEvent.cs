using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.InstitutionalAgenda
{
    public class AgendaEvent : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Pic { get; set; }
        public byte Type { get; set; } = CORE.Helpers.ConstantHelpers.Agenda.AgendaEvent.Type.Informative;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InscriptionStartDate { get; set; }
        public DateTime InscriptionEndDate { get; set; }
        public string Place { get; set; }
        public string DirectedTo { get; set; }
        public string Exhibitor { get; set; }
        public string Requiremetes { get; set; }
        public string Organizer { get; set; }
        public bool IsFree { get; set; } //s
        public decimal Cost { get; set; }
        public string InfoContact { get; set; }
        public Guid MenuOptionId { get; set; }
        public MenuOption MenuOption { get; set; }
    }
}