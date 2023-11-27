using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Geo.Implementations
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionService(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public async Task InsertConnection(Connection connection) =>
            await _connectionRepository.Insert(connection);

        public async Task UpdateConnection(Connection connection) =>
            await _connectionRepository.Update(connection);

        public async Task DeleteConnection(Connection connection) =>
            await _connectionRepository.Delete(connection);

        public async Task<Connection> GetConnectionById(Guid id) =>
            await _connectionRepository.Get(id);

        public async Task<IEnumerable<Connection>> GetAllConnections() =>
            await _connectionRepository.GetAll();
    }
}
