using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IAgreementTemplateRepository : IRepository<AgreementTemplate>
    {
        Task SaveChanges();

        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementTemplateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
