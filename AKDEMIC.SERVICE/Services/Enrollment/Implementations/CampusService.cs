using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;

        public CampusService(ICampusRepository campusRepository)
        {
            _campusRepository = campusRepository;
        }

        public async Task<Select2Structs.ResponseParameters> GetAllSelect2(Select2Structs.RequestParameters requestParameters)
        {
            return await _campusRepository.GetAllSelect2(requestParameters);
        }

        Task ICampusService.DeleteById(Guid id)
        {
            return _campusRepository.DeleteById(id);
        }

        Task<Campus> ICampusService.Get(Guid id)
        {
            return _campusRepository.Get(id);
        }

        Task<IEnumerable<Campus>> ICampusService.GetAll()
        {
            return _campusRepository.GetAll();
        }

        Task<object> ICampusService.GetAllAsSelect2ClientSide()
        {
            return _campusRepository.GetAllAsSelect2ClientSide();
        }

        Task<object> ICampusService.GetCampus(Guid id)
        {
            return _campusRepository.GetCampus(id);
        }

        public async Task ClearPrincipal()
        {
            await _campusRepository.ClearPrincipal();
        }

        Task<DataTablesStructs.ReturnedData<object>> ICampusService.GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            return _campusRepository.GetDataDatatable(sentParameters, searchValue);
        }

        Task ICampusService.Insert(Campus campus)
        {
            return _campusRepository.Insert(campus);
        }

        Task ICampusService.Update(Campus campus)
        {
            return _campusRepository.Update(campus);
        }

        public async Task<object> GetCampusJson()
        {
            return await _campusRepository.GetCampusJson();
        }

        public async Task<object> GetCampusCareerJson(Guid cid)
        {
            return await _campusRepository.GetCampusCareerJson(cid);
        }

        public async Task<Campus> GetFirstCampus()
        {
            return await _campusRepository.GetFirstCampus();
        }

        public async Task<Campus> GetCampusPrincipal()
        {
            return await _campusRepository.GetCampusPrincipal();
        }

        public async Task<bool> AnyClassroom(Guid id)
            => await _campusRepository.AnyClassroom(id);
    }
}