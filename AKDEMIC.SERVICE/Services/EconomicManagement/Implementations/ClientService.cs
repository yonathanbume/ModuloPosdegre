using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    //public class ClientService : IClientService
    //{
    //    private readonly IClientRepository _clientRepository;

    //    public ClientService(IClientRepository clientRepository)
    //    {
    //        _clientRepository = clientRepository;
    //    }

    //    public async Task InsertClient(Client client) =>
    //        await _clientRepository.Insert(client);

    //    public async Task UpdateClient(Client client) =>
    //        await _clientRepository.Update(client);

    //    public async Task DeleteClient(Client client) =>
    //        await _clientRepository.Delete(client);

    //    public async Task<Client> GetClientById(Guid id) =>
    //        await _clientRepository.Get(id);

    //    public async Task<IEnumerable<Client>> GetAllClients() =>
    //        await _clientRepository.GetAll();

    //    public async Task<string> GetClientAddrees(Guid? id = null)
    //        => await _clientRepository.GetClientAddrees(id);
    //    public IQueryable<Client> ClientsQuery()
    //        =>  _clientRepository.ClientsQuery();
    //    public async Task<object> GetClientesSale(string term, string typeOfMovement, string userId)
    //        => await _clientRepository.GetClientesSale(term, typeOfMovement, userId);
    //    public async Task<Client> GetByUserId(string userId)
    //        => await _clientRepository.GetByUserId(userId);
    //    public async Task<Client> GetClientByDocument(string document)
    //        => await _clientRepository.GetClientByDocument(document);

    //    public async Task<object> GetClientssByTermSelect2(string term)
    //        => await _clientRepository.GetClientssByTermSelect2(term);
    //}
}
