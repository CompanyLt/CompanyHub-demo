using Darbu_records.Formos;
using Microsoft.Data.SqlClient;


namespace Darbu_records.OldItems
{
    public class Quest_Query_Action
    {
        private string dbconn { get; set; }
        private string query_action { get; set; }

        private SqlConnection Sqlconn;

        private int _id { get; set; }

        public List<IncidentForm> _records = new List<IncidentForm>();
        public List<InstructionForm> _instrukcijos = new List<InstructionForm>();

        //Konstruktoriai
        public Quest_Query_Action(string conn, string comm, int user)
        {
            _id = user;
            dbconn = conn;
            query_action = comm;
            Sqlconn = new SqlConnection(conn);


        }
        public Quest_Query_Action(string conn, string comm)
        {
            dbconn = conn;
            query_action = comm;
            Sqlconn = new SqlConnection(conn);

        }
        public Quest_Query_Action()
        {

        }
        //geteriai-------------------------------------
        public int Client_get()
        {
            return _id;
        }

        public void query_keitimas(string _query)
        {
            query_action = _query;
        }


        public void Gedimai()
        {
            Sqlconn.Open();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
#nullable disable

                    while (reader.Read())
                    {
                        // Console.WriteLine("sdsd");
                        IncidentForm Gedimo_forma = new IncidentForm(
                            reader["List"].ToString(),
                            reader["Description"].ToString(),
                            Convert.ToInt32(reader["Record_Id"]),
                            reader["Solution"].ToString()

                            );

                        _records.Add(Gedimo_forma);
                        //  Console.WriteLine(reader["List"].ToString());




                    }
#nullable enable




                }




            }


        }

        public async Task Instrukcijos()
        {

            using (SqlConnection connection = new SqlConnection(dbconn))
            {


                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query_action, connection))
                {




                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {

#nullable disable
                            InstructionForm _instrukcijos_forma = new InstructionForm(
                                reader["title"].ToString(),
                                reader["description"].ToString(),
                                Convert.ToInt32(reader["instruction_id"]),
                                Convert.ToInt32(reader["Patinka"]),
                                reader["file_name"].ToString()

                                );

                            _instrukcijos.Add(_instrukcijos_forma);
                            //  Console.WriteLine(reader["List"].ToString());



#nullable enable
                        }



                    }


                }
                await connection.CloseAsync();
            }



        }


        public void Search_Note_Quest(string search)
        {


            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {

                    // command.Parameters.Add(new SqlParameter("id", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {


                            int index = 0;
#nullable disable
                            foreach (var n in reader["List"].ToString())
                            {

                                if (search[index] == n && search.Length >= 3)
                                {

                                    index += 1;
                                }
                                else
                                {

                                    index = 0;
                                }
                                if (index == 3)
                                {

                                    IncidentForm Gedimo_forma = new IncidentForm(
                                        reader["List"].ToString(),
                                reader["Description"].ToString(),
                                Convert.ToInt32(reader["Record_Id"]),
                                reader["Solution"].ToString()
                              );
                                    _records.Add(Gedimo_forma);
#nullable enable
                                    break;
                                }

                            }






                        }


                    }

                }

            }

            finally { Sqlconn.Close(); }





        }

        public void Search_instrukcija_quest(string search)
        {


            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {


                            int index = 0;
#nullable disable
                            foreach (var n in reader["title"].ToString())
                            {

                                if (search[index] == n && search.Length >= 3)
                                {

                                    index += 1;
                                }
                                else
                                {

                                    index = 0;
                                }
                                if (index == 3)
                                {

                                    InstructionForm _instrukcijos_forma = new InstructionForm(
                                reader["title"].ToString(),
                                reader["description"].ToString(),
                                Convert.ToInt32(reader["instruction_id"]),
                                Convert.ToInt32(reader["Patinka"]),
                                reader["Nuotrauka"].ToString()

                                );
                                    _instrukcijos.Add(_instrukcijos_forma);
#nullable enable
                                    break;
                                }

                            }






                        }
                    }

                }

            }

            finally { Sqlconn.Close(); }





        }

        public async Task bring_list_quest()
        {

            using (SqlConnection connection = new SqlConnection(dbconn))
            {


                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query_action, connection))
                {


                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
#nullable disable

                        while (await reader.ReadAsync())
                        {
                            // Console.WriteLine("sdsd");
                            IncidentForm Gedimo_forma = new IncidentForm(
                                reader["List"].ToString(),
                                reader["Description"].ToString(),
                                Convert.ToInt32(reader["Record_Id"]),
                                reader["Solution"].ToString()

                                );

                            _records.Add(Gedimo_forma);
                            //  Console.WriteLine(reader["List"].ToString());




                        }
#nullable enable


                    }


                }
                await connection.CloseAsync();
            }



        }




    }
}
