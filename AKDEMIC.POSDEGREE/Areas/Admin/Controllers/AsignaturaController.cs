using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.POSDEGREE.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    public class AsignaturaController : BaseController
    {
       [Authorize]
       [Area("Admin")]
       [Route("admin/asignatura")]
        // GET: GestionController
        public ActionResult Index()
        {
            return View();
        }


    }
}
