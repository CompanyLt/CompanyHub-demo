using Microsoft.Data.SqlClient;
using Darbu_records.Formos;
using Darbu_records.OldItems;

namespace Darbu_records.SearchManagement
{
    public class SearchService : Query_Action
    {

        public SearchService(string connection, string querycommand, int userid) : base(connection, querycommand, userid)
        {




        }


        //INCIDENTU PAIESKA
        public void search_incident(string search, int id)
        {


            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {
                    int status = 1;
                    command.Parameters.Add(new SqlParameter("id", id));
                    command.Parameters.Add(new SqlParameter("@status", status));
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
        //INSTRUCKIJOS PAIESKA
        public void search_instruction(string search, int id)
        {


            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {

                    command.Parameters.Add(new SqlParameter("id", id));
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
                                reader["file_name"].ToString()

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






    }
}
