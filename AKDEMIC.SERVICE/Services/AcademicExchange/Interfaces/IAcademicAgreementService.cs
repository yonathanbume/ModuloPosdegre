using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IAcademicAgreementService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicAgreementDataTable(DataTablesStructs.SentParameters parameters, Guid? type, int statusId,string startDate, bool? onlyActive, string search, Guid? careerId = null, Guid? facultyId = null);
        Task Insert(ENTITIES.Models.AcademicExchange.AcademicAgreement newAgreement);
        Task<ENTITIES.Models.AcademicExchange.AcademicAgreement> Get(Guid id);
        Task Update(ENTITIES.Models.AcademicExchange.AcademicAgreement editing);
        Task DeleteById(Guid id);
        Task<int> Count();
        Task<IEnumerable<Select2Structs.Result>> GetAcademicAgreementSelect2ClientSide(byte? type);
        Task<bool> IsAnotherWithResolutionNumber(string resolutionNumber, Guid? id=null);
        Task<object> GetAcademicAgreementChart();
        Task<object> GetAcademicAgreementActiveAndInactiveChart();
    }
}
