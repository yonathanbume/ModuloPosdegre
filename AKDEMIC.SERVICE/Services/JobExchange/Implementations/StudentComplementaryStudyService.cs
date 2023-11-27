using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentComplementaryStudyService : IStudentComplementaryStudyService
    {
        private readonly IStudentComplementaryStudyRepository _studentComplementaryStudyRepository;
        public StudentComplementaryStudyService(IStudentComplementaryStudyRepository studentComplementaryStudyRepository)
        {
            _studentComplementaryStudyRepository = studentComplementaryStudyRepository;
        }
        public Task Delete(StudentComplementaryStudy studentComplementaryStudy)
            => _studentComplementaryStudyRepository.Delete(studentComplementaryStudy);

        public Task<StudentComplementaryStudy> Get(Guid id)
            => _studentComplementaryStudyRepository.Get(id);

        public Task<IEnumerable<StudentComplementaryStudy>> GetAll()
            => _studentComplementaryStudyRepository.GetAll();

        public Task<List<StudentComplementaryStudy>> GetByStudentId(Guid studentId, int? type = null)
            => _studentComplementaryStudyRepository.GetByStudentId(studentId, type);

        public Task Insert(StudentComplementaryStudy studentComplementaryStudy)
            => _studentComplementaryStudyRepository.Insert(studentComplementaryStudy);

        public Task Update(StudentComplementaryStudy studentComplementaryStudy)
            => _studentComplementaryStudyRepository.Update(studentComplementaryStudy);
    }
}
