using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class StudentUserProcedureService : IStudentUserProcedureService
    {
        private readonly IStudentUserProcedureRepository _studentUserProcedureRepository;

        public StudentUserProcedureService(
            IStudentUserProcedureRepository studentUserProcedureRepository
            )
        {
            _studentUserProcedureRepository = studentUserProcedureRepository;
        }

        public async Task Add(StudentUserProcedure entity)
            => await _studentUserProcedureRepository.Add(entity);

        public async Task Delete(StudentUserProcedure entity)
            => await _studentUserProcedureRepository.Delete(entity);

        public async Task<StudentUserProcedure> Get(Guid id)
            => await _studentUserProcedureRepository.Get(id);

        public async Task Insert(StudentUserProcedure entity)
            => await _studentUserProcedureRepository.Insert(entity);
    }
}
