using Darbu_records.Data;
using Darbu_records.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
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


namespace Darbu_records.Controllers
{
    public class SearchController : Controller
    {
        private int vartotojas { set; get; }
        // private IrasuKiekis _irasukiekis;
       // private AppInfo appsettings;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger, Objekt objekt)
        //{
        //    this.objekt = objekt;
        //    _logger = logger;

        //}
        //public SearchController(AppInfo _appsettings)
        //{
        //    appsettings = _appsettings;

        //}

        private void vartotojas_set()
        {

            vartotojas = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }


        
        public async Task<IActionResult> search_notes(string search, int id)
        {
            if (string.IsNullOrEmpty(search))
            {
                return View(new TaskForm());
            }
            TaskForm taskForm = new TaskForm()
            {
                searchText = search,
                incidentCollection = new List<IncidentForm>(),
                instructionCollection = new List<InstructionForm>()
            };
            SqlQuery query = new SqlQuery();
            IQueryService queryService = new SearchQueryService(query.dbConn,query.incidentSearchQuery,id);
            INoteTaskService taskService = new IncidentSearchService(queryService);
            SearchCenter searchCenter = new SearchCenter(taskService, queryService);
            await searchCenter.taskExecute(taskForm);
            //sukuriame kita uzduoti
            queryService.queryActionSet(query.instructionSearchQuery);
            taskService = new InstructionSearchService(queryService);
         //butinai turi inicializuoti nauja uzduyotiu nes priesingu atveju nuoroda i senus
            searchCenter.changeTaskService(taskService,queryService);
            await searchCenter.taskExecute(taskForm);

            return View(taskForm);






        }
    }
}
