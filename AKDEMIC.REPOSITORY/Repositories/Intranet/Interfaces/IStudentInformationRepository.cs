using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentInformationRepository : IRepository<StudentInformation>
    {
        Task<bool> HasStudentInformation(Guid studentId);
        Task<StudentInformation> GetByStudentAndTerm(Guid studentId, Guid termId);
        Task<object> GetOriginSchoolSelect();
    }
}
