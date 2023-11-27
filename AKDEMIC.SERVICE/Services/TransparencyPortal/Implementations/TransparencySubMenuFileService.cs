using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencySubMenuFileService : ITransparencySubMenuFileService
    {
        private readonly ITransparencySubMenuFileRepository _transparencySubMenuFileRepository;

        public TransparencySubMenuFileService(ITransparencySubMenuFileRepository transparencySubMenuFileRepository)
        {
            _transparencySubMenuFileRepository = transparencySubMenuFileRepository;
        }
    }
}
