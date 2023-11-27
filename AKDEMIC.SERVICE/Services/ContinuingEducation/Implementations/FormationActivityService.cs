using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.SERVICE.Services.ContinuingEducation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ContinuingEducation.Implementations
{
    public class FormationActivityService: IFormationActivityService
    {
        private readonly IFormationActivityRepository _formationActivityRepository;
        public FormationActivityService(IFormationActivityRepository formationActivityRepository)
        {
            _formationActivityRepository = formationActivityRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _formationActivityRepository.AnyByName(name, id);

        public Task Delete(ENTITIES.Models.ContinuingEducation.Activity formationActivity)
            => _formationActivityRepository.Delete(formationActivity);

        public Task<ENTITIES.Models.ContinuingEducation.Activity> Get(Guid id)
            => _formationActivityRepository.Get(id);

        public Task<IEnumerable<ENTITIES.Models.ContinuingEducation.Activity>> GetAll()
            => _formationActivityRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFormationActivitiesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _formationActivityRepository.GetAllFormationActivitiesDatatable(sentParameters, searchValue);

        public Task Insert(ENTITIES.Models.ContinuingEducation.Activity formationActivity)
            => _formationActivityRepository.Insert(formationActivity);

        public Task Update(ENTITIES.Models.ContinuingEducation.Activity formationActivity)
            => _formationActivityRepository.Update(formationActivity);
    }
}
