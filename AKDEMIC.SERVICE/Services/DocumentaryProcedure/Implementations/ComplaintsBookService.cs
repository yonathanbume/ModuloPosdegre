using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ComplaintsBookService : IComplaintsBookService
    {
        private readonly IComplaintsBookRepository _complaintsBookRepository;
        public ComplaintsBookService(IComplaintsBookRepository complaintsBookRepository)
        {
            _complaintsBookRepository = complaintsBookRepository;
        }

        public async Task Insert(Complaint newComplaint)
        {
            await _complaintsBookRepository.Insert(newComplaint);
        }
    }
}
