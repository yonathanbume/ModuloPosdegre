using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentPortfolioTypeService : IStudentPortfolioTypeService
    {
        private readonly IStudentPortfolioTypeRepository _studentPortfolioTypeRepository;

        public StudentPortfolioTypeService(IStudentPortfolioTypeRepository studentPortfolioTypeRepository)
        {
            _studentPortfolioTypeRepository = studentPortfolioTypeRepository;
        }

        public async Task DeleteById(Guid id)
            => await _studentPortfolioTypeRepository.DeleteById(id);

        public async Task<StudentPortfolioType> Get(Guid id)
            => await _studentPortfolioTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _studentPortfolioTypeRepository.GetDataDatatable(sentParameters, search);

        public async Task<List<StudentPortfolioType>> GetStudentPortfolioTypes(byte? type, bool? canUploadStudent)
            => await _studentPortfolioTypeRepository.GetStudentPortfolioTypes(type, canUploadStudent);

        public async Task Insert(StudentPortfolioType studentPortfolioType)
            => await _studentPortfolioTypeRepository.Insert(studentPortfolioType);

        public async Task Update(StudentPortfolioType studentPortfolioType)
            => await _studentPortfolioTypeRepository.Update(studentPortfolioType);
    }
}
