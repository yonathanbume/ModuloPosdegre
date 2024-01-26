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
    public  class TeachingLoadService:ITeachingLoadService
    {
        public  readonly ITeachingLoagRepository _teachingLoadRepository;
      
        public TeachingLoadService(ITeachingLoagRepository teachingLoadRepository)
        {
            _teachingLoadRepository = teachingLoadRepository; 
           

        }
    }
}
