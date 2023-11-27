using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkerRemunerationRepository : Repository<WorkerRemuneration>, IWorkerRemunerationRepository
    {
        public WorkerRemunerationRepository(AkdemicContext context) : base(context) { }

        public async Task<List<WorkerRemunerationTemplate>> GetAllToCloseWorkingTerm(Guid workingTermId)
        {
            var result = new List<WorkerRemunerationTemplate>();
            //Traer todos los pagos que se tienen que hacer en un periodo, para crear el historial (WorkerTermPayrollDetail)
            //La busqueda se base en el año , y mes en el que se encuentre
            var workingTermToClose = await _context.WorkingTerms
                .Where(x => x.Id == workingTermId)
                .Select(x => new 
                {
                    x.Id,
                    x.Year,
                    x.MonthNumber,
                    x.Number
                })
                .FirstOrDefaultAsync();

            //WHERE ANTERIOR
            /*
                .Where(x => x.StartWorkingTerm.Year <= workingTermToClose.Year && x.EndWorkingTerm.Year >= workingTermToClose.Year &&
                 x.StartWorkingTerm.MonthNumber <= workingTermToClose.MonthNumber && x.EndWorkingTerm.MonthNumber >= workingTermToClose.MonthNumber)
            */

if (workingTermToClose != null)
{
    var data = await _context.WorkerRemunerations
        .Where(x => x.StartWorkingTermId == workingTermToClose.Id || x.EndWorkingTermId == workingTermToClose.Id )
        .Select(x => new WorkerRemunerationTemplate
        {
            Id = x.Id,
            Description = x.Description,
            ConceptTypeId = x.ConceptTypeId,
            WageItemId = x.WageItemId,
            Amount = x.Amount,
            IsActive = x.IsActive,
            WorkerId = x.WorkerId,
            StartWorkingTerm = new WorkingTermInfoTemplate
            {
                Id = x.StartWorkingTerm.Id,
                Correlative = $"{x.StartWorkingTerm.Year}{x.StartWorkingTerm.MonthNumber}{x.StartWorkingTerm.Number}",
                Year = x.StartWorkingTerm.Year,
                MonthNumber = x.StartWorkingTerm.MonthNumber,
                Number = x.StartWorkingTerm.Number,
                StartDate = x.StartWorkingTerm.StartDate,
                StartDateText = x.StartWorkingTerm.StartDate.ToLocalDateFormat(),
                EndDate = x.StartWorkingTerm.EndDate,
                EndDateText = x.StartWorkingTerm.EndDate.ToLocalDateFormat(),
                IsExtraTerm = x.StartWorkingTerm.IsExtraTerm
            },
            EndWorkingTerm = new WorkingTermInfoTemplate
            {
                Id = x.EndWorkingTerm.Id,
                Correlative = $"{x.EndWorkingTerm.Year}{x.EndWorkingTerm.MonthNumber}{x.EndWorkingTerm.Number}",
                Year = x.EndWorkingTerm.Year,
                MonthNumber = x.EndWorkingTerm.MonthNumber,
                Number = x.EndWorkingTerm.Number,
                StartDate = x.EndWorkingTerm.StartDate,
                StartDateText = x.EndWorkingTerm.StartDate.ToLocalDateFormat(),
                EndDate = x.EndWorkingTerm.EndDate,
                EndDateText = x.EndWorkingTerm.EndDate.ToLocalDateFormat(),
                IsExtraTerm = x.EndWorkingTerm.IsExtraTerm
            }
        })
        .ToListAsync();

    result = data;
}

return result;
}

public async Task<DataTablesStructs.ReturnedData<object>> GetAllWorkerRemunerationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid workerId, Guid? conceptTypeId = null, string searchValue = null)
{
Expression<Func<WorkerRemuneration, dynamic>> orderByPredicate = null;
switch (sentParameters.OrderColumn)
{
    case "0":
        orderByPredicate = ((x) => x.WageItem.Name);
        break;
    case "1":
        orderByPredicate = (x) => x.Description;
        break;
    case "2":
        orderByPredicate = ((x) => x.ConceptType.Name);
        break;
    case "3":
        orderByPredicate = ((x) => x.StartWorkingTerm.StartDate);
        break;
    case "4":
        orderByPredicate = ((x) => x.EndWorkingTerm.EndDate);
        break;
    case "5":
        orderByPredicate = ((x) => x.Amount);
        break;
    case "6":
        orderByPredicate = ((x) => x.IsActive);
        break;

}

var query = _context.WorkerRemunerations.Where(x => x.WorkerId == workerId).AsNoTracking();

if (conceptTypeId != null)
{
    query = query.Where(x => x.ConceptTypeId == conceptTypeId);
}

if (!string.IsNullOrEmpty(searchValue))
{
    var searchTrim = searchValue.Trim();
    query = query.Where(x => x.WageItem.Name.Contains(searchTrim) ||
                            x.Description.Contains(searchTrim) ||
                            x.ConceptType.Name.Contains(searchTrim));
}

int recordsFiltered = await query.CountAsync();

var data = await query
    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
    .Select(x => new
    {
        x.Id,
        WageItemName = x.WageItem.Name,
        x.Description,
        ConceptType = x.ConceptType.Name,
        StartWorkingTerm = $"{x.StartWorkingTerm.Year}{x.StartWorkingTerm.MonthNumber}{x.StartWorkingTerm.Number}",
        EndWorkingTerm = $"{x.EndWorkingTerm.Year}{x.EndWorkingTerm.MonthNumber}{x.EndWorkingTerm.Number}",
        x.Amount,
        x.IsActive
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

public async Task<DataTablesStructs.ReturnedData<object>> GetFilteredWorkerRemunerationsDatatable(Guid workerId, Guid startWorkingTermId, Guid endWorkingTermId)
{
var result = new List<FilteredWorkerRemunerationtemplate>();

 var startWorkingTerm = await _context.WorkingTerms
.Where(x => x.Id == startWorkingTermId)
.Select(x => new
{
    x.Id,
    x.Year,
    x.MonthNumber,
    x.Number,
    x.StartDate
})
.FirstOrDefaultAsync();

 var endWorkingTerm = await _context.WorkingTerms
.Where(x => x.Id == endWorkingTermId)
.Select(x => new
{
    x.Id,
    x.Year,
    x.MonthNumber,
    x.Number,
    x.EndDate
})
.FirstOrDefaultAsync();

var toFilterByYear = await _context.WorkingTerms
.Where(x => x.Year <= endWorkingTerm.Year && x.Year >= startWorkingTerm.Year
&& x.EndDate <= endWorkingTerm.EndDate
&& x.StartDate >= startWorkingTerm.StartDate)
            .Select(x => new
            {
                x.Id,
                x.Year,
                x.MonthNumber,
                x.Number,
                x.Status
            })
.ToListAsync();

//var toFilter = await _context.WorkingTerms
//.Where(x => x.Year <= endWorkingTerm.Year && x.Year >= startWorkingTerm.Year 
//&& int.Parse(x.Year.ToString()+x.MonthNumber.ToString("00")) <= int.Parse(endWorkingTerm.Year.ToString()+endWorkingTerm.MonthNumber.ToString("00")) 
//&& int.Parse(x.Year.ToString()+x.MonthNumber.ToString("00")) >= int.Parse(startWorkingTerm.Year.ToString() + startWorkingTerm.MonthNumber.ToString("00")))
//            .Select(x => new
//            {
//                x.Id,
//                x.Year,
//                x.MonthNumber,
//                x.Number,
//                x.Status
//            })
//.ToListAsync();

var queryRemuneration = _context.WorkerRemunerations.Where(x => x.WorkerId == workerId).AsNoTracking();
var queryDetail = _context.WorkerTermPayrollDetails.Where(x => x.WorkerId == workerId).AsNoTracking();

foreach (var f in toFilterByYear)
{
    if (f.Status == ConstantHelpers.WORKINGTERM_STATUS.FINISHED)
    {
       var querydata = queryDetail.Where(x => x.WorkingTerm.Id == f.Id)
           .Select(x=> new FilteredWorkerRemunerationtemplate
           {
               WageItemName = x.WageItem.Name,
               Amount = x.Amount,
               WorkingTerm = $"{x.WorkingTerm.Year}{x.WorkingTerm.MonthNumber}{x.WorkingTerm.Number}",

           }).FirstOrDefault();

           if (querydata != null)
               result.Add(querydata);

    }

    else
    {
        var querydata = queryRemuneration.Where(x => (x.StartWorkingTerm.Year == f.Year || x.EndWorkingTerm.Year == f.Year )
        && (x.StartWorkingTerm.MonthNumber == f.MonthNumber || x.EndWorkingTerm.MonthNumber == f.MonthNumber))
            .Select(x => new FilteredWorkerRemunerationtemplate
            {
                WageItemName = x.WageItem.Name,
                Amount = x.Amount,
                WorkingTerm = $"{x.StartWorkingTerm.Year}{x.StartWorkingTerm.MonthNumber}{x.StartWorkingTerm.Number}",
            }).FirstOrDefault();

        result.Add(querydata);
    }
}

int recordsFiltered = result.Count();


var data = result
    .Select(x => new
    {
        x.WageItemName,
        x.Amount,
        x.WorkingTerm
    })
    .Distinct()
    .ToList();

int recordsTotal = data.Count;

return new DataTablesStructs.ReturnedData<object>
{
    Data = data,
    RecordsFiltered = recordsFiltered,
    RecordsTotal = recordsTotal
};
}
}
}
