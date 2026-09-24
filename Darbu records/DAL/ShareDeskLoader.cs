using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.ShareDesk;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Darbu_records.DAL
{
    public class ShareDeskLoader
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IQueryService _queryService;
        WorkerLoader _workerLoaderCache;



        public ShareDeskLoader(IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, [FromKeyedServices("CollectGroupQuery")] IQueryService queryService, WorkerLoader workerLoaderCache)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _queryService = queryService;
            _workerLoaderCache = workerLoaderCache;
        }



        public async Task<ShareDesk> GetAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new ShareDesk(); }

            string query = @"SELECT           
            SD.id,
            SD.title,
            SD.description,
            SD.department,
            SD.share_id,
            SD.department_access,
            SD.department_id,
            SDC.asp_controller,
            SDC.asp_action
        FROM ShareDesk SD
        LEFT JOIN ShareDeskConfig SDC
            ON SDC.id = SD.config_id             
        WHERE SD.department_id = @department_id;";

            var worker = await _workerLoaderCache.GetAsync();
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cachKey = $"shareDesk:{userId}";

            //cia jis patikrina ar cachKey yra jei ne tai sukuria kita
            var shareDesk = await _memoryCache.GetOrCreateAsync(cachKey, async entry =>
            {
               ShareDesk w = new();
                await LoadShareDesk(w.Items, worker,query);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return w;
            });

            return shareDesk;
        }

        public async Task<ShareDesk> GetSharedAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new ShareDesk(); }

            string query = @"SELECT           
            SD.id,
            SD.title,
            SD.description,
            SD.department,
            SD.share_id,
            SD.department_access,
            SD.department_id,
            SDC.asp_controller,
            SDC.asp_action
        FROM ShareDesk SD
        LEFT JOIN ShareDeskConfig SDC
            ON SDC.id = SD.config_id             
        WHERE SD.department_access = @department_id ;";

            var worker = await _workerLoaderCache.GetAsync();
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cachKey = $"shareAssignDesk:{userId}";

            //cia jis patikrina ar cachKey yra jei ne tai sukuria kita
            var shareDesk = await _memoryCache.GetOrCreateAsync(cachKey, async entry =>
            {
                ShareDesk w = new();
                await LoadShareDesk(w.Items, worker, query);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return w;
            });

            return shareDesk;
        }









        public async Task LoadShareDesk(List<ShareDeskItem> collection, IWorker worker,string query)
        {
           



            _queryService.queryActionSet(query);

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@department_id", worker.DepartmentId));                

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            ShareDeskItem item = new ShareDeskItem()
                            {

                                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                                Title = reader["title"] == DBNull.Value ? string.Empty : reader["title"].ToString(),
                                Name = reader["department"] == DBNull.Value ? string.Empty : reader["department"].ToString(),
                                Description = reader["description"] == DBNull.Value ? string.Empty : reader["description"].ToString(),
                                Share_id = reader["share_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["share_id"]),
                                Asp_controller = reader["asp_controller"] == DBNull.Value ? string.Empty : reader["asp_controller"].ToString(),
                                Asp_action = reader["asp_action"] == DBNull.Value ? string.Empty : reader["asp_action"].ToString(),
                                Department_access = reader["department_access"] == DBNull.Value ? 0 : Convert.ToInt32(reader["department_access"]),
                                Department_id = reader["department_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["department_id"])

                            };
                            collection.Add(item);
                        }

                    }
                }
                await connection.CloseAsync();
            }






        }
    }
}
