using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraContractField;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraContractFieldRepository : IRepository<ScaleExtraContractField>
    {
        Task<ScaleExtraContractField> GetScaleExtraContractFieldByResolutionId(Guid resolutionId);
        Task<WorkCertificateTemplate> GetContractCertificateData(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetContractRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<List<TeacherDataReportViewModel>> GetTeacherDataReportViewModel(Guid facultyId);
        Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);
        Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, List<string> resolutionTypeName);
    }
}
