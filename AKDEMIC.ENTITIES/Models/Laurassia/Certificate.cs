using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Certificate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Correlative { get; set; }
        public string Course { get; set; }
        public int Hours { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
