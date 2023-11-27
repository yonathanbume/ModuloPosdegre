using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryExamClassroomPostulantRepository : Repository<PreuniversitaryExamClassroomPostulant>, IPreuniversitaryExamClassroomPostulantRepository
    {
        public PreuniversitaryExamClassroomPostulantRepository(AkdemicContext context) : base(context) { }

        public async Task<Tuple<bool,string>> AssignPostulantsRandomly(Guid preuniversitaryExamId)
        {

            var preuniversitaryExam = await _context.PreuniversitaryExams.Where(x => x.Id == preuniversitaryExamId).FirstOrDefaultAsync();

            var classrooms = await _context.PreuniversitaryExamClassrooms.Where(x => x.PreuniversitaryExamId == preuniversitaryExamId).ToListAsync();

            var postulants = await _context.PreuniversitaryPostulants
                .Where(x => x.IsPaid && x.PreuniversitaryTermId == preuniversitaryExam.PreuniversitaryTermId)
                .OrderBy(x=>x.FullName)
                .ToListAsync();

            var examClassroomPostulants = await _context.PreuniversitaryExamClassroomPostulants
                .Where(x => x.PreuniversitaryExamClassroom.PreuniversitaryExamId == preuniversitaryExamId).ToListAsync();

            _context.PreuniversitaryExamClassroomPostulants.RemoveRange(examClassroomPostulants);

            if (classrooms.Sum(x => x.Vacancies) < postulants.Count())
                return new Tuple<bool, string>(false, "No cuenta con la capacidad suficiente para todos los postulantes.");

            var toAdd = new List<PreuniversitaryExamClassroomPostulant>();
            var skip = 0;
            foreach (var classroom in classrooms)
            {
                var seat = 1;
                foreach (var postulantClassroom in postulants.Skip(skip).Take(classroom.Vacancies))
                {
                    toAdd.Add(new PreuniversitaryExamClassroomPostulant
                    {
                        Seat = seat,
                        PreuniversitaryExamClassroomId = classroom.Id,
                        PreuniversitaryPostulantId = postulantClassroom.Id
                    });
                    seat++;
                }
                skip += classroom.Vacancies;
            }

            await _context.PreuniversitaryExamClassroomPostulants.AddRangeAsync(toAdd);
            await _context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "");
        }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId)
        {
            var preuniversitaryExam = await _context.PreuniversitaryExams.Where(x => x.Id == preuniversitaryExamId).FirstOrDefaultAsync();

            var preuniversitaryExamPostulants = await _context.PreuniversitaryExamClassroomPostulants.Where(x => x.PreuniversitaryExamClassroom.PreuniversitaryExamId == preuniversitaryExamId)
                .Select(x=>new
                {
                    x.PreuniversitaryPostulantId,
                    x.PreuniversitaryExamClassroom.Classroom.Description
                })
                .ToListAsync();

            var query = _context.PreuniversitaryPostulants
                .Where(x => x.IsPaid && x.PreuniversitaryTermId == preuniversitaryExam.PreuniversitaryTermId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.FullName,
                      x.Document,
                      classroom = _context.PreuniversitaryExamClassroomPostulants.Where(y=>y.PreuniversitaryPostulantId == x.Id).Select(y=>y.PreuniversitaryExamClassroom.Classroom.Description).FirstOrDefault()
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
