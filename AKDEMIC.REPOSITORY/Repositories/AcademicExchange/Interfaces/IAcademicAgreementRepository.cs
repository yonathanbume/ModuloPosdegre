using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IAcademicAgreementRepository : IRepository<AcademicAgreement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicAgreementDataTable(DataTablesStructs.SentParameters parameters, Guid? type, int statusId,string startDate, bool? onlyActive, string search, Guid? careerId = null, Guid? facultyId = null);
        Task<IEnumerable<Select2Structs.Result>> GetAcademicAgreementSelect2ClientSide(byte? type);
        Task<bool> IsAnotherWithResolutionNumber(string resolutionNumber, Guid? id);
        Task<object> GetAcademicAgreementChart();
        Task<object> GetAcademicAgreementActiveAndInactiveChart();
    }
}
