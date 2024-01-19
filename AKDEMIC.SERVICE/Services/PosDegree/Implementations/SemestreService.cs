using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public class SemestreService : ISemestreService
    {  private readonly   ISemestreRepository _semestreRepository;
        public SemestreService(ISemestreRepository semestreRepository) {
            _semestreRepository= semestreRepository;
        }

        public async Task DeleteSemestre(Guid id)
        {
            var semestre = await _semestreRepository.Get(id);
            await _semestreRepository.Delete(semestre);
        }

        public async Task<Semestre> Get(Guid id)
        {
           return await _semestreRepository.Get(id);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetSemestreDataTable(DataTablesStructs.SentParameters parameters1, string search)
        => _semestreRepository.GetSemestreDataTable(parameters1,search);

        public async  Task Insert(Semestre entity)
        {
          await _semestreRepository.Insert(entity);
        }
    }
}
