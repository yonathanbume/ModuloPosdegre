﻿using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkerTermPayrollDetailRepository : IRepository<WorkerTermPayrollDetail>
    {
        Task<bool> AnyByTerm(Guid workingTermId);
    }
}
