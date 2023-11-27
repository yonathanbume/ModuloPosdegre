﻿using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICurriculumEquivalenceService
    {
        Task<CurriculumEquivalence> GetByNewCurriculumId(Guid curriculumId);
        Task Delete(CurriculumEquivalence curriculumEquivalence);
        Task Insert(CurriculumEquivalence curriculumEquivalence);
    }
}
