using Darbu_records.Formos;
using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.User;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Security.Claims;
using static DevExpress.Utils.SafeXml;

namespace Darbu_records.DAL
{
    public class WorkerLoader
    {
        IQueryService _queryService;
        IHttpContextAccessor _contextAccessor;
        IMemoryCache _cache;


        public WorkerLoader([FromKeyedServices("SelectWorkerQuery")] IQueryService queryService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _queryService = queryService;
            _contextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<IWorker> GetAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new Worker(); }
            var userId = Convert.ToInt32(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cacheKey = $"worker{userId}";

            var worker = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);

                var w = new Worker();
                await LoadWorkerAsync(w, userId);
                //await LoadAchievementAsync(w, w.id);
                await LoadAchievementAsync(w);
                return w;
            });

            return worker;
        }

        public async Task<IWorker> GetByIdAsync(int id)
        {
            if (_contextAccessor.HttpContext == null) { return new Worker(); }
            var userId = id;
            var cacheKey = $"workerById{userId}";

            var worker = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);

                var w = new Worker();
                await LoadWorkerAsync(w, userId);
                //await LoadAchievementAsync(w, w.id);
                await LoadAchievementAsync(w);
                return w;
            });

            return worker;
        }


        public IWorker Get()
        {
            return GetAsync().GetAwaiter().GetResult();
        }




        public async Task LoadWorkerAsync(IWorker worker, int userId)
        {
            if (_contextAccessor.HttpContext == null) { return; }

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@workerId", userId));
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = 1 });

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
#nullable disable       
                        if (await reader.ReadAsync())
                        {
                            worker.id = Convert.ToInt32(reader["id"]);
                            worker.name = reader["name"].ToString();
                            worker.surname = reader["surname"].ToString();
                            worker.jobTitle = reader["jobTitle"].ToString();
                            worker.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                            worker.DepartmentName = reader["Department"].ToString();                           
                            worker.expierence = Convert.ToInt32(reader["expierence"]);
                            worker.progress = Convert.ToInt32(reader["progress"]);

                            Role role = new() { 
                                Id= Convert.ToInt32(reader["role_id"]),
                                Name= reader["role_name"].ToString(),

							};
                            worker.role = role;


                        }
#nullable enable
                    }
                }
            }
        }

        public async Task LoadAchievementAsync(IWorker worker)
        {
            _queryService.queryActionSet("loadAchievment");

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@workerId", worker.id));

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
#nullable disable
                        while (await reader.ReadAsync())
                        {
                            Achievment achievment = new Achievment()
                            {
                                Id = Convert.ToInt32(reader["AchievmentId"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Image = reader["Image"].ToString()
                            };
                            worker.achievments.Add(achievment);
                        }
#nullable enable
                    }
                }
            }
        }
    }
}