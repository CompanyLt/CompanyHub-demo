using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Microsoft.Data.SqlClient;

namespace Darbu_records.Query.Instruction
{
    public class InstructionViewQueryService : IQueryService
    {
        IConnectionRepository _connectionRepository;


        private string queryAction2 = @"Select
        I.title,
        I.description,
        I.id,
        I.category_id,
        CA.department_access
        FROM 
        Instruction AS I
        INNER JOIN 
        CategoryAccess AS CA
        ON I.category_id = CA.category_id            
        WHERE I.category_id = @category AND CA.department_id = @department_id AND I.status=@status AND CA.department_access=@department_access";


        //Cia su arba logika naudosiu
        private string queryAction = @"
                        SELECT
                        I.title,
                        I.description,
                        I.id,
                        I.category_id,
                        COALESCE(CA.department_access, GA.department_access, 0) AS department_access
                    FROM Instruction AS I
                    INNER JOIN Category AS C
                        ON I.category_id = C.id
                    LEFT JOIN CategoryAccess AS CA
                        ON I.category_id = CA.category_id
                        AND CA.department_id = @department_id
                        AND CA.department_access = @department_access
                    LEFT JOIN GroupAccess AS GA
                        ON GA.group_id = C.group_id
                        AND GA.department_id = @department_id
                        AND GA.department_access = @department_access
                    WHERE I.category_id = @category
                      AND I.status = @status
                      AND (
                            CA.category_id IS NOT NULL
                         OR GA.group_id IS NOT NULL
                      );";

        //SENAS
        //private string queryAction = @"Select
        //I.title,
        //I.description,
        //I.id,
        //K.likes_count
        //FROM 
        //Instruction AS I
        //INNER JOIN 
        //InstructionCategory ic ON I.instruction_id = ic.instruction_id
        //INNER JOIN
        //Category c ON ic.category_id = c.id
        //INNER JOIN
        //Instruction_status AS K ON I.instruction_id = K.instruction_id
        //WHERE c.id = @category AND I.department_id = @department_id AND I.status=@status";
        public InstructionViewQueryService(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }





        public string dbConnGet()
        {
            throw new NotImplementedException();
        }

        public string dbQueryGet()
        {
            throw new NotImplementedException();
        }



        public string queryActionGet()
        {
            return queryAction;
        }

        public void queryActionSet(string query)
        {
            queryAction = query;
        }

        public SqlConnection sqlConnGet()
        {
            return _connectionRepository.getSqlConnection();
        }

        public int idGet()
        {
            return 0;

        }
    }
   
}
