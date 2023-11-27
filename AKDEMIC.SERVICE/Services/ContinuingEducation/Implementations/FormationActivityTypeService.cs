using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ContinuingEducation;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationActivityTypeService : IFormationActivityTypeService
    {
        private readonly IFormationActivityTypeRepository _formationActivityTypeRepository;
        public FormationActivityTypeService(IFormationActivityTypeRepository formationActivityTypeRepository)
        {
            _formationActivityTypeRepository = formationActivityTypeRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _formationActivityTypeRepository.AnyByName(name, id);

        public Task Delete(ActivityType formationActivityType)
            => _formationActivityTypeRepository.Delete(formationActivityType);

        public Task<ActivityType> Get(Guid id)
            => _formationActivityTypeRepository.Get(id);

        public Task<IEnumerable<ActivityType>> GetAll()
            => _formationActivityTypeRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationActivitiesTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationActivityTypeRepository.GetAllFormationActivitiesTypeDatatable(sentParameters, searchValue);

        public Task Insert(ActivityType formationActivityType)
            => _formationActivityTypeRepository.Insert(formationActivityType);

        public Task Update(ActivityType formationActivityType)
            => _formationActivityTypeRepository.Update(formationActivityType);
    }
}
