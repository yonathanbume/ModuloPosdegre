using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IAcademicAgreementTypeService
    {
        Task Insert(AcademicAgreementType entity);
        Task Delete(AcademicAgreementType entity);
        Task DeleteRange(IEnumerable<AcademicAgreementType> entities);
        Task InsertRange(IEnumerable<AcademicAgreementType> entities);
        Task<IEnumerable<AcademicAgreementType>> GetAllByAcademicAgreement(Guid academicAgreementId);
    }
}
