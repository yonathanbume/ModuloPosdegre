using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureTaskService : IProcedureTaskService
    {
        private readonly IProcedureTaskRepository _procedureTaskRepository;

        public ProcedureTaskService(IProcedureTaskRepository procedureTaskRepository)
        {
            _procedureTaskRepository = procedureTaskRepository;
        }

        public async Task Delete(ProcedureTask entity)
            => await _procedureTaskRepository.Delete(entity);

        public async Task<ProcedureTask> Get(Guid id)
            => await _procedureTaskRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search)
            => await _procedureTaskRepository.GetDatatable(parameters, procedureId, search);

        public async Task<List<ProcedureTask>> GetProcedureTasks(Guid procedureId)
            => await _procedureTaskRepository.GetProcedureTasks(procedureId);

        public async Task Insert(ProcedureTask entity)
            => await _procedureTaskRepository.Insert(entity);

        public async Task Update(ProcedureTask entity)
            => await _procedureTaskRepository.Update(entity);
    }
}
