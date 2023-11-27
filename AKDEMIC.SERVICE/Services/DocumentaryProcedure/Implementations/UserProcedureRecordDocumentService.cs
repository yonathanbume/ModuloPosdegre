using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordDocumentService : IUserProcedureRecordDocumentService
    {
        private readonly IUserProcedureRecordDocumentRepository _userProcedureRecordDocumentRepository;

        public UserProcedureRecordDocumentService(IUserProcedureRecordDocumentRepository userProcedureRecordDocumentRepository)
        {
            _userProcedureRecordDocumentRepository = userProcedureRecordDocumentRepository;
        }

        public async Task InsertRange(IEnumerable<UserProcedureRecordDocument> documents)
        {
            await _userProcedureRecordDocumentRepository.InsertRange(documents);
        }

        public async Task<Tuple<int, List<UserProcedureRecordDocument>>> GetUserProcedureRecordDocuments(Guid userProcedureRecordId, DataTablesStructs.SentParameters sentParameters)
        {
            return await _userProcedureRecordDocumentRepository.GetUserProcedureRecordDocuments(userProcedureRecordId, sentParameters);
        }
    }
}
