using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraContractField;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraContractFieldService
    {
        Task<ScaleExtraContractField> Get(Guid id);
        Task Insert(ScaleExtraContractField entity);
        Task Update(ScaleExtraContractField entity);
        Task Delete(ScaleExtraContractField entity);
        Task<ScaleExtraContractField> GetScaleExtraContractFieldByResolutionId(Guid resolutionId);
        Task<WorkCertificateTemplate> GetContractCertificateData(Guid id);
        Task<List<TeacherDataReportViewModel>> GetTeacherDataReportViewModel(Guid facultyId);
        Task<DataTablesStructs.ReturnedData<object>> GetContractRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null );
        Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName);
        Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, List<string> resolutionTypeName);
    }
}
