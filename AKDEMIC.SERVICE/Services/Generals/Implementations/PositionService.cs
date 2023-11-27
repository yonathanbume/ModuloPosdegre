using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public sealed class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        Task IPositionService.DeleteAsync(Position position)
            => _positionRepository.Delete(position);

        Task<object> IPositionService.GetAllAsModelA()
            => _positionRepository.GetAllAsModelA();

        Task<Position> IPositionService.GetAsync(Guid id)
            => _positionRepository.Get(id);

        Task IPositionService.InsertAsync(Position position)
            => _positionRepository.Insert(position);

        Task IPositionService.UpdateAsync(Position position)
            => _positionRepository.Update(position);
    }
}