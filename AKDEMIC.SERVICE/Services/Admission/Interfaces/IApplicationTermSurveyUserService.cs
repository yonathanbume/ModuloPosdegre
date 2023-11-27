using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermSurveyUserService
    {
        Task Insert(ApplicationTermSurveyUser entity);
    }
}
