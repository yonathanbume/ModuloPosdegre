using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionRequirement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionRequirementRepository : Repository<AdmissionRequirement>, IAdmissionRequirementRepository
    {
        public AdmissionRequirementRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<AdmissionRequirement>> GetAllWithData()
        {
            var result = await _context.AdmissionRequirements.Include(x => x.PostulantAdmissionRequirements).ToListAsync();

            return result;
        }

        public async Task<object> GetPostulantRequiremtns(Guid admissionTypeId, Guid postulantId)
        {
            var result = await _context.AdmissionRequirements
                .Where(x => x.AdmissionTypeId == admissionTypeId)
                .Select(x => new
                {
                    requirementId = x.Id,
                    name = x.Name,
                    isOptional = x.IsOptional,
                    postulantFile = x.PostulantAdmissionRequirements.Where(y => y.PostulantId == postulantId).Select(y => y.File).FirstOrDefault()
                }).ToListAsync(); 

            return result;
        }
        public void Update(AdmissionRequirement admissionRequirement)
            => _context.AdmissionRequirements.Update(admissionRequirement);

        public async Task<List<AdmissionRequirement>> GetAdmissionRequirementByAdmissionTypeId(Guid id)
            => await _context.AdmissionRequirements.Where(x => x.AdmissionTypeId == id).ToListAsync();
        public async Task<object> GetRequirement(Guid id)
        {
            var ar = await _context.AdmissionRequirements.Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                isOptional = x.IsOptional
            }).FirstOrDefaultAsync();

            return ar;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantRequirementsDatatable(Guid postulantId)
        {
            var postulant = await _context.Postulants.FindAsync(postulantId);

            var postulantRequirements = await _context.PostulantAdmissionRequirements
                .Where(x => x.PostulantId == postulantId)
                .ToListAsync();

            var admissionRequirements = await _context.AdmissionRequirements
                .Where(x => x.AdmissionTypeId == postulant.AdmissionTypeId)
                .ToListAsync();

            var data = admissionRequirements
                .Select(x => new
                {
                    id = x.Id,
                    file = postulantRequirements.Where(y => y.AdmissionRequirementId == x.Id).Select(y => y.File).FirstOrDefault() ?? "",
                    name = x.Name,
                    isChecked = postulantRequirements.Any(y => y.AdmissionRequirementId == x.Id)
                }).OrderBy(x => x.name).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }


        public async Task<List<PostulantRequirementTemplate>> GetPostulantRequirements(Guid postulantId)
        {
            var postulant = await _context.Postulants.Where(x => x.Id == postulantId).FirstOrDefaultAsync();

            var postulantRequirements = await _context.PostulantAdmissionRequirements
                .Where(x => x.PostulantId == postulantId)
                .ToListAsync();

            var admissionRequirements = await _context.AdmissionRequirements
                .Where(x => x.AdmissionTypeId == postulant.AdmissionTypeId)
                .ToListAsync();

            var data = admissionRequirements
                .Select(x => new PostulantRequirementTemplate
                {
                    AdmissionRequirementId = x.Id,
                    //File = postulantRequirements.Where(y => y.AdmissionRequirementId == x.Id).Select(y => y.File).FirstOrDefault() ?? "",
                    Name = x.Name,
                    Validated = postulantRequirements.Any(y => y.AdmissionRequirementId == x.Id && y.IsValidated)
                })
                //.OrderBy(x => x.Name)
                .ToList();

            foreach (var item in data)
            {
                var file = postulantRequirements.Where(y => y.AdmissionRequirementId == item.AdmissionRequirementId).Select(y => y.File).FirstOrDefault();
                if (string.IsNullOrEmpty(file))
                {
                    item.File = "";
                    item.FileType = "";
                }
                else
                {
                    var extension = Path.GetExtension(file);
                    extension = extension.Replace(".", "");

                    if (ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.PDF.ToUpper().Contains(extension.ToUpper()))
                        item.FileType = "pdf";
                    else if (ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES.ToUpper().Contains(extension.ToUpper()))
                        item.FileType = "image";
                    else
                        item.FileType = "file";

                    item.File = file;
                }
            }

            return data;
        }
    }
}
