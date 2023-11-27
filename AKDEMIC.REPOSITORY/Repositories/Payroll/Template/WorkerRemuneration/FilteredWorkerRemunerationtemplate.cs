using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration
{
    public class FilteredWorkerRemunerationtemplate
    {

        public string WageItemName { get; set; }
        
        public string WorkingTerm { get; set; }

        public decimal Amount { get; set; }

    }
}
