using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class AcademicAgreementTypeService : IAcademicAgreementTypeService
    {
        private readonly IAcademicAgreementTypeRepository _academicAgreementTypeRepository;

        public AcademicAgreementTypeService(IAcademicAgreementTypeRepository academicAgreementTypeRepository)
        {
            _academicAgreementTypeRepository = academicAgreementTypeRepository;
        }

        public async Task Delete(AcademicAgreementType entity)
            => await _academicAgreementTypeRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<AcademicAgreementType> entities)
            => await _academicAgreementTypeRepository.DeleteRange(entities);

        public async Task<IEnumerable<AcademicAgreementType>> GetAllByAcademicAgreement(Guid academicAgreementId)
            => await _academicAgreementTypeRepository.GetAllByAcademicAgreement(academicAgreementId);

        public async Task Insert(AcademicAgreementType entity)
            => await _academicAgreementTypeRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<AcademicAgreementType> entities)
            => await _academicAgreementTypeRepository.InsertRange(entities);
    }
}
