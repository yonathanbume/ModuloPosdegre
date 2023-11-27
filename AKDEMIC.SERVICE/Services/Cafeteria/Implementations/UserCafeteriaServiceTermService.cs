using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class UserCafeteriaServiceTermService : IUserCafeteriaServiceTermService
    {
        private readonly IUserCafeteriaServiceTermRepository _userCafeteriaServiceRepository;
        public UserCafeteriaServiceTermService(IUserCafeteriaServiceTermRepository userCafeteriaServiceRepository)
        {
            _userCafeteriaServiceRepository = userCafeteriaServiceRepository;
        }

        public async Task<bool> AnyTermActiveByStudent(Guid studentId)
        {
            return await _userCafeteriaServiceRepository.AnyTermActiveByStudent(studentId);
        }

        public async Task Delete(UserCafeteriaServiceTerm userCafeteriaServiceTerm)
        {
            await _userCafeteriaServiceRepository.Delete(userCafeteriaServiceTerm);
        }

        public async Task<UserCafeteriaServiceTerm> GetByTermActiveAndStudent(Guid studentId)
        {
            return await _userCafeteriaServiceRepository.GetByTermActiveAndStudent(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            return await _userCafeteriaServiceRepository.GetCafeteriaStudentsWithServiceDatatable(sentParameters, searchValue, facultyId, careerId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsWithServiceForAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            return await _userCafeteriaServiceRepository.GetCafeteriaStudentsWithServiceForAssistanceDatatable(sentParameters, searchValue, facultyId, careerId);
        }

        public async Task<object> GetCafeteriaStudentsWithServiceForAssistanceDatatableClientSide()
            => await _userCafeteriaServiceRepository.GetCafeteriaStudentsWithServiceForAssistanceDatatableClientSide();

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceByDateDatatable(DataTablesStructs.SentParameters sentParameters, DateTime date, int turn, string searchValue)
            => await _userCafeteriaServiceRepository.GetHistoricAssistanceByDateDatatable(sentParameters, date, turn, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? weekId, int dayId, int turn, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
        {
            return await _userCafeteriaServiceRepository.GetHistoricAssistanceDatatable(sentParameters, weekId, dayId,turn, searchValue, facultyId, careerId);
        }

        public async Task Insert(UserCafeteriaServiceTerm userCafeteriaServiceTerm)
        {
            await _userCafeteriaServiceRepository.Insert(userCafeteriaServiceTerm);
        }

        public async Task SaveStudentList(Guid? careerId, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            await _userCafeteriaServiceRepository.SaveStudentList( careerId,  isCheckAll, lstToAdd, lstToAvoid);
        }
    }
}
