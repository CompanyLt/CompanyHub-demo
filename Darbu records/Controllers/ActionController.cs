using Darbu_records.Formos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Darbu_records.Singleton;
using Darbu_records.Query;
using Darbu_records.InstructionManagement;
using Darbu_records.OldItems;

namespace Darbu_records.Controllers
{
    public class ActionController : Controller
    {

        //singletonas
        private IrasuKiekis _irasukiekis;


        private int vartotojas { set; get; }
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger, Objekt objekt)
        //{
        //    this.objekt = objekt;
        //    _logger = logger;

        //}
        public ActionController(IrasuKiekis irasukiekis)
        {
            _irasukiekis = irasukiekis;

        }




        public IActionResult Index()
        {
           
            return View();
        }
        //view inicializavimas
        //public IActionResult gedimo_registravimas()
        //{
        //    //Gedimo_forma gedimo_forma = new Gedimo_forma();
        //    //sukuriam List<IFormFile> images
        //   // gedimo_forma.images_initializes();
          
        //    return View();
        //}
        //public IActionResult instrukcijos_registravimas()
        //{
        //    return View();
        //}

     

  

       

        //private void vartotojas_set()
        //{
        //    vartotojas = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //}

        //post metodai
        //[HttpPost]
        //public async Task <IActionResult> Add_Gedimas(Gedimo_forma gedimo_forma,List<IFormFile> images,IFormFile file)
        //{
        //    //ModelState.Remove(key: "file");
        //    //ModelState.Remove(key: "photos");
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    //MODELSTATE PATIKRA
        //    //    // var errors = ModelState
        //    //    //.Where(m => m.Value.Errors.Count > 0)
        //    //    //.Select(m => new
        //    //    //{
        //    //    //    Field = m.Key,
        //    //    //    Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToList()
        //    //    //}).ToList();

        //    //    // foreach (var error in errors)
        //    //    // {
        //    //    //     Console.WriteLine($"Field: {error.Field}");
        //    //    //     foreach (var err in error.Errors)
        //    //    //     {
        //    //    //         Console.WriteLine($"Error: {err}");
        //    //    //     }
        //    //    // }

        //    //    return View("gedimo_registravimas");
        //    //}

        //    //Nuotrauku issaugojimas
        //    //if (!Directory.Exists("wwwroot/incident/images"))
        //    //{
        //    //    Directory.CreateDirectory("wwwroot/incident/images");
        //    //}

        //    ////guid sukuria unikalu pavadinima+ prijungiam image pavadinima
        //    //if (images != null)
        //    //{
        //    //    gedimo_forma.images_initialize();
        //    //    foreach (var temp in images)
        //    //    {
        //    //        string name = (Guid.NewGuid().ToString() + temp.FileName);
        //    //        //cia sujungiam i kelia
        //    //        string kelias_image = Path.Combine("wwwroot", "incident","images", name);
        //    //        gedimo_forma.images.Add(name);
        //    //        //sukuriam filestream sukuriam faila 
        //    //        using (FileStream file_create = new FileStream(kelias_image, FileMode.Create))
        //    //        {

        //    //            await temp.CopyToAsync(file_create);

        //    //        }




        //    //    }

        //    //}


        //    //if (!Directory.Exists("wwwroot/incident/files"))
        //    //{
        //    //    Directory.CreateDirectory("wwwroot/incident/files");
        //    //}

        //    //if (file != null)
        //    //{
        //    //    string name = (Guid.NewGuid().ToString() + file.FileName);
        //    //    string file_path = Path.Combine("wwwroot", "incident", "files", name);
        //    //    gedimo_forma.file_name = name;
        //    //    using (FileStream file_create = new FileStream(file_path, FileMode.Create))
        //    //    {
        //    //        await file.CopyToAsync(file_create);
        //    //    }



        //    //}
        //    //else
        //    //{
              
        //    //    gedimo_forma.file_name = string.Empty;
        //    //}


        //    //vartotojas_set();
        //    //SqlQuery Dbstring = new SqlQuery();
        //    ////////perduodami duomenys i konstruktoriu
        //    /////
        //    //try
        //    //{
        //    //    IncidentService action = new IncidentService(Dbstring.dbConn,
        //    //                  Dbstring.addIncident,
        //    //                  vartotojas
        //    //                  );
        //    //    //////reikai prideti nuotrauka
        //    //    await action.Add_Gedimas(gedimo_forma);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //cia tik data
        //    //    //DateOnly data =DateOnly.FromDateTime(DateTime.Now);
        //    //    DateTime data = DateTime.Now;

        //    //    string path = "../Darbu records/log/";
        //    //    string logName = $"log_{data:yyyy-MM-dd_HH-mm-ss}.txt";
        //    //    string fullpath = Path.Combine(path1: path, path2: logName);

        //    //    if (!Directory.Exists(path: path))
        //    //    {
        //    //        Directory.CreateDirectory(path: path);


        //    //    }
        //    //    using (StreamWriter logcreate = new(fullpath))
        //    //    {
        //    //        logcreate.WriteLine($"Time: {data} Message: {ex.Message}");
        //    //    }


        //    //  // Console.WriteLine(ex.Message);

        //    //}



        // return RedirectToAction("Index", "Home");
           
        //}
        ////[HttpPost]
        //public async Task <IActionResult> Add_Instrukcija(Instrukcijos_forma forma, List<IFormFile> images, IFormFile file)
        //{




        //    //Nuotrauku issaugojimas
        //    if (!Directory.Exists("wwwroot/instruction/images"))
        //    {
        //        Directory.CreateDirectory("wwwroot/instruction/images");
        //    }

        //    //guid sukuria unikalu pavadinima+ prijungiam image pavadinima
        //    if (images != null)
        //    {
        //        forma.images_initialize();
        //        foreach (var temp in images)
        //        {
        //            string name = (Guid.NewGuid().ToString() + temp.FileName);
        //            Console.WriteLine(name);
        //            //cia sujungiam i kelia
        //            string kelias_image = Path.Combine("wwwroot", "instruction", "images", name);
        //           forma.images.Add(name);
        //            //sukuriam filestream sukuriam faila 
        //            using (FileStream file_create = new FileStream(kelias_image, FileMode.Create))
        //            {

        //                await temp.CopyToAsync(file_create);

        //            }




        //        }

        //    }




        //    if (!Directory.Exists("wwwroot/files/instruction"))
        //    {
        //        Directory.CreateDirectory("wwwroot/files/instruction");
        //    }
        //    if (file != null)
        //    {
        //        forma.file_name =(Guid.NewGuid() + file.FileName);
        //        string nuotraukos_kelias = Path.Combine("wwwroot", "files", "instruction", forma.file_name);
        //        using (FileStream stream = new FileStream(nuotraukos_kelias, FileMode.Create))
        //        {
        //          await  file.CopyToAsync(stream);
        //        }
        //    }
        //    else
        //    {
        //        forma.file_name = string.Empty;
        //    }


        //    int iraso_id = -1;
        //    vartotojas_set();
        //    SqlQuery query = new SqlQuery();
        //    InstructionService service = new InstructionService(query.dbConn, query.addInstruction, vartotojas);
        //   await service.add_instruction(forma);




        //    ////SqlQuery query = new SqlQuery(Dbstring.dbConn, Dbstring.Record_Add);
        //    ////query.conn;
        //    //using (SqlConnection connection = new SqlConnection(Dbstring.dbConn))
        //    //{
        //    //    connection.Open();
        //    //    using (SqlCommand command = new SqlCommand(Dbstring.Instrukcija_Add, connection))
        //    //    {
        //    //        //  Console.WriteLine("sdsdsds");
        //    //        command.Parameters.Add(new SqlParameter("@pavadinimas", forma.pavadinimas));
        //    //        command.Parameters.Add(new SqlParameter("@aprasymas", forma.aprasymas));
        //    //        command.Parameters.Add(new SqlParameter("@vartotojo_id", vartotojas));
        //    //        command.Parameters.Add(new SqlParameter("@nuotrauka", forma.file_name));

        //    //        iraso_id = (int)command.ExecuteScalar();



        //    //    }
        //    //    string query = "INSERT INTO Instrukcijos_statusas(Instrukcijos_id,Patinka,Nepatinka) VALUES(@id,@patinka,@nepatinka)";
        //    //    using (SqlCommand command = new SqlCommand(query, connection))
        //    //    {
        //    //        command.Parameters.Add(new SqlParameter("@id", iraso_id));
        //    //        command.Parameters.Add(new SqlParameter("@patinka", 1));
        //    //        command.Parameters.Add(new SqlParameter("@nepatinka", 1));
        //    //        command.ExecuteNonQuery();

        //    //    }


        //    //    connection.Close();
        //    //}


        //    return RedirectToAction("Index", "Home");
        //}


    }

}
