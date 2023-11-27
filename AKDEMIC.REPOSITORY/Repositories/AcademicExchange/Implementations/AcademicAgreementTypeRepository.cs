using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class AcademicAgreementTypeRepository : Repository<AcademicAgreementType> , IAcademicAgreementTypeRepository
    {
        public AcademicAgreementTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<AcademicAgreementType>> GetAllByAcademicAgreement(Guid academicAgreementId)
            => await _context.AcademicAgreementTypes.Where(x => x.AcademicAgreementId == academicAgreementId).ToArrayAsync();
    }
}
