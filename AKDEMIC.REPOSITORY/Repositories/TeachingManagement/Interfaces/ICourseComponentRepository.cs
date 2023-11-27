using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ICourseComponentRepository : IRepository<CourseComponent>
    {
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelB(Guid? id = null);
        Task<bool> AnyByNameAsync(string name, Guid? id = null);
        Task<object> GetCourseComponents();
        Task<object> GetAllAsSelect2ClientSide(bool? validate = null);
        Task<object> GetCourseComponentsJson();
        Task<int> AssignCourseComponentsJob();
        Task<DataTablesStructs.ReturnedData<object>> GetCourseComponentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
    }
}