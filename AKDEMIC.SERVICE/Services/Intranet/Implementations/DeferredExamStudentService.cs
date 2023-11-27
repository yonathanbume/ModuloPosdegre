using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DeferredExamStudentService : IDeferredExamStudentService
    {
        private readonly IDeferredExamStudentRepository _deferredExamStudentRepository;

        public DeferredExamStudentService(IDeferredExamStudentRepository deferredExamStudentRepository)
        {
            _deferredExamStudentRepository = deferredExamStudentRepository;
        }

        public async Task Delete(DeferredExamStudent entity)
            => await _deferredExamStudentRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<DeferredExamStudent> entities)
            => await _deferredExamStudentRepository.DeleteRange(entities);

        public async Task<DeferredExamStudent> Get(Guid id)
            => await _deferredExamStudentRepository.Get(id);

        public async Task<List<DeferredExamStudent>> GetAll(Guid deferredExamId)
            => await _deferredExamStudentRepository.GetAll(deferredExamId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeferredExamStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid deferredExamId, string search)
            => await _deferredExamStudentRepository.GetDeferredExamStudentsDatatable(parameters, deferredExamId, search);

        public async Task InsertRange(IEnumerable<DeferredExamStudent> entities)
            => await _deferredExamStudentRepository.InsertRange(entities);
    }
}
