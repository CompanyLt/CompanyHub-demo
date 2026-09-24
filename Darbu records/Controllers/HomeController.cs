using Darbu_records.Data;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Darbu_records.SearchManagement;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Darbu_records.Formos;
using System;
using Darbu_records.Singleton;
using Darbu_records.Query;
using System.ComponentModel;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.InstructionManagement;
using Darbu_records.OldItems;
using Darbu_records.SearchManagement;
using Microsoft.AspNetCore.Authorization;
using Darbu_records.DAL;

namespace Darbu_records.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
            
    
        NavigationLoader _appInfoService;

        public HomeController(NavigationLoader appInfoService)
        {
            _appInfoService = appInfoService;
           // _navigationLoaderCache = navigationLoaderCache;
        }



        //kiekviena karta kai pasileidzia Index yra nunulinama _record listas
        //visas sarasas
        public IActionResult Index()
        {
          //  Console.WriteLine($" sdsdsds {User.FindFirstValue(ClaimTypes.NameIdentifier)}");
            // Console.WriteLine(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //var ipadress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //    Console.WriteLine(ipadress);






            return View();
        }


        public IActionResult GroupIncidentView()
        {

            var group = _appInfoService.Get();

            return View(group.groups);
        }

        

        //public IActionResult GroupNoteView()
        //{

        //    var group = _appInfoService.Get();

        //    return View(group.groups);
        //}




        public IActionResult CategoriesIncidentView(int categoryId)
        {
            var filtered = _appInfoService.Get().categories.Where(c => c.GroupId == categoryId).ToList();
           

            return View(filtered);
        }

		


           
        
        //perduodam id ir patinka kieki
        public IActionResult Patinka_pridejimas(string pavadinimas,string aprasymas,int id,int patinka)
        {
          //  Console.WriteLine($"instrkcijos iraso id {id.ToString()}");
            //cia galima perdaryti konstrukroiu
            SqlQuery query = new SqlQuery();
            Query_Action action = new Query_Action(query.dbConn, query.patinka_pridejimas, id);
            action.patinka_pridejimas(id, patinka);

             InstructionForm forma = new InstructionForm(pavadinimas, aprasymas, id, patinka+=1,"");


            return View("Instrukcijos_info",forma);
        }

        public IActionResult nav_bar()
        {
           
           return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Rendernews()
        {



            return PartialView("news");
        }



       
    }
}