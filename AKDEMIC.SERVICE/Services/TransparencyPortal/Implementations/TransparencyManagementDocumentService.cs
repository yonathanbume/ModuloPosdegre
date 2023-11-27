using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyManagementDocumentService : ITransparencyManagementDocumentService
    {
        private readonly ITransparencyManagementDocumentRepository _transparencyManagementDocumentRepository;

        public TransparencyManagementDocumentService(
            ITransparencyManagementDocumentRepository transparencyManagementDocumentRepository
            )
        {
            _transparencyManagementDocumentRepository = transparencyManagementDocumentRepository;
        }

        public async Task Delete(TransparencyManagementDocument entity)
            => await _transparencyManagementDocumentRepository.Delete(entity);

        public async Task<TransparencyManagementDocument> Get(Guid id)
            => await _transparencyManagementDocumentRepository.Get(id);

        public async Task<IEnumerable<TransparencyManagementDocument>> GetAll()
            => await _transparencyManagementDocumentRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters)
            => await _transparencyManagementDocumentRepository.GetDatatable(parameters);

        public async Task Insert(TransparencyManagementDocument entity)
            => await _transparencyManagementDocumentRepository.Insert(entity);

        public async Task Update(TransparencyManagementDocument entity)
            => await _transparencyManagementDocumentRepository.Update(entity);
    }
}
