using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IChannelCareerRepository : IRepository<ChannelCareer>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetChannelCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid channelId);
        Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid? appTermAdmissionTypeId);
        Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid applicationTermId, Guid admissionTypeId);
        Task<object> GetAcademicProgramsSelect2ByChannelId(Guid id);
    }
}
