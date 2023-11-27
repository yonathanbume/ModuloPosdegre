using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DocumentFormat.OpenXml.Office2010.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class CorrectionExamService : ICorrectionExamService
    {
        private readonly ICorrectionExamRepository _correctionExamRepository;

        public CorrectionExamService(
            ICorrectionExamRepository correctionExamRepository
            )
        {
            _correctionExamRepository = correctionExamRepository;
        }

        public async Task<bool> AnyBySection(Guid sectionId)
            => await _correctionExamRepository.AnyBySection(sectionId);

        public async Task Delete(CorrectionExam entity)
            => await _correctionExamRepository.Delete(entity);

        public async Task<CorrectionExam> Get(Guid id)
            => await _correctionExamRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search, ClaimsPrincipal user = null)
            => await _correctionExamRepository.GetDatatable(parameters, termId, careerId, curriculumId, academicYear, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentToCorrectionExam(DataTablesStructs.SentParameters parameters, Guid correctionExamId, string search)
            => await _correctionExamRepository.GetStudentToCorrectionExam(parameters, correctionExamId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string teacherId, string search)
            => await _correctionExamRepository.GetTeacherDatatable(parameters,termId, teacherId, search);

        public async Task Insert(CorrectionExam entity)
            => await _correctionExamRepository.Insert(entity);

        public async Task Update(CorrectionExam entity)
            => await _correctionExamRepository.Update(entity);
    }
}
