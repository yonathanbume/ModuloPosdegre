using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class DigitalDocumentService : IDigitalDocumentService
    {
        private readonly IDigitalDocumentRepository _digitalDocumentRepository;
        public DigitalDocumentService(IDigitalDocumentRepository digitalDocumentRepository)
        {
            _digitalDocumentRepository = digitalDocumentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDigitalDocumentToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId)
            => await _digitalDocumentRepository.GetDigitalDocumentToTeacherDatatable(parameters, teacherId);

        Task IDigitalDocumentService.DeleteAsync(DigitalDocument digitalDocument)
            => _digitalDocumentRepository.Delete(digitalDocument);

        Task<object> IDigitalDocumentService.GetAllAsModelA()
            => _digitalDocumentRepository.GetAllAsModelA();

        Task<object> IDigitalDocumentService.GetAsModelB(Guid? id, string userToVerify)
            => _digitalDocumentRepository.GetAsModelB(id, userToVerify);

        Task<DigitalDocument> IDigitalDocumentService.GetAsync(Guid id)
            => _digitalDocumentRepository.Get(id);

        Task IDigitalDocumentService.InsertAsync(DigitalDocument digitalDocument)
            => _digitalDocumentRepository.Insert(digitalDocument);

        Task IDigitalDocumentService.UpdateAsync(DigitalDocument digitalDocument)
            => _digitalDocumentRepository.Update(digitalDocument);
    }
}