using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IChannelCareerService
    {
        Task Delete(ChannelCareer channelCareer);
        Task Insert(ChannelCareer channelCareer);
        Task<ChannelCareer> Get(Guid channelId, Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetChannelCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid channelId);
        Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid? appTermAdmissionTypeId);
        Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid applicationTermId, Guid admissionTypeId);
        Task<object> GetAcademicProgramsSelect2ByChannelId(Guid id);
    }
}
