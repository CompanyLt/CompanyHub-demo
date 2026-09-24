using Darbu_records.Formos;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace Darbu_records.OldItems
{
    public class IncidentService : Query_Action
    {



        public IncidentService(string connection, string querycommand, int userid) : base(connection, querycommand, userid)
        {







        }


        //surenka visa akaunto irasu sarasa i List<Record>
        public async Task bring_incident_view(string category)
        {




            await Sqlconn.OpenAsync();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {

                command.Parameters.Add(new SqlParameter("@id", _id));
                command.Parameters.Add(new SqlParameter("@category", category));
                command.Parameters.Add(new SqlParameter("@status", 1));

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
            await Sqlconn.CloseAsync();




        }


        //jei metodas asinchroninis tai ir kontroleryje turi buti asinchroninis priesingu atveju neveikia
        //iraso duomenu surinkimo metodas, apimantis ir kitas lenteles
        public async Task<IncidentForm> bring_incident_review()
        {
            IncidentForm? gedimo_forma = null;

            await Sqlconn.OpenAsync();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@id", _id));
                command.Parameters.Add(new SqlParameter("@status", 1));

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    int current_postid = -1;

                    while (await reader.ReadAsync())
                    {
                        int post_id = Convert.ToInt32(reader["Record_Id"]);



                        if (current_postid != post_id)
                        {
                            // Console.WriteLine(post_id);
                            gedimo_forma = new IncidentForm(
                            reader["List"].ToString(),
                            reader["Description"].ToString(),
                            Convert.ToInt32(reader["Record_Id"]),
                            reader["Solution"].ToString(),
                            reader["incident_file_name"].ToString()
                            );

                            gedimo_forma.photos_initialize();
                            current_postid = post_id;
                        }

                        //cia sudeda viska i konteineri
                        if (gedimo_forma != null && !reader.IsDBNull(reader.GetOrdinal("file_name")))
                        {
                            // Console.WriteLine($"Action file_name: {reader["file_name"]}");
                            // Console.WriteLine(reader["file_name"].ToString());
                            Photos photo = new Photos()
                            {
                                name = reader.GetString(reader.GetOrdinal("file_name")),
                                upload_date = reader.GetDateTime(reader.GetOrdinal("upload_date")),
                                directory = reader.GetString(reader.GetOrdinal("directory"))

                            };
                            gedimo_forma.photos.Add(photo);

                        }



                    }






                }


                await Sqlconn.CloseAsync();
            }
            //foreach (var items in _records)
            //{
            //    Console.WriteLine($"irasas{items.iraso_id}");
            //    foreach (var p in items.photos)
            //    {
            //        Console.WriteLine($"irasas{p.name} data:{p.upload_date}");
            //    }
            //}

            return gedimo_forma ?? new();
        }




        //INCIDENTU ATNAUJINIMAS
        public void Record_Update(string name, string description, string solution)
        {

            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {
                    command.Parameters.Add(new SqlParameter("@name", name));
                    command.Parameters.Add(new SqlParameter("@description", description));
                    command.Parameters.Add(new SqlParameter("@solution", solution));
                    command.Parameters.Add(new SqlParameter("@id", _id));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("klaida" + e.Message);
                throw;
                //tuscia, galima realizacija                 
            }
            finally
            {
                Sqlconn.Close();
            }
        }








        //Gedimo pridejimas
        public async Task Add_Gedimas(IncidentForm gedimo_forma)
        {
            //Console.WriteLine(gedimo_forma.images.Any());
            //foreach(var i in gedimo_forma.images)
            //{
            //    Console.WriteLine(i.ToString());
            //}
            int iraso_id = -1;
            await Sqlconn.OpenAsync();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                int status = 1;
                command.Parameters.Add(new SqlParameter("@name", gedimo_forma.pavadinimas));
                command.Parameters.Add(new SqlParameter("@client_id", _id));
                command.Parameters.Add(new SqlParameter("@description", gedimo_forma.aprasymas));
                command.Parameters.Add(new SqlParameter("@solution", gedimo_forma.sprendimas));
                command.Parameters.Add(new SqlParameter("@file_name", gedimo_forma.file_name));

                command.Parameters.Add(new SqlParameter("@status", status));
                //paimam is query uzklausos OUTPUT inserted.id

                iraso_id = (int)(await command.ExecuteScalarAsync() ?? 0);



            }
            //qurry kuris prideda incidento kategorija categiry_initiize tai metodas kuris paskaiciuoja kuri kategorija ir grazina int
            string action_query = "INSERT INTO IncidentCategory(incident_id, category_id) VALUES(@incident_id,@category_id)";
            using (SqlCommand command = new SqlCommand(action_query, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@incident_id", iraso_id));
                command.Parameters.Add(new SqlParameter("@category_id", category_initialize(gedimo_forma.category ?? "0")));
                await command.ExecuteNonQueryAsync();
            }

            //cia prideda nuotruakas
            if (gedimo_forma.images == null) return;
            string query = "INSERT INTO photos_incident(post_id,file_name,upload_date) VALUES(@post_id,@file_name,GETDATE())";
            foreach (var temp in gedimo_forma.images)
            {
                using (SqlCommand command = new SqlCommand(query, Sqlconn))
                {
                    command.Parameters.Add(new SqlParameter("@post_id", iraso_id));
                    command.Parameters.Add(new SqlParameter("@file_name", temp));

                    await command.ExecuteNonQueryAsync();
                }



            }


            //sukurdami gedima sukuriam ir komentaru lentele sitam gedimui
            //string query = "INSERT INTO Gedimo_komentarai(Gedimo_id)VALUES(@id)";
            //using (SqlCommand command = new SqlCommand(query, Sqlconn))
            //{
            //    command.Parameters.Add(new SqlParameter("@id", iraso_id));


            //    command.ExecuteNonQuery();

            //}



            await Sqlconn.CloseAsync();

        }


    }
}
