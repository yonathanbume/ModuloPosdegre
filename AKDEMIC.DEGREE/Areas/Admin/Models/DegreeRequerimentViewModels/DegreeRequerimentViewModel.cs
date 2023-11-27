using System;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.DegreeRequerimentViewModels
{
    public class DegreeRequerimentViewModel
    {
        public Guid Id { get; set; }
        public Guid DegreeId { get; set; }
        public Guid? ProcedureId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }        
    }
}
