using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationCalendarService
    {
        Task Insert(ConvocationCalendar entity);
        Task Update(ConvocationCalendar entity);
        Task<ConvocationCalendar> Get(Guid id);
        Task Delete(ConvocationCalendar entity);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId);
    }
}
