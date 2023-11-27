using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IAgreementFormatService
    {
        Task<AgreementFormat> Get(Guid  id);
        Task<object> GetById(Guid id);
        Task<IEnumerable<AgreementFormat>> GetAll();
        Task SaveChanges();
        Task Insert(AgreementFormat agreementFormat);
        Task Add(AgreementFormat agreementFormat);
        Task Delete(AgreementFormat agreementFormat);

        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatDatatable(DataTablesStructs.SentParameters sentParameters,int? state = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatNotAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatByCompanyDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, string searchValue = null);
    }
}
