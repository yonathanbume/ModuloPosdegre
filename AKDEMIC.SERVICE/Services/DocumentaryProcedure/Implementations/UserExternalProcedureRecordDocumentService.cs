using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserExternalProcedureRecordDocumentService : IUserExternalProcedureRecordDocumentService
    {
        private readonly IUserExternalProcedureRecordDocumentRepository _userExternalProcedureRecordDocumentRepository;

        public UserExternalProcedureRecordDocumentService(IUserExternalProcedureRecordDocumentRepository userExternalProcedureRecordDocumentRepository)
        {
            _userExternalProcedureRecordDocumentRepository = userExternalProcedureRecordDocumentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _userExternalProcedureRecordDocumentRepository.GetUserExternalProcedureRecordDocumentsDatatable(sentParameters, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserExternalProcedureRecordDocumentsDatatableByuserExternalProcedureRecord(DataTablesStructs.SentParameters sentParameters, Guid userExternalProcedureRecordId)
            => await _userExternalProcedureRecordDocumentRepository.GetUserExternalProcedureRecordDocumentsDatatableByuserExternalProcedureRecord(sentParameters, userExternalProcedureRecordId);
        public async Task<List<UserExternalProcedureRecordDocument>> GetUserExternalProcedureRecordDocumentsByuserExternalProcedureRecord(Guid userExternalProcedureRecordId)
            => await _userExternalProcedureRecordDocumentRepository.GetUserExternalProcedureRecordDocumentsByuserExternalProcedureRecord(userExternalProcedureRecordId);
        public async Task Delete(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument)
        {
            await _userExternalProcedureRecordDocumentRepository.Delete(userExternalProcedureRecordDocument);
        }

        public async Task Insert(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument)
        {
            await _userExternalProcedureRecordDocumentRepository.Insert(userExternalProcedureRecordDocument);
        }

        public async Task InsertRange(IEnumerable<UserExternalProcedureRecordDocument> userExternalProcedureRecordDocuments)
        {
            await _userExternalProcedureRecordDocumentRepository.InsertRange(userExternalProcedureRecordDocuments);
        }

        public async Task Update(UserExternalProcedureRecordDocument userExternalProcedureRecordDocument)
        {
            await _userExternalProcedureRecordDocumentRepository.Update(userExternalProcedureRecordDocument);
        }






















        public async Task<Tuple<int, List<UserExternalProcedureRecordDocument>>> GetUserExternalProcedureRecordDocuments(Guid userExternalProcedureRecordId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userExternalProcedureRecordDocumentRepository.GetUserExternalProcedureRecordDocuments(userExternalProcedureRecordId, sentParameters);
        }
    }
}
