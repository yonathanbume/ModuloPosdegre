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
    public class ExamWeekService : IExamWeekService
    {
        private readonly IExamWeekRepository _examWeekRepository;

        public ExamWeekService(IExamWeekRepository examWeekRepository)
        {
            _examWeekRepository = examWeekRepository;
        }

        public async Task<bool> AnyByTermAndType(Guid termId, byte type)
            => await _examWeekRepository.AnyByTermAndType(termId, type);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExamWeekDatatable(DataTablesStructs.SentParameters parameters)
            => await _examWeekRepository.GetExamWeekDatatable(parameters);

        public async Task Insert(ExamWeek entity)
            => await _examWeekRepository.Insert(entity);
    }
}
