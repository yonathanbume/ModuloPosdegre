using AKDEMIC.ENTITIES.Models.Sisco;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface IContactService
    {
        Task InsertContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(Contact contact);
        Task<Contact> GetContactById(Guid id);
        Task<Contact> GetFirstContact();
    }
}
