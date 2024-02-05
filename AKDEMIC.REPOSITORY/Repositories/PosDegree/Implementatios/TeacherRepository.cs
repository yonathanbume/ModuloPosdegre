using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public  class TeacherRepository:Repository<PosdegreeTeacher>,ITeacherPRepository
    {
        public TeacherRepository(AkdemicContext context):base(context) { }

        public async  Task<object> GetDocenteAllJson()
        {
            var docentes = await _context.PosdegreeTeachers
                 .OrderBy(x => x.name)
                 .Select(f => new
                 {
                     id = f.id,
                     text = f.name
                 }).ToListAsync();

            docentes.Insert(0, new { id = new Guid(), text = "Todas" });

            return docentes;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.PosdegreeTeachers.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.name.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await query.CountAsync();

          
        var data = await query.Skip(parameters1.PagingFirstRecord)
                .Take(parameters1.RecordsPerDraw).Select(x => new {
                    x.id,
                    x.name,
                    x.PaternalSurName,
                    x.Maternalsurname,
                    x.Email,
                    x.PhoneNumber,
                    x.Departament,
                    x.Especiality
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
    }
}
