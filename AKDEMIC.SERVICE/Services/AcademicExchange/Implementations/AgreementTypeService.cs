using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class AgreementTypeService : IAgreementTypeService
    {
        private readonly IAgreementTypeRepository _agreementTypeRepository;

        public AgreementTypeService(IAgreementTypeRepository agreementTypeRepository)
        {
            _agreementTypeRepository = agreementTypeRepository;
        }

        public async Task DeleteById(Guid typeId)
            => await _agreementTypeRepository.DeleteById(typeId);

        public async Task<AgreementType> Get(Guid typeId)
            => await _agreementTypeRepository.Get(typeId);

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _agreementTypeRepository.GetDataDatatable(sentParameters, search);

        public async Task<object> GetSelect2ClientSide()
            => await _agreementTypeRepository.GetSelect2ClientSide();

        public async Task Insert(AgreementType type)
            => await _agreementTypeRepository.Insert(type);

        public async Task Update(AgreementType type)
            => await _agreementTypeRepository.Update(type);
    }
}
