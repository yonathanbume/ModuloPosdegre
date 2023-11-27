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
    public class ConvocationService : IConvocationService
    {
        private readonly IConvocationRepository _convocationRepository;

        public ConvocationService(IConvocationRepository convocationRepository)
        {
            _convocationRepository = convocationRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _convocationRepository.AnyByName(name, ignoredId);

        public async Task<Convocation> Get(Guid id)
            => await _convocationRepository.Get(id);
            

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters)
            => await _convocationRepository.GetDatatable(parameters);

        public async Task Insert(Convocation entity)
            => await _convocationRepository.Insert(entity);

        public async Task Update(Convocation entity)
            => await _convocationRepository.Update(entity);
    }
}
