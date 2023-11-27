using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeViewModels
{
    public class DeleteGradesByStudent
    {
        public Guid StudentId { get; set; }
        public Guid SectionId { get; set; }
    }
}
