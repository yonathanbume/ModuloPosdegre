using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public class TypeEnrollmentService: ITypeEnrollmentService
    {
        public readonly ITypeEnrollmentRepository _typeEnrollmentRepository;

        public TypeEnrollmentService(ITypeEnrollmentRepository typeEnrollmentRepository)
        {
            _typeEnrollmentRepository = typeEnrollmentRepository;
        }

        public async Task DeleteTypeEnrollment(Guid id)
        {
            var typeEnrollment = await _typeEnrollmentRepository.Get(id);
            await _typeEnrollmentRepository.Delete(typeEnrollment);
        }

        public async  Task<TypeEnrollment> Get(Guid id)
        {
            return await _typeEnrollmentRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTypeEnrollmentDataTable(DataTablesStructs.SentParameters parameters1, string search)
           => await _typeEnrollmentRepository.GetTypeEnrollmentDataTable(parameters1, search);

        public async Task Insert(TypeEnrollment entity)
        {
            await _typeEnrollmentRepository.Insert(entity);
        }
    }
}
