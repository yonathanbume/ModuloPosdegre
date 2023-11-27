using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentInformationService
    {
        Task<StudentInformation> Get(Guid studentInformationId);
        Task<bool> HasStudentInformation(Guid studentId);
        Task<StudentInformation> GetByStudentAndTerm(Guid studentId , Guid termId);
        Task<bool> Any(Guid studentInformationId);
        Task DeleteById(Guid studentInformationId);
        Task Delete(StudentInformation studentInformation);
        Task Update(StudentInformation studentInformation);
        Task Insert(StudentInformation studentInformation);
        Task<IEnumerable<StudentInformation>> GetAll();
        Task<object> GetOriginSchoolSelect();
    }
}
