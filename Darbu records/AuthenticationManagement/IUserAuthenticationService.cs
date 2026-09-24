using Darbu_records.Data;
using System.Security.Claims;

namespace Darbu_records.AuthenticationManagement
{
    public interface IUserAuthenticationService
    {



        Task<bool> SetAuthentication(Login_validation validationForm);



    }
}
