using Darbu_records.Interfaces;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Models.Department;
using Darbu_records.Models.ShareDesk;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;

namespace Darbu_records.DAL
{
    public class DepartmentLoader
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IQueryService _queryService;
       

        public DepartmentLoader(IHttpContextAccessor contextAccessor, IMemoryCache memoryCache, [FromKeyedServices("CollectGroupQuery")] IQueryService queryService)
        {
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
            _queryService = queryService;
          
        }



        public async Task<Department> GetAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new Department(); }
          
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cachKey = $"department:{userId}";

            //cia jis patikrina ar cachKey yra jei ne tai sukuria kita
            var departmentCollection = await _memoryCache.GetOrCreateAsync(cachKey, async entry =>
            {
                Department w = new();
                await LoadShareDesk(w.departmentCollection);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return w;
            });

            return departmentCollection;
        }







        public async Task LoadShareDesk(List<DepartmentItem> collection)
        {
            string query = @"SELECT 
                            D.Id,
                            D.Name,
                            D.created_by,
                            (
                                SELECT 
                                    DF.file_name AS fileName,
                                    DF.upload_date AS fileDate,
                                    DF.directory AS fileDirectory,
                                    DF.original_name AS originalName
                                FROM DepartmentsFiles AS DF
                                WHERE DF.department_id = D.Id
                                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                            ) AS Files
                        FROM Departments AS D
                        ;";



            _queryService.queryActionSet(query);

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {                
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            DepartmentItem item = new DepartmentItem()
                            {

                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString(),
                                created_by = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : (int?)null

                        };
                            if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                            {

                                var jsonPath = reader.GetString(reader.GetOrdinal("Files"));
                                item.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);


                            }


                            collection.Add(item);
                        }

                    }
                }
                await connection.CloseAsync();
            }






        }

        public async Task<DepartmentItem> GetDepartmentByIdAsync(int id)
        {
            string query = @"SELECT 
                            D.Id,
                            D.Name,
                            D.created_by,
                            (
                                SELECT 
                                    DF.file_name AS fileName,
                                    DF.upload_date AS fileDate,
                                    DF.directory AS fileDirectory,
                                    DF.original_name AS originalName
                                FROM DepartmentsFiles AS DF
                                WHERE DF.department_id = D.Id
                                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                            ) AS Files
                        FROM Departments AS D
                        WHERE D.Id = @id;";



            _queryService.queryActionSet(query);

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            DepartmentItem item = new DepartmentItem()
                            {

                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString(),
                                created_by = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : (int?)null

                            };


                            if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                            {

                                var jsonPath = reader.GetString(reader.GetOrdinal("Files"));
                                item.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);


                            }
                            return item;
                        }

                    }
                }
                await connection.CloseAsync();
            }




            return new DepartmentItem() { Name = "Empty" };  
           
        }




    }
}
