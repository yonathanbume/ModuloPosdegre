using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class OrderChangeFileHistory
    {
        public Guid Id { get; set; }
        public Guid OrderChangeId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public OrderChangeHistory OrderChange { get; set; }
    }
}
