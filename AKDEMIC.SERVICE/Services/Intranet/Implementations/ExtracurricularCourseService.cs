using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularCourseService : IExtracurricularCourseService
    {
        private readonly IExtracurricularCourseRepository _extracurricularCourseRepository;

        public ExtracurricularCourseService(IExtracurricularCourseRepository extracurricularCourseRepository)
        {
            _extracurricularCourseRepository = extracurricularCourseRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularCourseRepository.DeleteById(id);

        public Task<ExtracurricularCourse> Get(Guid id)
            => _extracurricularCourseRepository.Get(id);

        public Task<IEnumerable<ExtracurricularCourse>> GetAll()
            => _extracurricularCourseRepository.GetAll();

        public Task<ExtracurricularCourse> GetByCode(string code)
            => _extracurricularCourseRepository.GetByCode(code);

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _extracurricularCourseRepository.GetDataDatatable(sentParameters, search);

        public async Task<IEnumerable<Select2Structs.Result>> GetExtracurricularCoursesSelect2ClientSide()
            => await _extracurricularCourseRepository.GetExtracurricularCoursesSelect2ClientSide();

        public Task Insert(ExtracurricularCourse extracurricularCourse)
            => _extracurricularCourseRepository.Insert(extracurricularCourse);

        public Task Update(ExtracurricularCourse extracurricularCourse)
            => _extracurricularCourseRepository.Update(extracurricularCourse);
    }
}
