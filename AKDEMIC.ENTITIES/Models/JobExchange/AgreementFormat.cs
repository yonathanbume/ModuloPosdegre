using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class AgreementFormat: Entity, ITimestamp
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string Title { get; set; }
        public string FilePath { get; set; }
        public int State { get; set; }
        public DateTime? ApprovedAt { get; set; }
        [StringLength(800)]
        public string Observations { get; set; }

        public string ApprovedUserId { get; set; }
        public Guid? CompanyId { get; set; }

        //Empresa que solicito el formato de convenio
        public Company Company { get; set; }
        //Usuario que aprobo la solicitud
        public ApplicationUser ApprovedUser { get; set; }
    }
}
