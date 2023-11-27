using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IStudentComplementaryStudyService
    {
        Task<StudentComplementaryStudy> Get(Guid id);
        Task<List<StudentComplementaryStudy>> GetByStudentId(Guid studentId, int? type = null);
        Task<IEnumerable<StudentComplementaryStudy>> GetAll();
        Task Insert(StudentComplementaryStudy studentComplementaryStudy);
        Task Update(StudentComplementaryStudy studentComplementaryStudy);
        Task Delete(StudentComplementaryStudy studentComplementaryStudy);
    }
}
