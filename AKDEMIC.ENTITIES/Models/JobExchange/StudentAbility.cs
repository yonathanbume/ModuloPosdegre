using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class StudentAbility
    {
        public Guid StudentId { get; set; }

        public Guid AbilityId { get; set; }

        public byte Level { get; set; } = 1;
        public string DocumentFilePath { get; set; }

        public Student Student { get; set; }

        public Ability Ability { get; set; }
    }
}