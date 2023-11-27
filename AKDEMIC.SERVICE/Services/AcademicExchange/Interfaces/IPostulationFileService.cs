using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IPostulationFileService
    {
        Task<IEnumerable<PostulationFile>> GetAllByPostulationId(Guid postulationId);
        Task Insert(PostulationFile entity);
        Task Delete(PostulationFile entity);
        Task<PostulationFile> Get(Guid id);
    }
}
