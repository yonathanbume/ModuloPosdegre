using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraContractField;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraContractFieldService : IScaleExtraContractFieldService
    {
        private readonly IScaleExtraContractFieldRepository _scaleExtraContractFieldRepository;

        public ScaleExtraContractFieldService(IScaleExtraContractFieldRepository scaleExtraContractFieldRepository)
        {
            _scaleExtraContractFieldRepository = scaleExtraContractFieldRepository;
        }

        public async Task<ScaleExtraContractField> Get(Guid id)
        {
            return await _scaleExtraContractFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraContractField entity)
        {
            await _scaleExtraContractFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraContractField entity)
        {
            await _scaleExtraContractFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraContractField entity)
        {
            await _scaleExtraContractFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraContractField> GetScaleExtraContractFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraContractFieldRepository.GetScaleExtraContractFieldByResolutionId(resolutionId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetContractRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
            => await _scaleExtraContractFieldRepository.GetContractRecordByUser(sentParameters,userId,searchValue);

        public async Task<List<TeacherDataReportViewModel>> GetTeacherDataReportViewModel(Guid facultyId)
            => await _scaleExtraContractFieldRepository.GetTeacherDataReportViewModel(facultyId);

        public async Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId,string resolutionTypeName)
        {
            return await _scaleExtraContractFieldRepository.GetByScaleResolutionTypeAndUser(userId,resolutionTypeName);
        }

        public async Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, List<string> resolutionTypeName)
        {
            return await _scaleExtraContractFieldRepository.GetByScaleResolutionTypeAndUser(userId, resolutionTypeName);
        }

        public Task<WorkCertificateTemplate> GetContractCertificateData(Guid id)
            => _scaleExtraContractFieldRepository.GetContractCertificateData(id);
    }
}
