using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        public SchoolService(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task DeleteById(Guid id)
            => await _schoolRepository.DeleteById(id);

        public async Task<School> Get(Guid id)
            => await _schoolRepository.Get(id);

        public async Task<IEnumerable<School>> GetAll()
            => await _schoolRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string search = null)
            => await _schoolRepository.GetDatatable(sentParameters, type, search);

        public async Task<object> GetSelect2ClientSide()
            => await _schoolRepository.GetSelect2ClientSide();

        public async Task<object> GetSelect2ServerSide(string term, int? type = null, Guid? departmentId = null, Guid? provinceId = null)
            => await _schoolRepository.GetSelect2ServerSide(term, type, departmentId, provinceId);

        public async Task Insert(School school)
            => await _schoolRepository.Insert(school);

        public async Task InsertRange(List<School> schools)
            => await _schoolRepository.InsertRange(schools);

        public async Task Update(School school)
            => await _schoolRepository.Update(school);
    }
}
