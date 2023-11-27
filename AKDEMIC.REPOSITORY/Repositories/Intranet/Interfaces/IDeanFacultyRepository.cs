using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DeanFaculty;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDeanFacultyRepository: IRepository<DeanFaculty>
    {
        Task<List<DeanFacultyTemplate>> GetByFaculty(Guid id);
    }
}
