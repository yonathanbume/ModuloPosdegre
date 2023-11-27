using AKDEMIC.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Geo.Interfaces
{
    public interface IConnectionService 
    {
        Task InsertConnection(Connection connection);
        Task UpdateConnection(Connection connection);
        Task DeleteConnection(Connection connection);
        Task<Connection> GetConnectionById(Guid id);
        Task<IEnumerable<Connection>> GetAllConnections();
    }
}
