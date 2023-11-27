using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CurriculumArea;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CurriculumAreaRepository : Repository<CurriculumArea>, ICurriculumAreaRepository
    {
        public CurriculumAreaRepository(AkdemicContext context) : base(context) { }

        public async Task<CurriculumArea> GetWithFacultiesId(Guid id) => await _context.CurriculumAreas
            .Where(x => x.Id == id)
            .Select(x =>
            new CurriculumArea
            {
                Id = x.Id,
                Name = x.Name,
                FacultyCurriculumAreas = x.FacultyCurriculumAreas.Select(fc => new FacultyCurriculumArea
                {
                    FacultyId = fc.FacultyId
                }).ToList()
            }).FirstOrDefaultAsync();

        public async Task<object> GetCurriculumAreasJson(string q)
        {
            var result = _context.CurriculumAreas
                .Select(f => new
                {
                    id = f.Id,
                    name = f.Name
                }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(x => x.name.Contains(q));

            var model = await result.ToListAsync();
            return model;
        }
        public async Task<List<CurriculumAreaTemplate>> GetCurriculumsForTemary()
        {
            return await _context.CurriculumAreas
                .Select(x => new CurriculumAreaTemplate
                {
                    Name = x.Name,
                })
                .ToListAsync();
        }
    }
}
