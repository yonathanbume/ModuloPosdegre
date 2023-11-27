using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
