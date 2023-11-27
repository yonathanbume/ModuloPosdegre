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
    public class AgreementTemplateService : IAgreementTemplateService
    {

        private readonly IAgreementTemplateRepository _agreementTemplateRepository;
        public AgreementTemplateService(
            IAgreementTemplateRepository agreementTemplateRepository
        )
        {
            _agreementTemplateRepository = agreementTemplateRepository;
        }


        public Task Add(AgreementTemplate agreementTemplate)
            => _agreementTemplateRepository.Add(agreementTemplate);

        public Task Delete(AgreementTemplate agreementTemplate)
            => _agreementTemplateRepository.Delete(agreementTemplate);

        public Task<AgreementTemplate> Get(Guid id)
            => _agreementTemplateRepository.Get(id);

        public Task<IEnumerable<AgreementTemplate>> GetAll()
            => _agreementTemplateRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementTemplateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _agreementTemplateRepository.GetAllAgreementTemplateDatatable(sentParameters, searchValue);

        public Task Insert(AgreementTemplate agreementTemplate)
            => _agreementTemplateRepository.Insert(agreementTemplate);

        public Task SaveChanges()
            => _agreementTemplateRepository.SaveChanges();

    }
}
