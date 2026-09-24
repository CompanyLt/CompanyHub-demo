using Darbu_records.Singleton;
using static DevExpress.Utils.SafeXml;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Darbu_records.Models;
using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Microsoft.Data.SqlClient;
using Darbu_records.Interfaces;

namespace Darbu_records.DAL
{
    public class DepartmentBoardLoader
    {

       private readonly IHttpContextAccessor _contextAccessor;
       private readonly IMemoryCache _memoryCache;
       private readonly IQueryService _queryService;
        WorkerLoader _workerLoaderCache;
        


        public DepartmentBoardLoader(IHttpContextAccessor contextAccessor,IMemoryCache memoryCache, [FromKeyedServices("CollectGroupQuery")] IQueryService queryService,WorkerLoader workerLoaderCache)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _queryService = queryService;
            _workerLoaderCache = workerLoaderCache;
        }



        public async Task<DepartmentBoard> GetAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new DepartmentBoard(); }


           
            var worker = await _workerLoaderCache.GetAsync();
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cachKey = $"departmentTopics:{userId}";
           
            //cia jis patikrina ar cachKey yra jei ne tai sukuria kita
            var departmentTopics = await _memoryCache.GetOrCreateAsync(cachKey, async entry =>
            {
                DepartmentBoard w = new();
                await LoadTopics(w._topics,worker);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return w;
            });

            return departmentTopics;
        }


        




        public async Task LoadTopics(List<DepartmentTopic> departmentTopics,IWorker worker)
        {
                   string query = @"SELECT 
            db.id,
            db.title,
            db.description,
            db.created_at,
            db.comment_count,
            db.status,
            db.avatar,
            rc.read_count
        FROM DepartmentBoard db
        LEFT JOIN DBoard_read_comments rc
            ON rc.topic_id = db.id
            AND rc.worker_id = @WorkerId 
        INNER JOIN DepartmentBoardAccess dba
            ON db.id=dba.topic_id
        WHERE dba.department_id = @DepartmentId
        ORDER BY db.created_at DESC;";



            _queryService.queryActionSet(query);
 
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@DepartmentId", worker.DepartmentId));
                    command.Parameters.Add(new SqlParameter("@WorkerId", worker.id));

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            DepartmentTopic departmentTopic = new DepartmentTopic()
                            {

                                Id = reader["id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["id"]),
                                Title = reader["title"] == DBNull.Value ? string.Empty : reader["title"].ToString(),
                                Description = reader["description"] == DBNull.Value ? string.Empty : reader["description"].ToString(),
                                Avatar = reader["avatar"] == DBNull.Value ? string.Empty : reader["avatar"].ToString(),
                                CreatedAt = reader["created_at"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["created_at"]),
                                CommentCount = reader["comment_count"] == DBNull.Value ? 0 : Convert.ToInt32(reader["comment_count"]),
                                Status = reader["status"] == DBNull.Value ? string.Empty : reader["status"].ToString(),
                                CommentRead = reader["read_count"] == DBNull.Value ? 0 : Convert.ToInt32(reader["read_count"])

                            };
                            departmentTopics.Add(departmentTopic);
                        }

                    }
                }
                await connection.CloseAsync();
            }






        }
    }
}
