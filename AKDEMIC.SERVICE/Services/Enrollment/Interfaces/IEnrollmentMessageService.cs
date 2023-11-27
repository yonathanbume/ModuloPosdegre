using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentMessageService
    {
        Task Insert(EnrollmentMessage message);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters,Guid careerId, string search);
        Task<EnrollmentMessage> Get(Guid id);
        Task Update(EnrollmentMessage enrollment);
    }
}
