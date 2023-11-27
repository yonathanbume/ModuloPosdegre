using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class PortalNew : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(800)]
        public string Description { get; set; }
        public int System { get; set; } //Core.Helpers.ConstantHelpers.Solution
        public DateTime PublishDate { get; set; }
        public string Picture { get; set; }
    }
}
