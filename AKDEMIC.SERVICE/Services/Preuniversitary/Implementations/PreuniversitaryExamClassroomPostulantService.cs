using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryExamClassroomPostulantService : IPreuniversitaryExamClassroomPostulantService
    {
        private readonly IPreuniversitaryExamClassroomPostulantRepository _preuniversitaryExamClassroomPostulantRepository;

        public PreuniversitaryExamClassroomPostulantService(IPreuniversitaryExamClassroomPostulantRepository preuniversitaryExamClassroomPostulantRepository)
        {
            _preuniversitaryExamClassroomPostulantRepository = preuniversitaryExamClassroomPostulantRepository;
        }

        public async Task<Tuple<bool, string>> AssignPostulantsRandomly(Guid preuniversitaryExamId)
            => await _preuniversitaryExamClassroomPostulantRepository.AssignPostulantsRandomly(preuniversitaryExamId);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
            => await _preuniversitaryExamClassroomPostulantRepository.GetDatatable(sentParameters, preuniversitaryExamId);
    }
}
