using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmiApp.Service.Dto.Auth;

namespace BmiApp.Service.Auth
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> CreateToken();
    }
}
