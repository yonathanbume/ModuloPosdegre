using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerAssistance
{
    public class WorkerReportTemplate
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<WorkerAssistanceTemplate> Assistances  { get; set; }
    }

    public class WorkerAssistanceTemplate
    {
        public string RegisterDate { get; set; }
        public string StartTime { get; set; }
        public string Endtime { get; set; }
        public string StatusText { get; set; }
    }
}
