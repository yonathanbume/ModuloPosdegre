using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class CafobeRequestDetail : Entity, ITimestamp
    {
        //Rendicion de cuentas de la solicitud

        [Key]
        public Guid CafobeRequestId { get; set; }

        [ForeignKey("CafobeRequestId")]
        public CafobeRequest CafobeRequest { get; set; }
        public int Status { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS;
        public string Comentary { get; set; }
        public string FileDetailUrl { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
