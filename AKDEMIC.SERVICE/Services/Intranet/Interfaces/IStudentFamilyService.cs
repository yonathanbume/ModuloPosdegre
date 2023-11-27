using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentFamilyService
    {
        Task AddAsync(StudentFamily studentFamily);
        Task InsertStudentFamily(StudentFamily studentFamily);
        Task UpdateStudentFamily(StudentFamily studentFamily);
        Task DeleteStudentFamily(StudentFamily studentFamily);
        Task<StudentFamily> GetStudentFamilyById(Guid id);
        Task<IEnumerable<StudentFamily>> GetAllStudentFamilys();
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
