
using Darbu_records.Data;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Darbu_records.Models;
using Darbu_records.Query;
using Darbu_records.OldItems;
using Microsoft.Data.SqlClient;
using Darbu_records.DAL;
using Darbu_records.Singleton;
using System.Text.Json;
using Darbu_records.Interfaces;
using Darbu_records.UserManagement;
using Darbu_records.AuthenticationManagement;
using FileCheck = System.IO.File;

namespace Darbu_records.Controllers
{
  
    public class AccessController : Controller
    {
  
        IUserAuthenticationService _userAuthenticationService;
        IUserRegistrationService _userRegistrationService;

        public AccessController(      
            IUserAuthenticationService userAuthenticationService,
            IUserRegistrationService userRegistrationService)
        {
                   
            _userAuthenticationService = userAuthenticationService;
            _userRegistrationService = userRegistrationService;
        }


        public IActionResult Index()
        {
         

            return View();
        }
        public IActionResult Index_Error(Login_validation validation)
        {
        
            return View(validation);
        }


        public IActionResult Error()
        {
           
          
            return Redirect("Index");
           

        }

       
        public IActionResult Login()
        {
           
            
           
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> user_login(Login_validation validation)
        {

            if (!ModelState.IsValid)
            {
             return View("Login", validation);
            }
          //  Console.WriteLine("sdsds");
            //IKELIAM I AUTHENTICATION
            bool isAuthenticate = await _userAuthenticationService.SetAuthentication(validation);         
            if (isAuthenticate)
                {
              
                return RedirectToAction("Index", "Home");
                // return RedirectToAction(nameof(SetWorker));
            }
                                
            return View("Login", validation);
        }




        public IActionResult RoleCheck()
        {
          

              //  Console.WriteLine(User.FindFirstValue(ClaimTypes.NameIdentifier));
                // Patikriname role per Claims
                if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
                {
                    // Nukreipiame į Admin Area
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

            if (User.FindFirstValue(ClaimTypes.Role) == "User")
            {
                // Nukreipiame į Admin Area
                return RedirectToAction("Index", "Dashboard", new { area = "Member" });
            }

            // Paprastam user nukreipiame į Home
            return RedirectToAction("Index", "Home");
            




        }
           
                         

        //[HttpGet]
        //public async Task<IActionResult> SetWorker()
        //{
        //    //PRIDEDAM WORKER I SESIJA
        //    await _workerLoader.loadWorker(_worker, Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        //    await _workerLoader.loadAchievment(_worker);

        //    _workerSessionService.SetWorker();          
        //    return RedirectToAction(nameof(SetDepartment),new { _worker.DepartmentId });
        //}






        [HttpGet("/Access/GetFile")]
        public IActionResult GetIncidentFile(string url,string directory)
        {
                    //GetFileName is pilno kelio paima tik pavadinima
            string fileName = Path.GetFileName(url);

            string filePath = Path.Combine(directory, fileName);

      
            //cia sukurta using FileCheck = System.IO.File kuris tikrina ar yra toks failas
            if(!FileCheck.Exists(filePath))
            {
                return NotFound();
            }

            FileStream file = new FileStream(filePath,FileMode.Open, FileAccess.Read);

             string contentType = "application/octet-stream";
            



            return File(file,contentType,fileName);
        }

        [HttpGet("/Access/GetPdfFile")]
        public IActionResult GetPdfFile(string url, string directory)
        {
        

            //GetFileName is pilno kelio paima tik pavadinima
            string fileName = Path.GetFileName(url);

            string filePath = Path.Combine(directory, fileName);


            //cia sukurta using FileCheck = System.IO.File kuris tikrina ar yra toks failas
            if (!FileCheck.Exists(filePath))
            {
                return NotFound();
            }

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // string contentType = "application/octet-stream";
            string contentType = "application/pdf";



            return File(file, contentType);
        }



        //[HttpGet]
        //public async Task<IActionResult> SetDepartment(int departmentId)
        //{        
        //    await _navigationLoader.LoadGroups(_appInfo.groups, departmentId);
        //    await _navigationLoader.LoadCategory(_appInfo.categories, departmentId);

        //    //Serializacija i sesija
        //    HttpContext.Session.SetString("AppInfo", JsonSerializer.Serialize(_appInfo));



        //    return RedirectToAction("Index", "Home");
        //}
        //akaunto sukurimas
        [HttpPost]
        public async Task <IActionResult> user_registration(Validation validation)
        {
            
            ///PATIKROS
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            if (validation.Password != validation.Password_repeat)
            {
                ModelState.AddModelError("Password_repeat", "Slaptažodžiai turi sutapti.");
                return View("Index", validation);
            }
            //////////////////////////////////////////////
            bool addUser = await _userRegistrationService.AddUser(validation);

            if (addUser)
            {
                return View("Login");
            }

            return View("Index");




            //    SqlQuery Dbstring = new SqlQuery();
            // using (SqlConnection connection = new SqlConnection(Dbstring.dbConn))
            // {
            //    connection.Open();

            //    using (SqlCommand userCheck = new SqlCommand(Dbstring.UserCheck, connection))
            //    {

            //        userCheck.Parameters.Add(new SqlParameter("@email",validation.Email));
            //        int count_login = (int)userCheck.ExecuteScalar();


            //            //Console.WriteLine($"patikra {count_login.ToString()}");
            //        if(count_login == 0 )
            //        {
            //            using (SqlCommand write = new SqlCommand(Dbstring.Add_App, connection))
            //            {
            //                write.Parameters.Add(new SqlParameter("@name", validation.Name));
            //                write.Parameters.Add(new SqlParameter("@password", validation.Password));
            //                write.Parameters.Add(new SqlParameter("@email", validation.Email));

            //                write.ExecuteNonQuery();
            //            }
            //           connection.Close();  
            //            return View("Login");
            //        }else
            //        {
            //                connection.Close();



            //        }             
            //    }



            //}

        }

            
          
          




        public async Task <IActionResult> logout()
        {
            HttpContext.Session.Clear();
           

          await  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);         
            return RedirectToAction("Index", "Home");
        }
    }
}
