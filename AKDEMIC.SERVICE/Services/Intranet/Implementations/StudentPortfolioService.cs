using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentPortfolioService : IStudentPortfolioService
    {
        private readonly IStudentPortfolioRepository _studentPortfolioRepository;

        public StudentPortfolioService(IStudentPortfolioRepository studentPortfolioRepository)
        {
            _studentPortfolioRepository = studentPortfolioRepository;
        }

        public async Task<StudentPortfolio> Get(Guid id, Guid type)
            => await _studentPortfolioRepository.Get(id, type);

        public async Task<object> GetStudentPortfolioDatatable(Guid studentId, Guid? dependencyId = null, byte? type = null, bool? canUploadStudent = null, bool? onlyPending = null, ClaimsPrincipal user = null)
            => await _studentPortfolioRepository.GetStudentPortfolioDatatable(studentId, dependencyId, type, canUploadStudent, onlyPending, user);

        public async Task<List<StudentPortfolio>> GetStudentPortfoliosByStudent(Guid studentId)
            => await _studentPortfolioRepository.GetStudentPortfoliosByStudent(studentId);

        public async Task Insert(StudentPortfolio entity)
            => await _studentPortfolioRepository.Insert(entity);

        public async Task Update(StudentPortfolio entity)
            => await _studentPortfolioRepository.Update(entity);
    }
}
