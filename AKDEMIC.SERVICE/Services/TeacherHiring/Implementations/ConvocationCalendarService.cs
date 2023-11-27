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
    public class ConvocationCalendarService : IConvocationCalendarService
    {
        private readonly IConvocationCalendarRepository _convocationCalendarRepository;

        public ConvocationCalendarService(IConvocationCalendarRepository convocationCalendarRepository)
        {
            _convocationCalendarRepository = convocationCalendarRepository;
        }

        public async Task Delete(ConvocationCalendar entity)
            => await _convocationCalendarRepository.Delete(entity);

        public async Task<ConvocationCalendar> Get(Guid id)
            => await _convocationCalendarRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId)
            => await _convocationCalendarRepository.GetDatatable(parameters, convocationId);

        public async Task Insert(ConvocationCalendar entity)
            => await _convocationCalendarRepository.Insert(entity);

        public async Task Update(ConvocationCalendar entity)
            => await _convocationCalendarRepository.Update(entity);
    }
}
