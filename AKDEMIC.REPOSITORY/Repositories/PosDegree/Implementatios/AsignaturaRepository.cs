using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public class AsignaturaRepository : Repository<Master>, IAsignaturaRepository
    {
        public AsignaturaRepository(AkdemicContext context) : base(context)
        {

        }

        public Task DownloadExcel(IXLWorksheet worksheet)
        {
            throw new NotImplementedException();
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Master entity)
        {
            throw new NotImplementedException();
        }

       
    }
}
