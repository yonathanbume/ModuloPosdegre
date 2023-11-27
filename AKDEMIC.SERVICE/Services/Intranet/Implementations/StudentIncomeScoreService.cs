using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentIncomeScoreService : IStudentIncomeScoreService
    {
        private readonly IStudentIncomeScoreRepository _studentIncomeScoreRepository;

        public StudentIncomeScoreService(IStudentIncomeScoreRepository studentIncomeScoreRepository)
        {
            _studentIncomeScoreRepository = studentIncomeScoreRepository;
        }
        public async Task<StudentIncomeScore> Get(Guid id)
            => await _studentIncomeScoreRepository.Get(id);

        public async Task<StudentIncomeScore> GetByStudent(Guid studentId)
            => await _studentIncomeScoreRepository.GetByStudent(studentId);

        public async Task Insert(StudentIncomeScore entity)
            => await _studentIncomeScoreRepository.Insert(entity);

        public async Task Update(StudentIncomeScore entity)
            => await _studentIncomeScoreRepository.Update(entity);
    }
}
