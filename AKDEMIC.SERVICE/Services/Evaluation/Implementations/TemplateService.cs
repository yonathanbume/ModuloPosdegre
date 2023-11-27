using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        public TemplateService(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        public async Task<bool> AnyTemplateByName(string name, Guid? id)
        {
            return await _templateRepository.AnyTemplateByName(name, id);
        }
        public async Task<int> Count()
        {
            return await _templateRepository.Count();
        }
        public async Task<Template> Get(Guid id)
        {
            return await _templateRepository.Get(id);
        }
        public async Task<object> GetTemplate(Guid id)
        {
            return await _templateRepository.GetTemplate(id);
        }
        public async Task<IEnumerable<Template>> GetAll()
        {
            return await _templateRepository.GetAll();
        }
        public async Task<IEnumerable<object>> GetTemplates()
        {
            return await _templateRepository.GetTemplates();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetTemplatesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _templateRepository.GetTemplatesDatatable(sentParameters, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _templateRepository.DeleteById(id);
        }
        public async Task Insert(Template template)
        {
            await _templateRepository.Insert(template);
        }
        public async Task Update(Template template)
        {
            await _templateRepository.Update(template);
        }
    }
}
