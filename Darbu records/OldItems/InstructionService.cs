using Darbu_records.Formos;
using Darbu_records.OldItems;
using System.Data;
using Microsoft .Data.SqlClient;

namespace Darbu_records.OldItems
{
    public class InstructionService : Query_Action
    {



        public InstructionService(string connection, string querycommand, int userid) : base(connection, querycommand, userid)
        {
        }



        public void updateInstruction(string name, string description)
        {

            try
            {
                Sqlconn.Open();
                using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
                {
                    command.Parameters.Add(new SqlParameter("@name", name));
                    command.Parameters.Add(new SqlParameter("@description", description));
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


        public async Task bring_instruction_view(string category)
        {




            await Sqlconn.OpenAsync();
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@category", category));
                command.Parameters.Add(new SqlParameter("@id", _id));
                command.Parameters.Add(new SqlParameter("@status", 1));

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
#nullable disable
                        InstructionForm _instrukcijos_forma = new InstructionForm(
                            reader["title"].ToString(),
                            reader["description"].ToString(),
                            Convert.ToInt32(reader["instruction_id"]),
                            Convert.ToInt32(reader["Patinka"])
                            //reader["file_name"].ToString()

                            );

                        _instrukcijos.Add(_instrukcijos_forma);
                        //  Console.WriteLine(reader["List"].ToString());



#nullable enable
                    }



                }


            }
            await Sqlconn.CloseAsync();




        }

        public async Task<InstructionForm> bring_instruction_review()
        {
            InstructionForm? forma = null;
            await Sqlconn.OpenAsync();
            Console.WriteLine(dbconn);
            using (SqlCommand command = new SqlCommand(query_action, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@instruction_id", SqlDbType.Int) { Value = _id });
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });



                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        forma = new InstructionForm(
                           reader["title"].ToString() ?? string.Empty,
                           reader["description"]?.ToString() ?? string.Empty,
                           reader["instruction_id"] != DBNull.Value ? Convert.ToInt32(reader["instruction_id"]) : 0,
                           reader["Patinka"] != DBNull.Value ? Convert.ToInt32(reader["Patinka"]) : 0,
                           reader["file_name"]?.ToString() ?? string.Empty
                            );



                    }




                }

            }
            await Sqlconn.CloseAsync();
            //graziname forma jei null tai sukuriame tuscia
            return forma ?? new();
        }



        public async Task add_instruction(InstructionForm instructionForm)
        {
            int iraso_id = -1;

            await Sqlconn.OpenAsync();

            using (SqlCommand command = new SqlCommand(cmdText: query_action, connection: Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@pavadinimas", instructionForm.pavadinimas));
                command.Parameters.Add(new SqlParameter("@aprasymas", instructionForm.aprasymas));
                command.Parameters.Add(new SqlParameter("@vartotojo_id", _id));
                command.Parameters.Add(new SqlParameter("@file_name", instructionForm.file_name));
                command.Parameters.Add(new SqlParameter("@status", 1));
                iraso_id = (int)(await command.ExecuteScalarAsync() ?? 0);






            }
            //Patinka sql irasas
            string query = "INSERT INTO Instrukcijos_statusas(Instrukcijos_id,Patinka,Nepatinka) VALUES(@id,@patinka,@nepatinka)";
            using (SqlCommand command = new SqlCommand(query, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@id", iraso_id));
                command.Parameters.Add(new SqlParameter("@patinka", 1));
                command.Parameters.Add(new SqlParameter("@nepatinka", 1));
                await command.ExecuteNonQueryAsync();

            }

            //kategorijos idejimas
            query = "INSERT INTO InstructionCategory(instruction_id,category_id) VALUES(@instruction_id, @category_id)";
            using (SqlCommand command = new(query, Sqlconn))
            {
                command.Parameters.Add(new SqlParameter("@instruction_id", iraso_id));
               // command.Parameters.Add(new SqlParameter("@category_id", category_initialize(instructionForm.categoryId ?? "0")));
                await command.ExecuteNonQueryAsync();
            }


            //cia prideda nuotruakas
            //if (instructionForm.images == null) return;
            query = "INSERT INTO photos_instructions(instruction_id,file_name,upload_date) VALUES(@instruction_id,@file_name,GETDATE())";
            //foreach (var temp in instructionForm.images)
            //{
            //    using (SqlCommand command = new SqlCommand(query, Sqlconn))
            //    {
            //        command.Parameters.Add(new SqlParameter("@instruction_id", iraso_id));
            //        command.Parameters.Add(new SqlParameter("@file_name", temp));

            //        await command.ExecuteNonQueryAsync();
            //    }



            //}


            await Sqlconn.CloseAsync();






        }






    }
}
