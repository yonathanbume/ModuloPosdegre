using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CareerEnrollmentShiftService : ICareerEnrollmentShiftService
    {
        private readonly ICareerEnrollmentShiftRepository _careerEnrollmentShiftRepository;

        public CareerEnrollmentShiftService(ICareerEnrollmentShiftRepository careerEnrollmentShiftRepository)
        {
            _careerEnrollmentShiftRepository = careerEnrollmentShiftRepository;
        }

        public async Task<CareerEnrollmentShift> Get(Guid id) => await _careerEnrollmentShiftRepository.Get(id);

        public async Task<CareerEnrollmentShift> Get(Guid termId, Guid careerId) => await _careerEnrollmentShiftRepository.Get(termId, careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerShiftDatatable(DataTablesStructs.SentParameters sentParameters, Guid enrollmentShiftId, ClaimsPrincipal user = null, string search = null)
            => await _careerEnrollmentShiftRepository.GetCareerShiftDatatable(sentParameters, enrollmentShiftId, user, search);

        public async Task<object> GetDataDatatableClientSide(Guid enrollmentShiftId, ClaimsPrincipal user = null) => await _careerEnrollmentShiftRepository.GetDataDatatableClientSide(enrollmentShiftId, user);

        public async Task Insert(CareerEnrollmentShift careerEnrollmentShift) => await _careerEnrollmentShiftRepository.Insert(careerEnrollmentShift);

        public async Task Update(CareerEnrollmentShift careerEnrollmentShift) => await _careerEnrollmentShiftRepository.Update(careerEnrollmentShift);

        public async Task UpdateInEnrollmentShift(Guid enrollmentShiftId, List<CareerEnrollmentShift> careerEnrollmentShifts)
        {
            var oldCareerEnrollmentShifts = await _careerEnrollmentShiftRepository.GetAllByEnrollmentShift(enrollmentShiftId);
            _careerEnrollmentShiftRepository.RemoveRange(oldCareerEnrollmentShifts);

            await _careerEnrollmentShiftRepository.InsertRange(careerEnrollmentShifts);
        }
    }
}
