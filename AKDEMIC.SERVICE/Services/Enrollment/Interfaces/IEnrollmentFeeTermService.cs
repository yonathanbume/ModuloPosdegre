using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentFeeTermService
    {
        Task<EnrollmentFeeTerm> Get(Guid id);
        Task Insert(EnrollmentFeeTerm fee);
        Task Update(EnrollmentFeeTerm fee);
        Task<EnrollmentFeeTerm> GetByScaleAndTerm(Guid studentScaleId, Guid termId, Guid? campusId = null, Guid? careerId = null, Guid? conceptId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid campusId, Guid? careerId = null, string search = null);
        Task DeleteWithAllFees(Guid enrollmentFeeTermId);
    }
}
