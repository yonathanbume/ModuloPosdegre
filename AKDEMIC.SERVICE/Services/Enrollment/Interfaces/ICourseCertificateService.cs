using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseCertificateService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task Insert(CourseCertificate courseCertificate);
        Task<CourseCertificate> Get(Guid id);
        Task<IEnumerable<CourseCertificate>> GetAll();
        Task Update(CourseCertificate courseCertificate);
        Task DeleteById(Guid id);
        Task<object> GetSelect2ClientSide();
    }
}
