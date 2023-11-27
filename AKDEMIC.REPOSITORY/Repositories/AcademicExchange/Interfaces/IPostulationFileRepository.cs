using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IPostulationFileRepository : IRepository<PostulationFile>
    {
        Task<IEnumerable<PostulationFile>> GetAllByPostulationId(Guid postulationId);
    }
}
