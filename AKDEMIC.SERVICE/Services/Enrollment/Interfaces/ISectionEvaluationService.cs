using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ISectionEvaluationService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid? sectionId);
    }
}
