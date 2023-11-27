using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class AgreementTemplate : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string Title { get; set; }
        public string FilePath { get; set; }
    }
}
