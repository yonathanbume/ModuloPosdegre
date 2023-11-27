using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ISupportOfficeRepository : IRepository<SupportOffice>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSupportOfficeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<List<SupportOffice>> GetAllWithOut(Guid? supportOfficeId = null);
        Task<object> GetSelect2WithOut(Guid? supportOfficeId = null);
    }
}
