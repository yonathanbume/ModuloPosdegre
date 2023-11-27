using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class CourseComponentService : ICourseComponentService
    {
        private readonly ICourseComponentRepository _courseComponentRepository;

        public CourseComponentService(ICourseComponentRepository courseComponentRepository)
        {
            _courseComponentRepository = courseComponentRepository;
        }

        Task<bool> ICourseComponentService.AnyByNameAsync(string name, Guid? id)
            => _courseComponentRepository.AnyByNameAsync(name, id);

        Task ICourseComponentService.DeleteAsync(CourseComponent courseComponent)
            => _courseComponentRepository.Delete(courseComponent);

        Task<object> ICourseComponentService.GetAllAsModelA()
            => _courseComponentRepository.GetAllAsModelA();

        Task<object> ICourseComponentService.GetAllAsSelect2ClientSide(bool? validate = null)
            => _courseComponentRepository.GetAllAsSelect2ClientSide(validate);

        Task<object> ICourseComponentService.GetAsModelB(Guid? id)
            => _courseComponentRepository.GetAsModelB(id);

        Task<CourseComponent> ICourseComponentService.GetAsync(Guid id)
            => _courseComponentRepository.Get(id);

        Task<object> ICourseComponentService.GetCourseComponents()
            => _courseComponentRepository.GetCourseComponents();

        Task ICourseComponentService.InsertAsync(CourseComponent courseComponent)
            => _courseComponentRepository.Insert(courseComponent);

        Task ICourseComponentService.UpdateAsync(CourseComponent courseComponent)
            => _courseComponentRepository.Update(courseComponent);

        public async Task<object> GetCourseComponentsJson()
            => await _courseComponentRepository.GetCourseComponentsJson();

        public async Task<int> AssignCourseComponentsJob()
        {
            return await _courseComponentRepository.AssignCourseComponentsJob();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseComponentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _courseComponentRepository.GetCourseComponentsDatatable(sentParameters, searchValue);
    }
}