using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public  class PosdegreeStudentService:IPosdegreeStudentService
    { 
        public readonly IPosdegreeStudentRepository _PosdegreeStudentRepository;
        public PosdegreeStudentService(IPosdegreeStudentRepository PosdegreeStudentRepository)
        {
            _PosdegreeStudentRepository= PosdegreeStudentRepository;
        }

        public async Task Delete(Guid id)
        {
            var  Student=  await _PosdegreeStudentRepository.Get(id);
            await _PosdegreeStudentRepository.Delete(Student);
        }

        public  Task<DataTablesStructs.ReturnedData<object>> GetStudentDataTable(DataTablesStructs.SentParameters parameters1, string search)
           => _PosdegreeStudentRepository.GetStudentDataTable(parameters1, search);
        
      
        public async Task Insert(PosdegreeStudent entity)
        {
            await _PosdegreeStudentRepository.Insert(entity);
        }
        public async Task<PosdegreeStudent> Get(Guid id) {
            return await _PosdegreeStudentRepository.Get(id);
        }

    }
}
