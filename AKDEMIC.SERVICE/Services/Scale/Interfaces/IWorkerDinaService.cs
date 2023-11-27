using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerDinaService
    {
        Task<WorkerDina> GetByUserId(string userId);
        Task Insert(WorkerDina entity);
        Task Update(WorkerDina entity);
        Task<int> GetTeacherInDinaCount();
    }
}
