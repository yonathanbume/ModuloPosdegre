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
    public class WorkCertificateRecordService : IWorkCertificateRecordService
    {
        private readonly IWorkCertificateRecordRepository _workCertificateRecordRepository;

        public WorkCertificateRecordService(IWorkCertificateRecordRepository workCertificateRecordRepository)
        {
            _workCertificateRecordRepository = workCertificateRecordRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkCertificateRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _workCertificateRecordRepository.GetWorkCertificateRecordsDatatable(sentParameters, userId);

        public async Task Insert(WorkCertificateRecord entity)
            => await _workCertificateRecordRepository.Insert(entity);
    }
}
