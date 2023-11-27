using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    //public class ClientRepository : Repository<Client>, IClientRepository
    //{
    //    public ClientRepository(AkdemicContext context) : base(context) { }

    //    public async Task<string> GetClientAddrees(Guid? id = null)
    //     => await _context.Clients.Where(x => x.Id == id).Select(x => x.ContactEmail).FirstOrDefaultAsync();

    //    public async Task<Client> GetClientByDocument(string document)
    //        => await _context.Clients.FirstOrDefaultAsync(x => x.IdentificationDocumentNumber == document);
    //    public IQueryable<Client> ClientsQuery()
    //        => _context.Clients.AsNoTracking();

    //    public async Task<object> GetClientesSale(string term, string typeOfMovement, string userId)
    //    {
    //        var clientsQuery = _context.Clients.AsNoTracking();

    //        if (typeOfMovement == "3")
    //        {
    //            clientsQuery = clientsQuery.Where(x => (x.BusinessName.Contains(term) || x.IdentificationDocumentNumber.Contains(term)) && x.IdentificationDocumentType == "6");
    //        }
    //        else if (typeOfMovement == "2")
    //        {
    //            clientsQuery = clientsQuery.Where(x => (x.Name.Contains(term) || x.MatSurname.Contains(term) || x.PatSurname.Contains(term) || x.IdentificationDocumentNumber.Contains(term)) && x.IdentificationDocumentType == "1");
    //        }

    //        var clients = await clientsQuery
    //           .Select(x => new
    //           {
    //               id = x.Id,
    //               text = $"{x.IdentificationDocumentNumber} - {x.BusinessName ?? (x.PatSurname + " " + x.MatSurname + ", " + x.Name)}"
    //           })
    //           .ToListAsync();

    //        return clients;
    //    }

    //    public async Task<object> GetClientssByTermSelect2(string term)
    //    {
    //        var clientsQuery = _context.Clients
    //            .Where(x => x.BusinessName.ToUpper().Contains(term.ToUpper()) || x.Name.ToUpper().Contains(term.ToUpper()) || x.IdentificationDocumentNumber.ToUpper().Contains(term.ToUpper()))
    //            .AsNoTracking();

    //        var clients = await clientsQuery
    //           .Select(x => new
    //           {
    //               id = x.Id,
    //               text = $"{x.IdentificationDocumentNumber} - {x.BusinessName ?? (string.IsNullOrEmpty(x.PatSurname) ? x.Name : x.PatSurname + " " + x.MatSurname + ", " + x.Name)}"
    //           })
    //           .ToListAsync();

    //        return clients;
    //    }

    //    public async Task<Client> GetByUserId(string userId)
    //    => await _context.Clients.Where(x => x.Name == userId).FirstOrDefaultAsync();
    //}
}
