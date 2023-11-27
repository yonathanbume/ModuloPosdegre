using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IAgreementTemplateService
    {
        Task<AgreementTemplate> Get(Guid id);
        Task<IEnumerable<AgreementTemplate>> GetAll();
        Task SaveChanges();
        Task Insert(AgreementTemplate agreementTemplate);
        Task Add(AgreementTemplate agreementTemplate);
        Task Delete(AgreementTemplate agreementTemplate);

        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementTemplateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
