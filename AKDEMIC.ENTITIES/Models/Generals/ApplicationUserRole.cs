using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
