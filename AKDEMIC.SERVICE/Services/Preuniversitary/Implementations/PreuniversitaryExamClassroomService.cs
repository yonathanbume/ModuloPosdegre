using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryExamClassroomService : IPreuniversitaryExamClassroomService
    {
        private readonly IPreuniversitaryExamClassroomRepository _preuniversitaryExamClassroomRepository;

        public PreuniversitaryExamClassroomService(IPreuniversitaryExamClassroomRepository preuniversitaryExamClassroomRepository)
        {
            _preuniversitaryExamClassroomRepository = preuniversitaryExamClassroomRepository;
        }

        public async Task<bool> AnyClassroomByExam(Guid classroomId, Guid preuniversitaryExamId, Guid? ignoredId = null)
            => await _preuniversitaryExamClassroomRepository.AnyClassroomByExam(classroomId, preuniversitaryExamId, ignoredId);

        public async Task Delete(PreuniversitaryExamClassroom entity)
            => await _preuniversitaryExamClassroomRepository.Delete(entity);

        public async Task<PreuniversitaryExamClassroom> Get(Guid id)
            => await _preuniversitaryExamClassroomRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
            => await _preuniversitaryExamClassroomRepository.GetDatatable(sentParameters, preuniversitaryExamId);

        public async Task Insert(PreuniversitaryExamClassroom entity)
            => await _preuniversitaryExamClassroomRepository.Insert(entity);

        public async Task Update(PreuniversitaryExamClassroom entity)
            => await _preuniversitaryExamClassroomRepository.Update(entity);
    }
}
