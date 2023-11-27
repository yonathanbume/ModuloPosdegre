using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentPortfolioTypeService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task Insert(StudentPortfolioType studentPortfolioType);
        Task DeleteById(Guid id);
        Task<StudentPortfolioType> Get(Guid id);
        Task Update(StudentPortfolioType studentPortfolioType);
        Task<List<StudentPortfolioType>> GetStudentPortfolioTypes(byte? type, bool? canUploadStudent);
    }
}
