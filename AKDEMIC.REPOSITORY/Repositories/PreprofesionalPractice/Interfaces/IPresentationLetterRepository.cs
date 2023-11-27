using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces
{
    public interface IPresentationLetterRepository : IRepository<PresentationLetter>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPresentationLettersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search);
        Task<string> GetCode();
    }
}
