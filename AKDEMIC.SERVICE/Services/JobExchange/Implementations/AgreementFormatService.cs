using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class AgreementFormatService : IAgreementFormatService
    {
        private readonly IAgreementFormatRepository _agreementFormatRepository;

        public AgreementFormatService(IAgreementFormatRepository agreementFormatRepository)
        {
            _agreementFormatRepository = agreementFormatRepository;
        }

        public Task Add(AgreementFormat agreementFormat)
            => _agreementFormatRepository.Add(agreementFormat);

        public Task Delete(AgreementFormat agreementFormat)
            => _agreementFormatRepository.Delete(agreementFormat);

        public Task<AgreementFormat> Get(Guid id)
            => _agreementFormatRepository.Get(id);

        public Task<IEnumerable<AgreementFormat>> GetAll()
            => _agreementFormatRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatByCompanyDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, string searchValue = null)
            => _agreementFormatRepository.GetAllAgreementFormatByCompanyDatatable(sentParameters, companyId, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatDatatable(DataTablesStructs.SentParameters sentParameters, int? state = null, string searchValue = null)
            => _agreementFormatRepository.GetAllAgreementFormatDatatable(sentParameters, state, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatNotAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _agreementFormatRepository.GetAllAgreementFormatNotAcceptedDatatable(sentParameters, searchValue);

        public Task<object> GetById(Guid id)
            => _agreementFormatRepository.GetById(id);

        public Task Insert(AgreementFormat agreementFormat)
            => _agreementFormatRepository.Insert(agreementFormat);

        public Task SaveChanges()
            => _agreementFormatRepository.SaveChanges();
    }
}
