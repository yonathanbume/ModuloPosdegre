using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DeanFaculty;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DeanFacultyService : IDeanFacultyService
    {
        private readonly IDeanFacultyRepository _deanFacultyRepository;
        public DeanFacultyService(IDeanFacultyRepository deanFacultyRepository)
        {
            _deanFacultyRepository = deanFacultyRepository;
        }

        public async Task<DeanFaculty> Get(Guid id)
        {
            return await _deanFacultyRepository.Get(id);
        }

        public async Task<List<DeanFacultyTemplate>> GetByFaculty(Guid id)
        {
            return await _deanFacultyRepository.GetByFaculty(id);
        }

        public async Task Delete(DeanFaculty lastdean)
        {
            await _deanFacultyRepository.Delete(lastdean);
        }

        public async Task Insert(DeanFaculty entity)
            => await _deanFacultyRepository.Insert(entity);
    }
}
