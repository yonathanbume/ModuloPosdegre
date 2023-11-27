using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentFeeTermRepository : IRepository<EnrollmentFeeTerm>
    {
        //Task<List<Select2Structs.Result>> GetAllSelect2Data();
        Task<EnrollmentFeeTerm> GetByScaleAndTerm(Guid studentScaleId, Guid termId, Guid? campusId = null, Guid? careerId = null, Guid? conceptId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid campusId, Guid? careerId = null, string search = null);
        Task DeleteWithAllFees(Guid enrollmentFeeTermId);
    }
}