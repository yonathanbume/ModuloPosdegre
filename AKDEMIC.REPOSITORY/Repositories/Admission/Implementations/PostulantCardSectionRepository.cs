using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class PostulantCardSectionRepository : Repository<PostulantCardSection>, IPostulantCardSectionRepository
    {
        public PostulantCardSectionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<PostulantCardSection>> GetConfiguration(Guid admissionTypeId)
        {
            //appTerm AdmissionType
            var exist = await _context.PostulantCardSections.AnyAsync(x => x.AdmissionTypeId == admissionTypeId);


            if (!exist)
            {
                var list = new List<PostulantCardSection>();
                foreach (var item in ConstantHelpers.POSTULANT_CARD.SECTION.VALUES)
                {
                    list.Add(new PostulantCardSection
                    {
                        AdmissionTypeId = admissionTypeId,
                        IsRequired = false,
                        IsVisible = true,
                        SectionId = item.Key,
                    });
                }
                await _context.PostulantCardSections.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            else
            {
                var postulantCards = await _context.PostulantCardSections.Where(x => x.AdmissionTypeId == admissionTypeId).ToListAsync();

                if (ConstantHelpers.POSTULANT_CARD.SECTION.VALUES.Count() > postulantCards.Count())
                {
                    var postulantExistingCards = ConstantHelpers.POSTULANT_CARD.SECTION.VALUES.Select(x => x.Key).ToList();
                    var indexPostulantCards = postulantCards.Select(x => x.SectionId).ToList();
                    var indexResolveList = postulantExistingCards.Except(indexPostulantCards).ToList();
                    if (indexResolveList.Count > 0)
                    {
                        var listToAdd = new List<PostulantCardSection>();
                        foreach (var item in indexResolveList)
                        {
                            listToAdd.Add(new PostulantCardSection
                            {
                                AdmissionTypeId = admissionTypeId,
                                IsRequired = false,
                                IsVisible = true,
                                SectionId = item,
                            });
                        }
                        await _context.PostulantCardSections.AddRangeAsync(listToAdd);
                        await _context.SaveChangesAsync();
                    }


                }
            }

            return await _context.PostulantCardSections.Where(x => x.AdmissionTypeId == admissionTypeId).ToListAsync();
        }

        public async Task SaveConfiguration(Guid id, List<PostulantCardSection> sections)
        {
            var exist = await _context.PostulantCardSections.AnyAsync(x => x.AdmissionTypeId == id);

            if (!exist)
            {
                var list = new List<PostulantCardSection>();
                foreach (var item in ConstantHelpers.POSTULANT_CARD.SECTION.VALUES)
                {
                    var sect = sections.FirstOrDefault(x => x.SectionId == item.Key);
                    var newsect = new PostulantCardSection();
                    newsect.AdmissionTypeId = id;
                    newsect.IsRequired = sect.IsRequired;
                    newsect.IsVisible = sect.IsVisible;
                    newsect.SectionId = item.Key;

                    list.Add(sect);
                }
                await _context.PostulantCardSections.AddRangeAsync(list);
            }
            else
            {
                foreach (var item in sections)
                {
                    var updatesect = await _context.PostulantCardSections.FirstOrDefaultAsync(x => x.AdmissionTypeId == id && x.SectionId == item.SectionId);
                    if (updatesect == null)
                    {
                        updatesect = new PostulantCardSection();
                        updatesect.AdmissionTypeId = id;
                        updatesect.IsRequired = item.IsRequired;
                        updatesect.IsVisible = item.IsVisible;
                        updatesect.SectionId = item.SectionId;
                        _context.PostulantCardSections.Add(updatesect);
                    }
                    else
                    {
                        updatesect.IsRequired = item.IsRequired;
                        updatesect.IsVisible = item.IsVisible;
                    }

                }
            }

            var admissionType = await _context.AdmissionTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (sections.Any(x => x.SectionId == ConstantHelpers.POSTULANT_CARD.SECTION.GRADUATES
            && x.IsVisible && x.IsRequired))
                admissionType.IsGraduateAdmissionType = true;
            else admissionType.IsGraduateAdmissionType = false;

            if (sections.Any(x => x.SectionId == ConstantHelpers.POSTULANT_CARD.SECTION.PREUNIVERSITY_CENTER
            && x.IsVisible && x.IsRequired))
                admissionType.IsCepreAdmissionType = true;
            else admissionType.IsCepreAdmissionType = false;

            if (sections.Any(x => x.SectionId == ConstantHelpers.POSTULANT_CARD.SECTION.INTERNAL_TRANSFER
            && x.IsVisible && x.IsRequired))
                admissionType.IsInternalTransfer = true;
            else admissionType.IsInternalTransfer = false;

            await _context.SaveChangesAsync();
        }
    }
}
