using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class CourseComponentRepository : Repository<CourseComponent>, ICourseComponentRepository
    {
        public CourseComponentRepository(AkdemicContext context) : base(context) { }

        Task<bool> ICourseComponentRepository.AnyByNameAsync(string name, Guid? id)
        {
            if (id.HasValue)
            {
                return _context.CourseComponents.AnyAsync(x => x.Name.Trim().ToLower().Equals(name) && x.Id != id);
            }
            else
            {
                return _context.CourseComponents.AnyAsync(x => x.Name.Trim().ToLower().Equals(name));
            }
        }

        async Task<object> ICourseComponentRepository.GetAllAsModelA()
        {
            var components = await _context.CourseComponents
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        x.QuantityOfUnits
                    }
                )
                .ToListAsync();

            return components;
        }

        public async Task<object> GetAllAsSelect2ClientSide(bool? validate = null)
        {

            if (validate.HasValue && validate.Value)
            {
                var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT).FirstOrDefaultAsync();

                if (configuration is null)
                {
                    configuration = new ENTITIES.Models.Configuration
                    {
                        Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                        Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                    };
                }

                var evaluationsByUnit = Convert.ToBoolean(configuration.Value);

                if (!evaluationsByUnit)
                {
                    var result = new List<Select2Structs.Result>
                    {
                        new Select2Structs.Result
                        {
                             Id = "0",
                             Text = "Sin Componentes"
                        }
                    };

                    return result;
                }

            }

            var componets = await _context.CourseComponents
              .Select(
                  x => new Select2Structs.Result
                  {
                      Id = x.Id,
                      Text = x.Name
                  }
              )
              .ToListAsync();

            return componets;
        }

        async Task<object> ICourseComponentRepository.GetAsModelB(Guid? id)
        {
            var query = _context.CourseComponents.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var component = await query
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        x.QuantityOfUnits
                    }
                )
                .FirstOrDefaultAsync();

            return component;
        }

        async Task<object> ICourseComponentRepository.GetCourseComponents()
        {
            var components = await _context.CourseComponents
                .Select(f => new
                {
                    id = f.Id,
                    text = f.Name,
                    number = f.QuantityOfUnits
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return components;
        }

        public async Task<object> GetCourseComponentsJson()
        {
            var componets = await _context.CourseComponents
                .Select(
                    x => new
                    {
                        id = x.Id,
                        text = x.Name
                    }
                )
                .OrderBy(x => x.text)
                .ToListAsync();
            return componets;
        }

        public async Task<int> AssignCourseComponentsJob()
        {
            var twoUnitsComponent = await _context.CourseComponents.FirstOrDefaultAsync(x => x.QuantityOfUnits == 2);
            if (twoUnitsComponent == null)
            {
                twoUnitsComponent = new CourseComponent
                {
                    Name = "Componente de 2 unidades",
                    QuantityOfUnits = 2
                };

                await _context.CourseComponents.AddAsync(twoUnitsComponent);
                await _context.SaveChangesAsync();
            }

            var threeUnitsComponent = await _context.CourseComponents.FirstOrDefaultAsync(x => x.QuantityOfUnits == 3);
            if (threeUnitsComponent == null)
            {
                threeUnitsComponent = new CourseComponent
                {
                    Name = "Componente de 3 unidades",
                    QuantityOfUnits = 3
                };

                await _context.CourseComponents.AddAsync(threeUnitsComponent);
                await _context.SaveChangesAsync();
            }

            var courses = await _context.Courses.ToListAsync();

            foreach (var course in courses)
            {
                if (course.Credits <= 3) course.CourseComponentId = twoUnitsComponent.Id;
                else course.CourseComponentId = threeUnitsComponent.Id;
            }

            await _context.SaveChangesAsync();
            return courses.Count;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseComponentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<CourseComponent, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.QuantityOfUnits); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            var query = _context.CourseComponents.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      x.QuantityOfUnits
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