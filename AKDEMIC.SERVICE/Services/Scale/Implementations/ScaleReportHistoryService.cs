using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleReportHistoryService : IScaleReportHistoryService
    {

        private readonly IScaleReportHistoryRepository _scaleReportHistoryRepository;

        public ScaleReportHistoryService(IScaleReportHistoryRepository scaleReportHistoryRepository)
        {
            _scaleReportHistoryRepository = scaleReportHistoryRepository;
        }
        public Task<ScaleReportHistory> Add(ScaleReportHistory scaleReportHistory)
            => _scaleReportHistoryRepository.Add(scaleReportHistory);

        public Task<int> Count()
            => _scaleReportHistoryRepository.Count();

        public Task<DataTablesStructs.ReturnedData<object>> GetScaleReportHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => _scaleReportHistoryRepository.GetScaleReportHistoryDatatable(sentParameters,userId);

        public Task Insert(ScaleReportHistory scaleReportHistory)
            => _scaleReportHistoryRepository.Insert(scaleReportHistory);
    }
}
