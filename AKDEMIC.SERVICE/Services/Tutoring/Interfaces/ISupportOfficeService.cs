using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ISupportOfficeService
    {
        Task<IEnumerable<SupportOffice>> GetAll();
        Task<SupportOffice> Get(Guid supportOfficeId);
        Task Insert(SupportOffice supportOffice);
        Task Update(SupportOffice supportOffice);
        Task Delete(SupportOffice supportOffice);
        Task DeleteById(Guid id);
        Task<SupportOffice> Get(string supportOfficeId);
        Task<DataTablesStructs.ReturnedData<object>> GetSupportOfficeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<List<SupportOffice>> GetAllWithOut(Guid? supportOfficeId = null);
        Task<object> GetSelect2WithOut(Guid? supportOfficeId = null);
    }
}
