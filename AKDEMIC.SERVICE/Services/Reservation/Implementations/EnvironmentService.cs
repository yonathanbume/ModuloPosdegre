using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using AKDEMIC.SERVICE.Services.Reservation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IEnvironmentRepository _environmentRepository;

        public EnvironmentService(IEnvironmentRepository environmentRepository)
        {
            _environmentRepository = environmentRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _environmentRepository.AnyByCode(code, ignoredId);

        public async Task Delete(ENTITIES.Models.Reservations.Environment entity)
            => await _environmentRepository.Delete(entity);

        public async Task<ENTITIES.Models.Reservations.Environment> Get(Guid id)
            => await _environmentRepository.Get(id);

        public async Task<IEnumerable<ENTITIES.Models.Reservations.Environment>> GetAll()
            => await _environmentRepository.GetAll();

        public async Task<object> GetAllByFilters(bool? isActive = null, bool? reservationExternal = null)
            => await _environmentRepository.GetAllByFilters(isActive, reservationExternal);

        public async Task<IEnumerable<Select2Structs.Result>> GetEnvironmentsSelect2ClientSide()
            => await _environmentRepository.GetEnvironmentsSelect2ClientSide();

        public async Task Insert(ENTITIES.Models.Reservations.Environment entity)
            => await _environmentRepository.Insert(entity);

        public async Task Update(ENTITIES.Models.Reservations.Environment entity)
            => await _environmentRepository.Update(entity);
    }
}
