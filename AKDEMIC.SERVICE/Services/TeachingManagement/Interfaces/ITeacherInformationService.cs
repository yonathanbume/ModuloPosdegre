using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherInformationService
    {
        Task<TeacherInformation> GetAsync(Guid id);
    }
}