using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationComiteeService
    {
        Task<ConvocationComitee> Get(Guid convocationId, string userId);
        Task<List<ConvocationComitee>> GetComitee(Guid convocationId);
        Task Delete(ConvocationComitee entity);
        Task Insert(ConvocationComitee entity);
    }
}
