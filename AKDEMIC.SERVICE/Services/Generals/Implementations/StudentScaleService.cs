using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class StudentScaleService : IStudentScaleService
    {
        private readonly IStudentScaleRepository _studentScaleRepository;

        public StudentScaleService(IStudentScaleRepository studentScaleRepository)
        {
            _studentScaleRepository = studentScaleRepository;
        }

        public async Task Delete(StudentScale studentScale)
            => await _studentScaleRepository.Delete(studentScale);

        public async Task DeleteById(Guid id)
            => await _studentScaleRepository.DeleteById(id);

        public async Task<StudentScale> Get(Guid id)
            => await _studentScaleRepository.Get(id);

        public async Task<IEnumerable<StudentScale>> GetAll()
            => await _studentScaleRepository.GetAll();

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _studentScaleRepository.GetDataDatatable(sentParameters, search);

        public async Task<object> GetStudentScalesSelect()
            => await _studentScaleRepository.GetStudentScalesSelect();

        public async Task Insert(StudentScale studentScale)
            => await _studentScaleRepository.Insert(studentScale);

        public async Task Update(StudentScale studentScale)
            => await _studentScaleRepository.Update(studentScale);
    }
}
