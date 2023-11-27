using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IPostulationInformationService
    {
        Task<PostulationInformation> Get(Guid id);
    }
}
