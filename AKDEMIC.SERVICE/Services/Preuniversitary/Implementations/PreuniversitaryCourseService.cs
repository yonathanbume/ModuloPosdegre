using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryCourseService : IPreuniversitaryCourseService
    {
        private readonly IPreuniversitaryCourseRepository _preuniversitaryCourseRepository;

        public PreuniversitaryCourseService(IPreuniversitaryCourseRepository preuniversitaryCourseRepository)
        {
            _preuniversitaryCourseRepository = preuniversitaryCourseRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _preuniversitaryCourseRepository.AnyByCode(code, ignoredId);

        public async Task Delete(PreuniversitaryCourse entity)
            => await _preuniversitaryCourseRepository.Delete(entity);

        public async Task<PreuniversitaryCourse> Get(Guid id)
            => await _preuniversitaryCourseRepository.Get(id);

        public async Task<List<PreuniversitaryCourse>> GetAllByCareerId(Guid careerId)
            => await _preuniversitaryCourseRepository.GetAllByCareerId(careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _preuniversitaryCourseRepository.GetCoursesDatatable(sentParameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesToScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchvalue = null)
            => await _preuniversitaryCourseRepository.GetCoursesToScheduleDatatable(sentParameters, preuniversitaryTermId, searchvalue);

        public async Task Insert(PreuniversitaryCourse entity)
            => await _preuniversitaryCourseRepository.Insert(entity);

        public async Task Update(PreuniversitaryCourse entity)
            => await _preuniversitaryCourseRepository.Update(entity);
    }
}
