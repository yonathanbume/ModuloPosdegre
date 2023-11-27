using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class SiafExpense : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Record { get; set; }

        public string Document { get; set; }

        public DateTime Date { get; set; }

        public string Client { get; set; }

        public decimal Amount { get; set; }

        public string AssociatedDocument { get; set; }

        public string Description { get; set; }

        public bool Received { get; set; }

        public string RoleId { get; set; }

        public string Observations { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
