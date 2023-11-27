using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComUserGroup
    {
        public Guid Id { get; set; }
        public Guid ComGroupId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ComGroup ComGroup { get; set; }
        public Guid? PaymentId { get; set; }
        public Payment Payment  { get; set; }

    }
}
