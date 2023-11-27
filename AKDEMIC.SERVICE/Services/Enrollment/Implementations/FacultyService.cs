using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Faculty;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;

        public FacultyService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public async Task<Faculty> Get(Guid id)
        {
            return await _facultyRepository.Get(id);
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            return await _facultyRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<Faculty>> GetFacultiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _facultyRepository.GetFacultiesDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultiesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _facultyRepository.GetFacultiesSelect2(requestParameters, searchValue);
        }

        public async Task Delete(Faculty faculty)
        {
            await _facultyRepository.Delete(faculty);
        }

        public async Task Insert(Faculty faculty)
        {
            await _facultyRepository.Insert(faculty);
        }

        public async Task Update(Faculty faculty)
        {
            await _facultyRepository.Update(faculty);
        }

        public async Task<object> GetFacultiesSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null)
        {
            return await _facultyRepository.GetFacultiesSelect2ClientSide(hasAll, user);
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultiesByAcademicCoordinatorSelect2(Select2Structs.RequestParameters requestParameters, string academicCoordinatorId, string searchValue = null)
        {
            return await _facultyRepository.GetFacultiesByAcademicCoordinatorSelect2(requestParameters, academicCoordinatorId, searchValue);
        }

        public async Task<Faculty> GetFacultyByCareerId(Guid careerId)
        {
            return await _facultyRepository.GetFacultyByCareerId(careerId);
        }

        public Task<object> GetAllAsSelect2ClientSide(bool hasAll = false, ClaimsPrincipal user = null)
        {
            return _facultyRepository.GetAllAsSelect2ClientSide(hasAll, user);
        }

        public async Task<object> GetFaculties3()
        {
            return await _facultyRepository.GetFaculties3();
        }

        public async Task<object> GetFaculty(Guid id)
        {
            return await _facultyRepository.GetFaculty(id);
        }

        public Task<object> GetAllAsSelect2ClientSide2(bool includeTitle = false)
        {
            return _facultyRepository.GetAllAsSelect2ClientSide2(includeTitle);
        }

        public async Task<ModelFacultyTemplate> GetFacultiesAdmitted()
        {
            return await _facultyRepository.GetFacultiesAdmitted();
        }

        public async Task<object> GetFacultiesJson(string q, ClaimsPrincipal user = null)
        {
            return await _facultyRepository.GetFacultiesJson(q, user);
        }

        public async Task<object> GetFacultiesAllJson()
        {
            return await _facultyRepository.GetFacultiesAllJson();
        }

        public async Task<List<Faculty>> GetAllWithCareers()
        {
            return await _facultyRepository.GetAllWithCareers();
        }

        public async Task InsertRange(Faculty[] listFaculty)
        {
            await _facultyRepository.InsertRange(listFaculty);
        }
        public async Task<string> GetDeanById(Guid facultyId)
        {
            return await _facultyRepository.GetDeanById(facultyId);
        }

        public async Task<Faculty> GetWithHistory(Guid id)
        {
            return await _facultyRepository.GetWithHistory(id);
        }

        public async Task<object> GetFacultyIncomesDatatable(DataTablesStructs.SentParameters sentParameters, int? year = null, int? type = null)
            => await _facultyRepository.GetFacultyIncomesDatatable(sentParameters, year, type);

    }
}