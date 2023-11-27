using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationSectionService
    {
        Task<IEnumerable<ConvocationSection>> GetSectionsByConvocationId(Guid convocationId);
        Task<bool> AnyByTitle(Guid convocationId, string title, Guid? ignoredId = null);
        Task Insert(ConvocationSection entity);
        Task Delete(ConvocationSection entity);
        Task<ConvocationSection> Get(Guid id);
    }
}
