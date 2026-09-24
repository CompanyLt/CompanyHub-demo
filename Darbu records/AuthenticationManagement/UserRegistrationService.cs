using Darbu_records.Data;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Microsoft.Data.SqlClient;

namespace Darbu_records.AuthenticationManagement
{
    public class UserRegistrationService : IUserRegistrationService
    {
        IQueryService _queryService;

        public UserRegistrationService([FromKeyedServices("UserRegistrationQueryService")]IQueryService queryService) 
        {
            _queryService = queryService;
        }


        public async Task<bool> AddUser(Validation validation) 
        {

            using (SqlConnection connection = _queryService.sqlConnGet())
            {
               await connection.OpenAsync();

                using (SqlCommand userCheck = new SqlCommand(_queryService.queryActionGet(), connection))
                {

                    userCheck.Parameters.Add(new SqlParameter("@email", validation.Email));
                    int count_login = (int)userCheck.ExecuteScalar();


                    //Console.WriteLine($"patikra {count_login.ToString()}");
                    if (count_login == 0)
                    {
                        _queryService.queryActionSet("next query");
                        using (SqlCommand write = new SqlCommand(_queryService.queryActionGet(), connection))
                        {
                            write.Parameters.Add(new SqlParameter("@name", validation.Name));
                            write.Parameters.Add(new SqlParameter("@password", validation.Password));
                            write.Parameters.Add(new SqlParameter("@email", validation.Email));

                          await  write.ExecuteNonQueryAsync();
                        }
                     await   connection.CloseAsync();
                        return true;
                    }
                    else
                    {
                      await  connection.CloseAsync();
                        return false;


                    }
                }



            }


        }
    }
}
