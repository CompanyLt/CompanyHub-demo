using Darbu_records.OldItems;
using Microsoft.AspNetCore.Mvc;

namespace Darbu_records.Controllers
{
    public class SveciasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search_Note(string search)
        {
            Console.WriteLine(search);
            try
            {
                 if(search!=null && search.Length>0)
                {
              
                    Quest_SqlQuery query = new Quest_SqlQuery();

                    Quest_Query_Action action = new Quest_Query_Action(query.dbConn, query.Record_check_quest);
               
                    action.Search_Note_Quest(search);
                         foreach (var n in action._records)
                        {
                            Console.WriteLine($"Vardas: {n.pavadinimas}");
                        }
                        action.query_keitimas(query.Instrukcijos_all);
                    action.Search_instrukcija_quest(search);
                  
                return View("Index",action);

                }
            Quest_Query_Action action_empty = new Quest_Query_Action();
                return View("Index",action_empty);

            }catch(Exception ex)
            {
                DateTime data = DateTime.Now;
                string path = "../Darbu records/log/Query_Action_log/";
                string name = $"log_{data:yyyy-MM-dd_hh-mm-ss}.txt";
                string fullpath = Path.Combine(path1: path, path2: name);
                if (!Directory.Exists(path: path))
                {
                    Directory.CreateDirectory(path);
                }
                using(StreamWriter sw = new(path:fullpath))
                {
                    sw.WriteLine($"Time: {0}, Exception: {1}",
                        arg0: data,
                        arg1: ex.Message);




                }



                return View("Index");
            }
            

          
        }

        public async Task<IActionResult> Index_gedimai()
        {
                 
                // int id_app = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ;
                Quest_SqlQuery query = new Quest_SqlQuery();


                //i query action konstrktoriu perduodam 3 argumentus sqlconection, query action ir id kliento
                Quest_Query_Action action = new Quest_Query_Action(query.dbConn, query.Gedimai);


                await action.bring_list_quest();
                return View("Index", action);
            

           
        }

        public async Task<IActionResult> Index_instrukcijos()
        {
            Quest_SqlQuery query = new Quest_SqlQuery();
            Quest_Query_Action action = new Quest_Query_Action(query.dbConn, query.Instrukcijos_all);
           await action.Instrukcijos();



            return View("Index",action);
        }

    }
}
