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
    public class PreuniversitaryExamTeacherService : IPreuniversitaryExamTeacherService
    {
        private readonly IPreuniversitaryExamTeacherRepository _preuniversitaryExamTeacherRepository;

        public PreuniversitaryExamTeacherService(IPreuniversitaryExamTeacherRepository preuniversitaryExamTeacherRepository)
        {
            _preuniversitaryExamTeacherRepository = preuniversitaryExamTeacherRepository;
        }

        public async Task<bool> AnyUserByExam(Guid preuniversitaryExamId, string userId)
            => await _preuniversitaryExamTeacherRepository.AnyUserByExam(preuniversitaryExamId, userId);

        public async Task AssignTeachersRandomly(Guid preuniversitaryExamId)
            => await _preuniversitaryExamTeacherRepository.AssignTeachersRandomly(preuniversitaryExamId);

        public async Task Delete(PreuniversitaryExamTeacher entity)
            => await _preuniversitaryExamTeacherRepository.Delete(entity);

        public async Task<PreuniversitaryExamTeacher> Get(Guid id)
            => await _preuniversitaryExamTeacherRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
            => await _preuniversitaryExamTeacherRepository.GetDatatable(sentParameters, preuniversitaryExamId);

        public async Task Insert(PreuniversitaryExamTeacher entity)
            => await _preuniversitaryExamTeacherRepository.Insert(entity);

        public async Task InsertRange(List<PreuniversitaryExamTeacher> entities)
            => await _preuniversitaryExamTeacherRepository.InsertRange(entities);

        public async Task Update(PreuniversitaryExamTeacher entity)
            => await _preuniversitaryExamTeacherRepository.Update(entity);
    }
}
