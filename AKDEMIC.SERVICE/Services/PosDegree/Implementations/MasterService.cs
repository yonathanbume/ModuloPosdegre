using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using Amazon.S3.Model.Internal.MarshallTransformations;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public class MasterService:IMasterService
    {
        public readonly IMasterRepository _masterRepository;
        public MasterService(IMasterRepository masterRepository) { 
            _masterRepository= masterRepository;
        }

        public async  Task Delete(Master entity)
        {
            await _masterRepository.Delete(entity);
        }

		public Task<List<Master>> GetAllMaster()
		{
			return _masterRepository.GetAll();
		}
       

        public Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search)
            =>_masterRepository.GetMasterDataTable(parameters1, search);
        

        public  async Task Insert(Master entity)
        {
            await _masterRepository.Insert(entity);
        }

        public async Task Update(Master entity)
        {
            await _masterRepository.Update(entity);
        }

        public async Task DownloadExcel(IXLWorksheet worksheet)
        {
            await _masterRepository.DownloadExcel(worksheet);
        }

        public async Task DeleteMaster(Guid id)
        {
            var maestria = await _masterRepository.Get(id);
            await _masterRepository.Delete(maestria);
        }
       

        public async Task<Master> Get(Guid id)
        {
             return await _masterRepository.Get(id);
        }        
    }
}
