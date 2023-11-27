using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DeanFaculty;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDeanFacultyService
    {
        Task<List<DeanFacultyTemplate>> GetByFaculty(Guid id);
        Task<DeanFaculty> Get(Guid id);
        Task Delete(DeanFaculty lastdean);
        Task Insert(DeanFaculty entity); 
    }
}
