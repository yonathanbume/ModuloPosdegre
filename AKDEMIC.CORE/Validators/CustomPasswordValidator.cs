using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Validators
{
    public class CustomPasswordValidator<TUser>: IPasswordValidator<TUser> where TUser : IdentityUser
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "No se puede usar el usuario como contraseña."
                }));
            }

            if (await manager.CheckPasswordAsync(user, password))
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "SamePassword",
                    Description = "La nueva contraseña no debe ser igual a la contraseña actual."
                }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }
    }
}
