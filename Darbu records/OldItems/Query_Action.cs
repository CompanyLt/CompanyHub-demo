using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using Darbu_records.Formos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Darbu_records.OldItems
{
    //cia buvo Query_Action:SqlQuery
    public class Query_Action
    {
        protected string dbconn { get; set; }
        protected string query_action { get; set; }

        protected SqlConnection Sqlconn;

        protected int _id { get; set; }

        public int _departmentId { get; set; }  

        public string role { get; set; }    


        public string queryActionGet()
        {
            return query_action;
        }

        public string dbConnGet()
        {
            return dbconn;
        }

        public SqlConnection sqlConnGet()
        {
            return Sqlconn;
        }

        public List<IncidentForm> recordsGet()
        {
            return _records;
        }
        public int idGet()
        {
            return _id;
        }




        public List<InstructionForm> _instrukcijos = new List<InstructionForm>();
        public List<IncidentForm> _records = new List<IncidentForm>();


        public Query_Action()
        {

        }



        //Konstruktoriai
        public Query_Action(string connection, string querycommand, int userid)
        {
            _id = userid;
            dbconn = connection;
            query_action = querycommand;
            Sqlconn = new SqlConnection(connection);


        }
        public void query_change(string _query)
        {
            query_action = _query;
        }


        public Query_Action(string conn, string comm)
        {
            dbconn = conn;
            query_action = comm;
            Sqlconn = new SqlConnection(conn);

        }
        //geteriai-------------------------------------
        public int Client_get()
        {
            return _id;
        }

        public int Department_get()
        {
            return _departmentId;
        }


        public string Role_get()
        {
            return role;
        }


        //----------------------------------------------
        //Metodai-------------------------------------------------------------
        //trina pasirinkta sarasa
        public void Record_Delete(int index)
        {


            Sqlconn.Open();
            using (SqlCommand comand = new SqlCommand(query_action, Sqlconn))
            {
                int n = 0;
                comand.Parameters.Add(new SqlParameter("@Check_Id", index));
                comand.Parameters.Add(new SqlParameter("@status", n));


                comand.ExecuteNonQuery();
            }
            Sqlconn.Close();





        }





        //Iesko ar yra toks klientas
        public bool Client_check(string Name, string Pass)
        {

           
           
            Sqlconn.Open();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
              
                command.Parameters.Add(new SqlParameter("@name", Name));
                command.Parameters.Add(new SqlParameter("@password", Pass));

               
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() == true)
                    {
                       // Console.WriteLine(reader["id"]);
                        _id = Convert.ToInt32(reader["id"]);
                        _departmentId = Convert.ToInt32(reader["DepartmentId"]);
                        role = reader["role_name"].ToString();
                        Sqlconn.Close();
                        return true;


                    }
                    else
                    {
                        Sqlconn.Close();
                        return false;
                    }

                }

            }


        }








        public void patinka_pridejimas(int iraso_id, int patinka_kiekis)
        {
            Console.WriteLine(iraso_id.ToString());
            Console.WriteLine(patinka_kiekis.ToString());
            Sqlconn.Open();

            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@id", iraso_id));
                command.Parameters.Add(new SqlParameter("@skaicius", patinka_kiekis += 1));

                command.ExecuteNonQuery();
            }
            Sqlconn.Close();
        }


        //reikalingas tam kad galima butu nukreipti sudetingesne logika pvz group 1-sco group 2-kasa category-svarstykles
        public static int category_initialize(string parameter)
        {
            switch (parameter)
            {
                case "svarstykles":
                    return 1;
                case "spausdintuvai":
                    return 6;
                case "skaneriai":
                    return 4;
                case "bankiniai":
                    return 5;
                case "iEKA":
                    return 3;
                case "RR":
                    return 10;
                default:
                    return 7;





            }





        }


    }

}
