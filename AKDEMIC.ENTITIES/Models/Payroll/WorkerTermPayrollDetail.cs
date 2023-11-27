using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class WorkerTermPayrollDetail : Entity, ITimestamp  //Detalle Planilla por trabajador por periodo
    {
        public Guid Id { get; set; }

        public Guid WageItemId { get; set; } //CONCEPTOS REMUNERATIVOS

        public WageItem WageItem { get; set; }

        public Guid WorkingTermId {get; set;} //PERIODOS

        public WorkingTerm WorkingTerm { get; set; }

        public decimal Amount { get; set; }

        public Guid WorkerId { get; set; } //TRABAJADOR

        public Worker Worker { get; set; }
    }
}
