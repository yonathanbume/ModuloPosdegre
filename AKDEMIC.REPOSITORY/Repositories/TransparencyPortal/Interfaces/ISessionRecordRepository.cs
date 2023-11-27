﻿using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ISessionRecordRepository : IRepository<SessionRecord>
    {
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<SessionRecord> GetBySlug(string slug);
    }
}
