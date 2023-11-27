using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ICourseComponentService
    {
        Task<object> GetAllAsModelA();
        Task<object> GetAsModelB(Guid? id = null);
        Task<bool> AnyByNameAsync(string name, Guid? id = null);
        Task<object> GetCourseComponents();
        Task<CourseComponent> GetAsync(Guid id);
        Task InsertAsync(CourseComponent courseComponent);
        Task UpdateAsync(CourseComponent courseComponent);
        Task DeleteAsync(CourseComponent courseComponent);
        Task<object> GetAllAsSelect2ClientSide(bool? validate = null);
        Task<object> GetCourseComponentsJson();
        Task<int> AssignCourseComponentsJob();
        Task<DataTablesStructs.ReturnedData<object>> GetCourseComponentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
    }
}