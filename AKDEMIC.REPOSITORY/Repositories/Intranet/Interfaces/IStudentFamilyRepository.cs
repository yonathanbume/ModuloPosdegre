using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentFamilyRepository : IRepository<StudentFamily>
    {
        Task<IEnumerable<StudentFamily>> GetAllStudentFamilysWithData();
        Task<List<StudentFamily>> GetStudentFamilyByStudentId(Guid studentId);
        Task<StudentFamily> GetStudentFamilyByUsername(string username);
        Task<object> GetStudentFamilySelectById(Guid id);
        Task<object> GetStudentFamilyByStudentIdSelect(Guid studentId);
        Task<StudentFamily> GetStudentFamilyByStudentInformationInclude(Guid studentInformationId);
        Task<object> GetStudentFamilySelecByIdInformation(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAllFamilyMembersFromStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
    }
}
