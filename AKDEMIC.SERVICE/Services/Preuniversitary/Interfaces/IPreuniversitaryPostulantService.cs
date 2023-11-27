using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryPostulantService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue);
        Task<object> GetGradesByStudent(Guid pid);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPaymentDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid guid);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable_V2(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantFileDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantPreRegistersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<string> UsersWithCodeExist(string userCodePrefix);
        Task Insert(PreuniversitaryPostulant entity);
        Task<PreuniversitaryPostulant> Get(Guid id);
        Task Update(PreuniversitaryPostulant entity);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAdmittedPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null);
        Task<List<PostulantTemplate>> GetPostulantsReportData(Guid termId, string searchValue = null, bool? onlyPaid = null);
    }
}
