using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class UserAbsenceJustificationService : IUserAbsenceJustificationService
    {
        private readonly IUserAbsenceJustificationRepository _userAbsenceJustificationRepository;

        public UserAbsenceJustificationService(IUserAbsenceJustificationRepository userAbsenceJustificationRepository)
        {
            _userAbsenceJustificationRepository = userAbsenceJustificationRepository;
        }

        public async Task<bool> Any(Guid workingDayId, int? status = null)
            => await _userAbsenceJustificationRepository.Any(workingDayId, status);

        public async Task Delete(UserAbsenceJustification userAbsenceJustification)
            => await _userAbsenceJustificationRepository.Delete(userAbsenceJustification);

        public async Task DeleteById(Guid id)
            => await _userAbsenceJustificationRepository.DeleteById(id);

        public async Task<UserAbsenceJustification> Get(Guid id)
            => await _userAbsenceJustificationRepository.Get(id);

        public async Task<IEnumerable<UserAbsenceJustification>> GetAll(Guid? termId = null, string userId = null)
            => await _userAbsenceJustificationRepository.GetAll(termId, userId);

        public async Task Insert(UserAbsenceJustification userAbsenceJustification)
            => await _userAbsenceJustificationRepository.Insert(userAbsenceJustification);

        public async Task Update(UserAbsenceJustification userAbsenceJustification)
            => await _userAbsenceJustificationRepository.Update(userAbsenceJustification);
    }
}
