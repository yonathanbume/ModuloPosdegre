using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(AkdemicContext context) : base(context) { }

        public async Task<List<Classroom>> GetAllByBuildingId(Guid buildingId)
            => await _context.Classrooms
                .Where(x => x.BuildingId == buildingId)
                .ToListAsync();

        public async Task<List<Classroom>> GetAllWithData()
           => await _context.Classrooms
                .Include(x => x.Building)
                .ThenInclude(x => x.Campus)
            .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<Classroom, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Capacity;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Building.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Building.Campus.Name;
                    break;
                default:
                    break;
            }

            var query = _context.Classrooms
                .IgnoreQueryFilters().Where(x => x.DeletedAt == null)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   id = x.Id,
                   name = x.Description,
                   building = new
                   {
                       id = x.BuildingId,
                       name = x.Building.Name
                   },
                   campus = new
                   {
                       id = x.Building.CampusId,
                       name = x.Building.Campus.Name
                   },
                   type = new
                   {
                       id = x.ClassroomTypeId,
                       name = x.ClassroomType.Name
                   },
                   x.FacultyId,
                   FacultyName = x.FacultyId != null ? x.Faculty.Name : "",
                   capacity = x.Capacity,
                   status = x.Status == CORE.Helpers.ConstantHelpers.STATES.ACTIVE,
                   iPAddress = x.IPAddress,
                   code = x.Code,
                   floor = x.Floor,
                   number = x.Number
               })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public override async Task<Classroom> Get(Guid id) => await _context.Classrooms.IgnoreQueryFilters().Where(x => x.Id == id).FirstOrDefaultAsync();

        public override async Task DeleteById(Guid id)
        {
            var classroom = await _context.Classrooms.IgnoreQueryFilters().Where(x => x.Id == id).FirstOrDefaultAsync();
            await Delete(classroom);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetClassroomsSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null)
        {
            var query = _context.Classrooms
                .IgnoreQueryFilters()
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE && x.DeletedAt == null && x.DeletedBy == null)
                .AsQueryable();

            var classroomDummy = await _context.Classrooms.Where(x => x.Description == "Sin Asignar" && x.Code == "Sin Asignar").FirstOrDefaultAsync();
            if (classroomDummy == null)
            {
                var building = await _context.Buildings.FirstOrDefaultAsync();
                var type = await _context.ClassroomTypes.FirstOrDefaultAsync();
                classroomDummy = new Classroom
                {
                    BuildingId = building.Id,
                    ClassroomTypeId = type.Id,
                    Capacity = 30,
                    Code = "Sin Asignar",
                    Description = "Sin Asignar",
                    Status = 1
                };

                await _context.Classrooms.AddAsync(classroomDummy);
                await _context.SaveChangesAsync();
            }

            if (buildingId.HasValue)
                query = query.Where(x => x.BuildingId == buildingId);

            if (campusId.HasValue)
                query = query.Where(x => x.Building.CampusId == campusId);

            var result = await query
                .OrderBy(x => x.Description)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Description,
                    Selected = false
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetClassroomsWithDataSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null, string search = null)
        {
            var query = _context.Classrooms.AsQueryable();

            if (buildingId.HasValue)
                query = query.Where(x => x.BuildingId == buildingId);

            if (campusId.HasValue)
                query = query.Where(x => x.Building.CampusId == campusId);



            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Building.Campus.Name} - {x.Building.Name} - {x.Description} - {x.Code}",
                    capacity = x.Capacity,
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => x.text.ToUpper().Contains(search.ToUpper())).ToList();
            }

            result = result.OrderBy(x => x.text).Take(100).ToList();

            return result;
        }

        public async Task<object> GetClassroomsJson(string q)
        {
            var result = _context.Classrooms
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Description
                }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(c => c.text.Contains(q));

            var model = await result.OrderBy(c => c.text).ToListAsync();

            return model;
        }

        public async Task<object> GetClassroomsWithCampusJson(string q)
        {
            var result = _context.Classrooms
                .Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Building.Campus.Name} - {c.Building.Name} - {c.Description}"
                }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(c => c.text.Contains(q));

            var model = await result.OrderBy(c => c.text).ToListAsync();

            return model;
        }

        public async Task<Select2Structs.ResponseParameters> GetClassroomByBuilding(Select2Structs.RequestParameters requestParameters, Guid? buildingId = null, Expression<Func<Classroom, Select2Structs.Result>> selectPredicate = null, Func<Classroom, string[]> searchValuePredicate = null, string search = null)
        {
            var query = _context.Classrooms
                            .Where(x => x.BuildingId == buildingId)
                            //.WhereSearchValue(searchValuePredicate, search)
                            .AsNoTracking();

            //if (!string.IsNullOrEmpty(search))
            //{
            //    query = query.Where(x => x.Description.ToUpper().Contains(search.ToUpper()));
            //}

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, search)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetClassroomSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? buildingId = null, Guid? campusId = null)
        {
            var query = _context.Classrooms
                .IgnoreQueryFilters()
                .Where(x=>x.Status == ConstantHelpers.STATES.ACTIVE && x.DeletedAt == null && x.DeletedBy == null)
                .AsNoTracking();

            var classroomDummy = await _context.Classrooms.Where(x => x.Description == "Sin Asignar" && x.Code == "Sin Asignar").FirstOrDefaultAsync();
            if (classroomDummy == null)
            {
                var building = await _context.Buildings.FirstOrDefaultAsync();
                var type = await _context.ClassroomTypes.FirstOrDefaultAsync();
                classroomDummy = new Classroom
                {
                    BuildingId = building.Id,
                    ClassroomTypeId = type.Id,
                    Capacity = 30,
                    Code = "Sin Asignar",
                    Description = "Sin Asignar",
                    Status = 1
                };

                await _context.Classrooms.AddAsync(classroomDummy);
                await _context.SaveChangesAsync();
            }

            if (buildingId.HasValue)
                query = query.Where(x => x.BuildingId == buildingId);

            if (campusId.HasValue)
                query = query.Where(x => x.Building.CampusId == campusId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.Contains(searchValue.Trim().ToLower()));

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Description
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);

        }

        public async Task<Classroom> GetWithData(Guid id)
            => await _context.Classrooms.Include(x => x.Building).ThenInclude(x => x.Campus).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> AnyClassSchedule(Guid id, Guid termId)
            => await _context.Classrooms.Where(x => x.Id == id && x.ClassSchedules.Any(y => y.Section.CourseTerm.TermId == termId)).AnyAsync();
        
        public async Task<bool> AnyClassSchedule(Guid id)
            => await _context.Classrooms.Where(x => x.Id == id && x.ClassSchedules.Any()).AnyAsync();

        public async Task<bool> AnyByDescription(string description, Guid buildingId, Guid? ignoredId)
        {
            return await _context.Classrooms.AnyAsync(x => x.Description.ToLower().Trim() == description.ToLower().Trim() && x.BuildingId == buildingId && x.Id != ignoredId);
        }
    }
}
