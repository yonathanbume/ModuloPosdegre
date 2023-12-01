using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public class PosdegreeStudentRepository : Repository<PosdegreeStudentRepository>, IPosdegreeStudentRepository
    {
        public PosdegreeStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public Task<PosdegreeStudent> Add(PosdegreeStudent entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<PosdegreeStudent> entity)
        {
            throw new NotImplementedException();
        }

        public  async Task Delete(PosdegreeStudent entity)
        {
                _context.PosdegreeStudents.Remove(entity);
                await _context.SaveChangesAsync();

        }

        public Task DeleteRange(IEnumerable<PosdegreeStudent> entities)
        {
            throw new NotImplementedException();
        }

        public  async Task<DataTablesStructs.ReturnedData<object>> GetStudentDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.PosdegreeStudents.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await query.CountAsync();

            var data = await query.Skip(parameters1.PagingFirstRecord)
                .Take(parameters1.RecordsPerDraw).Select(x => new {
                    x.Id,
                    x.Codigo,
                    x.Dni,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Email,
                    x.PhoneNumber,
                    x.Address,
                
                }).ToListAsync();
            var recordTotal = data.Count();
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters1.DrawCounter,
                RecordsFiltered = recorFilter,
                RecordsTotal = recordTotal
            };
        }

        /*public  async Task Insert(PosdegreeStudent entity)
        {
            await _context.PosdegreeStudents.Add(entity);
           await _context.SaveChanges();
        }*/

        public virtual async Task Insert(PosdegreeStudent entity)
        {
            await _context.PosdegreeStudents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task InsertRange(IEnumerable<PosdegreeStudent> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(PosdegreeStudent entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<PosdegreeStudent> entities)
        {
            throw new NotImplementedException();
        }

        public Task Update(PosdegreeStudent entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(IEnumerable<PosdegreeStudent> entities)
        {
            throw new NotImplementedException();
        }

        Task<PosdegreeStudent> IRepository<PosdegreeStudent>.First()
        {
            throw new NotImplementedException();
        }

        async Task<PosdegreeStudent> IRepository<PosdegreeStudent>.Get(Guid id)
        {

            var entity =  _context.PosdegreeStudents.FindAsync(id);
            return await  entity;
           
        }

        Task<PosdegreeStudent> IRepository<PosdegreeStudent>.Get(string id)
        {
            throw new NotImplementedException();
        }
      
        
      
        Task<PosdegreeStudent> IRepository<PosdegreeStudent>.Get(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<PosdegreeStudent>> IRepository<PosdegreeStudent>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<PosdegreeStudent> IRepository<PosdegreeStudent>.Last()
        {
            throw new NotImplementedException();
        }
    }
}
