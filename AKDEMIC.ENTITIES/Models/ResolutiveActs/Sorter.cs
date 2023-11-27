using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.ResolutiveActs
{
    public class Sorter : Entity ,ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string CreatedFormattedDate => CreatedAt.ToLocalDateFormat();
    }
}
