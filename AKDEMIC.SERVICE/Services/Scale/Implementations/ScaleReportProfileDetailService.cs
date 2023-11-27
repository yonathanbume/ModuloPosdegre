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
    public class ScaleReportProfileDetailService: IScaleReportProfileDetailService
    {
        private readonly IScaleReportProfileDetailRepository _scaleReportProfileDetailRepository;

        public ScaleReportProfileDetailService(IScaleReportProfileDetailRepository scaleReportProfileDetailRepository)
        {
            _scaleReportProfileDetailRepository = scaleReportProfileDetailRepository;
        }

        public Task<bool> AnyBySectionNumber(int sectionNumber, Guid reportProfileId, Guid? id = null)
            => _scaleReportProfileDetailRepository.AnyBySectionNumber(sectionNumber, reportProfileId, id);

        public Task Delete(ScaleReportProfileDetail scaleReportProfileDetail)
            => _scaleReportProfileDetailRepository.Delete(scaleReportProfileDetail);

        public Task<ScaleReportProfileDetail> Get(Guid id)
            => _scaleReportProfileDetailRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDetailDataTable(DataTablesStructs.SentParameters sentParameters,Guid profileId, string searchValue = null)
            => _scaleReportProfileDetailRepository.GetReportProfileDetailDataTable(sentParameters,profileId,searchValue);

        public Task<List<int>> GetSectionsByProfile(Guid profileId)
            => _scaleReportProfileDetailRepository.GetSectionsByProfile(profileId);

        public Task Insert(ScaleReportProfileDetail scaleReportProfileDetail)
            => _scaleReportProfileDetailRepository.Insert(scaleReportProfileDetail);

        public Task Update(ScaleReportProfileDetail scaleReportProfileDetail)
            => _scaleReportProfileDetailRepository.Update(scaleReportProfileDetail);
    }
}
