using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularActivityStudentService : IExtracurricularActivityStudentService
    {
        private readonly IExtracurricularActivityStudentRepository _extracurricularActivityStudentRepository;

        public ExtracurricularActivityStudentService(IExtracurricularActivityStudentRepository extracurricularActivityStudentRepository)
        {
            _extracurricularActivityStudentRepository = extracurricularActivityStudentRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularActivityStudentRepository.DeleteById(id);

        public Task<ExtracurricularActivityStudent> Get(Guid id)
            => _extracurricularActivityStudentRepository.Get(id);

        public Task<IEnumerable<ExtracurricularActivityStudent>> GetAll()
            => _extracurricularActivityStudentRepository.GetAll();

        public async Task<List<ExtracurricularActivityStudent>> GetAllByStudent(Guid studentId)
            => await _extracurricularActivityStudentRepository.GetAllByStudent(studentId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, string search)
            => await _extracurricularActivityStudentRepository.GetDatatable(parameters, termId, search);

        public Task Insert(ExtracurricularActivityStudent extracurricularActivityStudent)
            => _extracurricularActivityStudentRepository.Insert(extracurricularActivityStudent);

        public Task Update(ExtracurricularActivityStudent extracurricularActivityStudent)
            => _extracurricularActivityStudentRepository.Update(extracurricularActivityStudent);
    }
}
