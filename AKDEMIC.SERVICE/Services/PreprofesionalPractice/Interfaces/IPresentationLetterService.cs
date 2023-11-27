using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces
{
    public interface IPresentationLetterService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPresentationLettersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string search);
        Task Insert(PresentationLetter entity);
        Task Update(PresentationLetter entity);
        Task<PresentationLetter> Get(Guid id);
        Task Delete(PresentationLetter entity);
        Task<string> GetCode();
    }
}
