﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface ICafobeRequestDetailRepository:IRepository<CafobeRequestDetail>
    {
        Task<CafobeRequestDetailTemplate> GetDataById(Guid cafobeRequestId);
        Task<List<CafobeRequestDetailTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null);
    }
}
