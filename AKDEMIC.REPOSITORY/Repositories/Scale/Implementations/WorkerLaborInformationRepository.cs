using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.WorkerLaborInformation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerLaborInformationRepository : Repository<WorkerLaborInformation>, IWorkerLaborInformationRepository
    {
        public WorkerLaborInformationRepository(AkdemicContext context) : base(context) { }

        public async Task<WorkerLaborInformation> GetByUserId(string userId)
        {
            var query = _context.WorkerLaborInformation
                .Include(x => x.Campus)
                .Include(x => x.Building)
                .Include(x => x.WorkerFamilyInformations)
                .Include(x => x.WorkerLaborCategory)
                .Include(x => x.AcademicDepartment)
                .Include(x => x.WorkerLaborCondition)
                .Include(x => x.WorkerLaborRegime)
                .Where(x => x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }      

        public async Task<List<LaborUserTemplate>> GetUserLaborReport(string search)
        {
            var query = _context.Users
                .Where(x => !x.Students.Any(y => y.UserId == x.Id))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                string searchTrim = search.Trim();
                query = query.Where(x => x.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.FullName.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.Dni.ToUpper().Contains(searchTrim.ToUpper()));
            }

            var data = await query
                .Select(x => new LaborUserTemplate
                {
                    UserName = x.UserName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Name = x.Name,
                    Dni = x.Dni,
                    EntryDate = x.WorkerLaborInformation.EntryDate.HasValue ? x.WorkerLaborInformation.EntryDate.Value.ToLocalDateFormat() : "",
                    LaborRegime = x.WorkerLaborInformation.WorkerLaborRegimeId != null ? x.WorkerLaborInformation.WorkerLaborRegime.Name : "",
                    RetirementSystem = ConstantHelpers.RETIREMENT_SYSTEM.VALUES.ContainsKey(x.WorkerRetirementSystemHistories.Where(y => y.UserId == x.Id).Select(y => y.RetirementSystem).FirstOrDefault()) ?
                        ConstantHelpers.RETIREMENT_SYSTEM.VALUES[x.WorkerRetirementSystemHistories.Where(y => y.UserId == x.Id).Select(y => y.RetirementSystem).FirstOrDefault()] : ""
                })
                .ToListAsync();


            return data;
        }

        public async Task<bool> AnyPlaceCode(string placeCode, string userId = null)
        {
            var query = _context.WorkerLaborInformation.AsQueryable();

            if (string.IsNullOrEmpty(userId))
                return await query.AnyAsync(x => x.PlaceCode == placeCode);

            return await query.Where(x => x.UserId != userId)
                            .AnyAsync(x => x.PlaceCode == placeCode);
        }

        public async Task<object> GetUserDetailInformation(string userId)
        {
            var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userId);
            var teacher = await _context.Teachers
                .Where(x => x.UserId == userId)
                .Select(x => new 
                {
                    AcademicDepartment = x.AcademicDepartment == null ? "": x.AcademicDepartment.Name,
                    TeacherDedication = x.TeacherDedication == null ? "": x.TeacherDedication.Name
                })
                .FirstOrDefaultAsync();

            var laborInformation = await _context.WorkerLaborInformation
                .Where(x => x.UserId == userId)
                .Select(x => new 
                {
                    LaborRegime = x.WorkerLaborRegime == null ? "": x.WorkerLaborRegime.Name,
                    LaborCondition = x.WorkerLaborCondition == null ? "": x.WorkerLaborCondition.Name,
                    LaborCategory = x.WorkerLaborCategory == null ? "": x.WorkerLaborCategory.Name
                })
                .FirstOrDefaultAsync();

            var userDependecies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency.Name)
                .ToListAsync();

            var userroles = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.Role.Name)
                .ToListAsync();

            var administrative = false;

            if (userroles.Count > 1)
            {
                administrative = true;
            }else if (userroles.Count == 1 && teacher == null)
            {
                administrative = true;
            }

            var result = new
            {
                laborRegime = laborInformation == null ? "" : laborInformation.LaborRegime,
                laborCondition = laborInformation == null ? "" : laborInformation.LaborCondition,
                laborCategory = laborInformation == null ? "" : laborInformation.LaborCategory,
                academicDepartment = teacher == null ? "" : teacher.AcademicDepartment,
                dedication = teacher == null ? "" : teacher.TeacherDedication,
                userDependecies = string.Join(", ", userDependecies),
                userRoles = string.Join(", ", userroles),
                isTeacher = teacher == null ? false : true,
                isAdministrative = administrative,
                state = user.State
            };

            return result;
        }

        public void Remove(WorkerLaborInformation workerLaborInformation)
        {
            _context.WorkerLaborInformation.Remove(workerLaborInformation);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetLaborUserDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.MaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Dni);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.WorkerLaborInformation.EntryDate);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.WorkerLaborInformation.WorkerLaborRegime.Name);
                    break;
            }

            var query = _context.Users
                .Where(x => !x.Students.Any(y => y.UserId == x.Id))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.FullName.ToUpper().Contains(searchTrim.ToUpper()) ||
                                    x.Dni.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserName,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Dni,
                    EntryDate = x.WorkerLaborInformation.EntryDate.HasValue ? x.WorkerLaborInformation.EntryDate.Value.ToLocalDateFormat() : "",
                    LaborRegime = x.WorkerLaborInformation.WorkerLaborRegimeId != null ? x.WorkerLaborInformation.WorkerLaborRegime.Name : "",
                    RetirementSystem = ConstantHelpers.RETIREMENT_SYSTEM.VALUES.ContainsKey(x.WorkerRetirementSystemHistories.Where(y => y.UserId == x.Id).Select(y => y.RetirementSystem).FirstOrDefault()) ?
                        ConstantHelpers.RETIREMENT_SYSTEM.VALUES[x.WorkerRetirementSystemHistories.Where(y => y.UserId == x.Id).Select(y => y.RetirementSystem).FirstOrDefault()] : ""
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
