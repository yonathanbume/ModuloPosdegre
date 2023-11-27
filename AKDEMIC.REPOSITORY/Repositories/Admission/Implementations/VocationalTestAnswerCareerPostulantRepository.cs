using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class VocationalTestAnswerCareerPostulantRepository : Repository<VocationalTestAnswerCareerPostulant>, IVocationalTestAnswerCareerPostulantRepository
    {
        public VocationalTestAnswerCareerPostulantRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<object> GetVocationalTestAnswerCareerPostulantsFiltered(Guid postulantId)
        {
            var client = await _context.VocationalTestAnswerCareerPostulants
                .Where(x => x.PostulantId == postulantId)
                .Select(x => new
                {
                    x.VocationalTestAnswerCareer,
                    x.VocationalTestAnswerCareer.Career,
                })
                .ToListAsync();
            var result = client
                .GroupBy(s => s.VocationalTestAnswerCareer)
                .Select(x => new
                {
                    career = x.Key.Career.Name,
                    score = x.Sum(c => c.VocationalTestAnswerCareer.Score)
                })
                .OrderByDescending(a => a.score)
                .Take(3)
                .ToList();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetVocationalTestAnswerCareerDataTable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null)
        {
            Expression<Func<VocationalTestAnswerCareerPostulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Postulant.ApplicationTerm.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => $"{x.Postulant.Name} {x.Postulant.PaternalSurname} {x.Postulant.MaternalSurname}";
                    break;
                default:
                    orderByPredicate = (x) => x.Postulant.ApplicationTerm.Name;
                    break;
            }

            var query = _context.VocationalTestAnswerCareerPostulants
                .Include(x => x.VocationalTestAnswerCareer.Career).AsQueryable();

            if (applicationTermId != null)
            {
                query = query.Where(x => x.Postulant.ApplicationTermId == applicationTermId);
            }

            var recordsFiltered = query.Count();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                 .OrderBy(x => x.Postulant.Name)
                .ThenBy(x => x.VocationalTestAnswerCareer.Score)
                .Select(x => new
            {
                applicationTerm = x.Postulant.ApplicationTerm.Term.Name,
                postulant = $"{x.Postulant.Name} {x.Postulant.PaternalSurname} {x.Postulant.MaternalSurname}",
                career = x.VocationalTestAnswerCareer.Career.Name,
                postulantcareer = x.Postulant.Career.Name,
                score = x.VocationalTestAnswerCareer.Score
            }).ToListAsync();


            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.postulant.Contains(search)).ToList();
            }   

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        
        public async Task<object> GetChart(Guid applicationTermId)
        {
            //var postulantAnswers = await _context.VocationalTestAnswerCareerPostulants
            //    .Where(x => x.Postulant.ApplicationTermId == applicationTermId)
            //    .Select(x => new
            //    {
            //        Career = x.VocationalTestAnswerCareer.Career.Name
            //    }).ToListAsync();

            //var result = postulantAnswers
            //    .GroupBy(s => s)
            //    .Select(x => new
            //    {
            //        career = x.Key.Career,
            //        score = x.Count()
            //    })
            //    .OrderByDescending(a => a.score)
            //    .ToList();

            var result = await _context.VocationalTestAnswerCareers
                .Select(x => new
                {
                    career = x.Career.Name,
                    score = x.Postulants.Where(y => y.Postulant.ApplicationTermId == applicationTermId).Count()
                })
                .ToListAsync();

            var data = new
            {
                categories = new List<string>(),
                data = new List<int>()
            };

            foreach (var item in result)
            {
                data.categories.Add(item.career);
                data.data.Add(item.score);
            }

            return data;
        }
    }
}
