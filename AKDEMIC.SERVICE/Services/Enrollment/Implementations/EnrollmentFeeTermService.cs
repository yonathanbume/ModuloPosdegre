using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentFeeTermService : IEnrollmentFeeTermService
    {
        private readonly IEnrollmentFeeTermRepository _enrollmentFeeTermRepository;
        public EnrollmentFeeTermService(IEnrollmentFeeTermRepository enrollmentFeeTermRepository)
        {
            _enrollmentFeeTermRepository = enrollmentFeeTermRepository;
        }

        public async Task DeleteWithAllFees(Guid enrollmentFeeTermId)
            => await _enrollmentFeeTermRepository.DeleteWithAllFees(enrollmentFeeTermId);

        public async Task<EnrollmentFeeTerm> Get(Guid id)
            => await _enrollmentFeeTermRepository.Get(id);

        public async Task<EnrollmentFeeTerm> GetByScaleAndTerm(Guid studentScaleId, Guid termId, Guid? campusId = null, Guid? careerId = null, Guid? conceptId = null)
            => await _enrollmentFeeTermRepository.GetByScaleAndTerm(studentScaleId, termId, campusId, careerId, conceptId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid campusId, Guid? careerId = null, string search = null)
            => await _enrollmentFeeTermRepository.GetDataDatatable(sentParameters, termId, campusId, careerId, search);

        public async Task Insert(EnrollmentFeeTerm fee)
            => await _enrollmentFeeTermRepository.Insert(fee);

        public async Task Update(EnrollmentFeeTerm fee)
            => await _enrollmentFeeTermRepository.Update(fee);
    }
}
