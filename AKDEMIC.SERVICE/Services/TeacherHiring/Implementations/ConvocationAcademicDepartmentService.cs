using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Implementations
{
    public class ConvocationAcademicDepartmentService : IConvocationAcademicDepartmentService
    {
        private readonly IConvocationAcademicDepartmentRepository _convocationAcademicDepartmentRepository;

        public ConvocationAcademicDepartmentService(IConvocationAcademicDepartmentRepository convocationAcademicDepartmentRepository)
        {
            _convocationAcademicDepartmentRepository = convocationAcademicDepartmentRepository;
        }

        public async Task<bool> AnyByAcademicDepartmentId(Guid convocationId, Guid academicDepartmentId, Guid? ignoreId = null)
            => await _convocationAcademicDepartmentRepository.AnyByAcademicDepartmentId(convocationId, academicDepartmentId, ignoreId);

        public async Task Delete(ConvocationAcademicDeparment entity)
            => await _convocationAcademicDepartmentRepository.Delete(entity);

        public async Task<ConvocationAcademicDeparment> Get(Guid id)
            => await _convocationAcademicDepartmentRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, string search)
            => await _convocationAcademicDepartmentRepository.GetDatatable(parameters, convocationId, search);

        public async Task Insert(ConvocationAcademicDeparment entity)
            => await _convocationAcademicDepartmentRepository.Insert(entity);

        public async Task Update(ConvocationAcademicDeparment entity)
            => await _convocationAcademicDepartmentRepository.Update(entity);
    }
}
