using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class AgreementService: IAgreementService
    {
        private readonly IAgreementRepository _agreementRepository;

        public AgreementService(IAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;
        }

        public async Task Delete(Agreement agreement)
        {
            await _agreementRepository.Delete(agreement);
        }

        public async Task<Agreement> Get(Guid id)
        {
            return await _agreementRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAgreementDatatable(DataTablesStructs.SentParameters sentParameters, int state , bool isActive, string searchValue = null)
        {
            return await _agreementRepository.GetAgreementDatatable(sentParameters, state, isActive, searchValue);
        }

        public async Task<object> GetAgreementsSelect2()
        {
            return await _agreementRepository.GetAgreementsSelect2();
        }

        public async Task Insert(Agreement agreement)
        {
            await _agreementRepository.Insert(agreement);
        }

        public async Task Update(Agreement agreement)
        {
            await _agreementRepository.Update(agreement);
        }
    }
}
