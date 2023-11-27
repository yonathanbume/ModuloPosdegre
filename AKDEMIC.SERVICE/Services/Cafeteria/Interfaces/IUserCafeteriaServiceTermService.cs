using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IUserCafeteriaServiceTermService
    {
        Task<object> GetCafeteriaStudentsWithServiceForAssistanceDatatableClientSide();
        Task<bool> AnyTermActiveByStudent(Guid studentId);
        Task<UserCafeteriaServiceTerm> GetByTermActiveAndStudent(Guid studentId);
        Task Insert(UserCafeteriaServiceTerm userCafeteriaServiceTerm);
        Task Delete(UserCafeteriaServiceTerm userCafeteriaServiceTerm);
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceForAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? weekId, int dayId, int turn, string searchValue = null, Guid? facultyId = null, Guid? careerId = null);
        Task SaveStudentList(Guid? careerId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid);
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceByDateDatatable(DataTablesStructs.SentParameters sentParameters, DateTime date, int turn, string searchValue);
    }
}
