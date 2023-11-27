using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationComiteeRepository : IRepository<ConvocationComitee>
    {
        Task<ConvocationComitee> Get(Guid convocationId, string userId);
        Task<List<ConvocationComitee>> GetComitee(Guid convocationId);
    }
}
