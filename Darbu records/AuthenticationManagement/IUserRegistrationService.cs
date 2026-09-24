using Darbu_records.Data;

namespace Darbu_records.AuthenticationManagement
{
    public interface IUserRegistrationService
    {



        Task<bool> AddUser(Validation validation);


    }
}
