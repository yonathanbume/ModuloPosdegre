using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class EmailManagementService : IEmailManagementService
    {
        private readonly IEmailManagementRepository _emailManagementRepository;

        public EmailManagementService(IEmailManagementRepository emailManagementRepository)
        {
            _emailManagementRepository = emailManagementRepository;
        }

        public async Task<bool> AnyBySytem(int system, Guid? ignoredId = null)
            => await _emailManagementRepository.AnyBySystem(system, ignoredId);

        public async Task Delete(EmailManagement entity)
            => await _emailManagementRepository.Delete(entity);

        public async Task<EmailManagement> Get(Guid id)
            => await _emailManagementRepository.Get(id);
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetEmailManagementDatatable(DataTablesStructs.SentParameters parameters, int? system)
            => await _emailManagementRepository.GetEmailManagementDatatable(parameters, system);

        public async Task Insert(EmailManagement entity)
            => await _emailManagementRepository.Insert(entity);

        public async Task Update(EmailManagement entity)
            => await _emailManagementRepository.Update(entity);
    }
}
