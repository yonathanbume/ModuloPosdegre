using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TermInform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITermInformRepository : IRepository<TermInform>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTermInformDatatable(DataTablesStructs.SentParameters parameters, Guid? termId);
        Task<bool> AnyTermInformTeacher(Guid id);
        Task<TermInformTemplate> GetTermInformTemplate(string teacherId, byte requestType);
        Task<bool> AnyByType(Guid termId, byte type);
        Task<DataTablesStructs.ReturnedData<object>> GetTermInformReportDatatable(DataTablesStructs.SentParameters parameters, Guid termId, byte type, string searchValue);

        Task<object> GetByFilters(Guid termId, byte type);
    }
}