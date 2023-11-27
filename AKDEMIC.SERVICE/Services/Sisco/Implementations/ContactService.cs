using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task InsertContact(Contact contact) =>
            await _contactRepository.Insert(contact);

        public async Task UpdateContact(Contact contact) =>
            await _contactRepository.Update(contact);

        public async Task DeleteContact(Contact contact) =>
            await _contactRepository.Delete(contact);

        public async Task<Contact> GetContactById(Guid id) =>
            await _contactRepository.Get(id);

        public async Task<Contact> GetFirstContact() =>
            await _contactRepository.GetFirstContact();
    }
}
