using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class TeacherPortfolioService : ITeacherPortfolioService
    {
        private readonly ITeacherPortfolioRepository _teacherPortfolioRepository;

        public TeacherPortfolioService(
            ITeacherPortfolioRepository teacherPortfolioRepository
            )
        {
            _teacherPortfolioRepository = teacherPortfolioRepository;
        }

        public async Task Delete(TeacherPortfolio entity)
            => await _teacherPortfolioRepository.Delete(entity);

        public async Task<TeacherPortfolio> Get(Guid id)
            => await _teacherPortfolioRepository.Get(id);

        public async Task<object> GetPortfolioCourseSyllabusDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
            => await _teacherPortfolioRepository.GetPortfolioCourseSyllabusDatatable(parameters, termId, teacherId, search);

        public async Task<object> GetPortfolioCurricularDesignDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
            => await _teacherPortfolioRepository.GetPortfolioCurricularDesignDatatable(parameters, termId, teacherId, search);

        public async Task<object> GetPortfolioCurriculumDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
            => await _teacherPortfolioRepository.GetPortfolioCurriculumDatatable(parameters, termId, teacherId, search);

        public async Task<object> GetPortfolioDatatable(DataTablesStructs.SentParameters parameters, string teacherId, byte folder, string search)
            => await _teacherPortfolioRepository.GetPortfolioDatatable(parameters, teacherId, folder, search);

        public async Task Insert(TeacherPortfolio entity)
            => await _teacherPortfolioRepository.Insert(entity);
    }
}
