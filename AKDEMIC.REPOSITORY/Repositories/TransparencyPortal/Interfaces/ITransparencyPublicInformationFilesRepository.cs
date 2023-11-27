﻿using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyPublicInformationFilesRepository:IRepository<TransparencyPublicInformationFile>
    {
        Task<List<TransparencyPublicInformationFile>> GetByTransparencyPublicInformationId(Guid id);
    }
}
