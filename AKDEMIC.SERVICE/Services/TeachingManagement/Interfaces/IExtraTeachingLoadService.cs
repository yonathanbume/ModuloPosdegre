using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IExtraTeachingLoadService
    {
        Task<ExtraTeachingLoad> GetExtraTeachingLoad(Guid termId, string teacherId);
        Task Insert(ExtraTeachingLoad entity);
        Task Update(ExtraTeachingLoad entity);
    }
}
