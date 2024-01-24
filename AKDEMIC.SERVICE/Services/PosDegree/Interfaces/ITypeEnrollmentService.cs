using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Interfaces
{
    public  interface ITypeEnrollmentService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTypeEnrollmentDataTable(DataTablesStructs.SentParameters parameters1, string search);
        Task Insert(TypeEnrollment entity );
        Task DeleteTypeEnrollment(Guid id);
        Task<TypeEnrollment> Get(Guid id);

    }
}
