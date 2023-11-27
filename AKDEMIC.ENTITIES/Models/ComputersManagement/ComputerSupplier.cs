using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class ComputerSupplier
    {
        public Guid Id { get; set; }

        public string Ruc { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }
}
