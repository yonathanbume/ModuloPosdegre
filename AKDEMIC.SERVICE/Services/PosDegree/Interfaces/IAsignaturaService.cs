using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Interfaces
{
    public  interface IAsignaturaService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAsignaturaDataTable(DataTablesStructs.SentParameters parameters1, string search);
        Task Insert(Asignatura entity);
        Task DeleteAsignatura(Guid id);
        Task<Asignatura> Get(Guid id);
    }
}
