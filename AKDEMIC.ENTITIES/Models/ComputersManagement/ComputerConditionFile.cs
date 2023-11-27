using System;

namespace AKDEMIC.ENTITIES.Models.ComputersManagement
{
    public class ComputerConditionFile
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }

        public Guid ComputerId { get; set; }
        public Computer Computer { get; set; }
    }
}
