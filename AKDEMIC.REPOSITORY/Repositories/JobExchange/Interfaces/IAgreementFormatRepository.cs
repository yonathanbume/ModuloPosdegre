using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IAgreementFormatRepository:IRepository<AgreementFormat>
    {
        Task SaveChanges();
        Task<object> GetById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatDatatable(DataTablesStructs.SentParameters sentParameters, int? state = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatNotAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatByCompanyDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, string searchValue = null);
    }
}
