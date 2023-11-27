using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.ResolutiveActs
{
    public class Document : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public byte Type { get; set; } = ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.TYPE.ACT;

        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public string Description { get; set; }
        public string Matter { get; set; }
        public string Number { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ResolutionDate { get; set; }

        public string Html { get; set; }

        public byte Status { get; set; } = AKDEMIC.CORE.Helpers.ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.GENERATED;

        [Required]
        public Guid SorterId { get; set; }
        public Sorter Sorter { get; set; }

        public Guid? ResolutionCategoryId { get; set; }
        public ResolutionCategory ResolutionCategory { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<DocumentFile> DocumentFiles { get; set; }

        [NotMapped]
        public string ResolutionFormattedDate => ResolutionDate.ToLocalDateFormat();
        [NotMapped]
        public string StatusStr { get; set; }
    }
}
