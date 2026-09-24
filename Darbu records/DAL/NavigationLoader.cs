using Darbu_records.Formos;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.Models;
using Darbu_records.Singleton;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Darbu_records.DAL
{
    public class NavigationLoader
    {
        IQueryService _queryService;
        IHttpContextAccessor _contextAccessor;
        IMemoryCache _cache;
        WorkerLoader _workerLoaderCache;

        public NavigationLoader([FromKeyedServices("CollectGroupQuery")] IQueryService queryService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, WorkerLoader workerLoaderCache)
        {
            _queryService = queryService;
            _contextAccessor = httpContextAccessor;
            _cache = cache;
            _workerLoaderCache = workerLoaderCache;
        }

        public async Task<AppInfo> GetAsync()
        {
            if (_contextAccessor.HttpContext == null) { return new AppInfo(); }

            //Gauname darbuotoja
            var worker = await _workerLoaderCache.GetAsync();
            
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cachKey = $"appInfo:{userId}";

            //cia jis patikrina ar cachKey yra jei ne tai sukuria kita
            var appInfo = await _cache.GetOrCreateAsync(cachKey, async entry =>
            {
                AppInfo w = new();
                await LoadGroupsAsync(w.groups, worker.DepartmentId);
                await LoadCategoryAsync(w.categories, worker.DepartmentId);
                await LoadNavigationActionAsync(w.actionNavigationItems, worker.DepartmentId);
                await LoadNavigationAsync(w.navigationItems, worker.DepartmentId);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                return w;
            });

            return appInfo;
        }

        public AppInfo Get()
        {
            return GetAsync().GetAwaiter().GetResult();
        }


         private T? SafeDeserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "{}")
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }



        public async Task LoadGroupsAsync(List<CategoryGroup> categoriesGroup, int departmentId)
        {          
            string query = $@"SELECT 
             gc.id,			 
             gc.group_name,
			 gca.group_id,
             gca.department_access,
					 (
			SELECT 
				IFL.file_name AS fileName,
				IFL.upload_date AS fileDate,
				IFL.directory AS fileDirectory,
				IFL.original_name AS originalName
			FROM GroupFiles AS IFL
			WHERE gc.id = IFL.group_id
			FOR JSON PATH,WITHOUT_ARRAY_WRAPPER
		) AS Files
             
            FROM GroupCategory gc
            LEFT JOIN GroupAccess gca
            ON gc.id = gca.group_id
            LEFT JOIN GroupFiles as gf
            ON gc.id=gf.group_id
            WHERE gca.department_id=@department_id AND gc.status=1 AND gca.department_access=@department_id";
            //string query = $@"SELECT 
            // gc.id,
            // gc.group_name,
            // gca.group_id
            //FROM GroupCategory gc
            //INNER JOIN GroupCategoryAssign gca
            //ON gc.id = gca.group_id
            //WHERE gca.department_id =@departmentId AND gca.department_parameter=@department_parameter AND gc.status=@status";

           // var worker = await _workerLoaderCache.GetAsync();
           // Console.WriteLine($"Is db{worker.DepartmentId}      {departmentId}");
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@department_id", departmentId));
                   // command.Parameters.Add(new SqlParameter("@department_parameter", departmentId));
                    command.Parameters.Add(new SqlParameter("@status", 1));
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            CategoryGroup group = new CategoryGroup
                            {
                                Id = Convert.ToInt32(reader["group_id"]),
                                Name = reader.GetString(reader.GetOrdinal("group_name")),
                                DepartmentAccess = Convert.ToInt32(reader["department_access"]),

                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                            {
                                var jsonPath = reader.GetString(reader.GetOrdinal("Files"));
                               
                                group.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);
                            }
                            categoriesGroup.Add(group);
                        }

                    }
                }
              
            }
        }


        //Paima pilnai su failais 
        public async Task LoadCategoriesAsync(List<Category> categories, int departmentId)
        {


            string query = $@"
                SELECT 
                  c.id,
                  c.category_name,                
                  c.group_id,
                  ca.category_id,                        
                  ca.department_access,

                  (
                    SELECT 
                        IFL.file_name AS fileName,
                        IFL.upload_date AS fileDate,
                        IFL.directory AS fileDirectory,
                        IFL.original_name AS originalName
                    FROM CategoryFiles AS IFL
                    WHERE ca.category_id = IFL.category_id
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
                  ) AS Files

                FROM Category c
                INNER JOIN CategoryAccess ca
                  ON c.id = ca.category_id

                WHERE ca.department_id = @department_id
                  ";

            string query2 = @"
                           SELECT 
                            c.id,
                            c.category_name,
                            c.group_id,
                            ca.department_access,

                            (
                                SELECT 
                                    IFL.file_name AS fileName,
                                    IFL.upload_date AS fileDate,
                                    IFL.directory AS fileDirectory,
                                    IFL.original_name AS originalName
                                FROM CategoryFiles AS IFL
                                WHERE IFL.category_id = c.id
                                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                            ) AS Files

                        FROM Category c

                        -- CategoryAccess prisijungiame tik jei egzistuoja prieiga
                        INNER JOIN CategoryAccess ca
                            ON c.id = ca.category_id
                            AND ca.department_id = @department_id

                        -- GroupAccess prisijungiame tik jei kategorija turi group_id
                        INNER JOIN GroupAccess ga
                            ON ga.group_id = c.group_id
                            AND ga.department_id = @department_id

                        WHERE 
                            -- arba prieiga per CategoryAccess
                            ca.category_id IS NOT NULL
                            -- arba prieiga per GroupAccess (tik jei kategorija turi group_id)
                            OR (c.group_id IS NOT NULL AND ga.group_id IS NOT NULL)
                            ";
            //string query = $@"SELECT 
            // gc.id,
            // gc.group_name,
            // gca.group_id
            //FROM GroupCategory gc
            //INNER JOIN GroupCategoryAssign gca
            //ON gc.id = gca.group_id
            //WHERE gca.department_id =@departmentId AND gca.department_parameter=@department_parameter AND gc.status=@status";

            // var worker = await _workerLoaderCache.GetAsync();
            // Console.WriteLine($"Is db{worker.DepartmentId}      {departmentId}");
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@department_id", departmentId));
                    // command.Parameters.Add(new SqlParameter("@department_parameter", departmentId));
                  //  command.Parameters.Add(new SqlParameter("@status", 1));
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                GroupId = reader["group_id"] != DBNull.Value
                                        ? Convert.ToInt32(reader["group_id"])
                                        : (int?)null,
                            Name = reader.GetString(reader.GetOrdinal("category_name")),
                                DepartmentAccess = Convert.ToInt32(reader["department_access"]),

                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                            {
                              
                                var jsonPath = reader.GetString(reader.GetOrdinal("Files"));                             
                                category.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);

                               
                            }
                           // Console.WriteLine(category.recordFile.originalName);
                            categories.Add(category);
                        }

                    }
                }

            }
        }



        public async Task LoadCategoriesAsyncPerziurai(List<Category> categories, int departmentId)
        {


            string query2 = $@"
                SELECT 
                  c.id,
                  c.category_name,                
                  c.group_id,
                  ca.category_id,                        
                  ca.department_access,

                  (
                    SELECT 
                        IFL.file_name AS fileName,
                        IFL.upload_date AS fileDate,
                        IFL.directory AS fileDirectory,
                        IFL.original_name AS originalName
                    FROM CategoryFiles AS IFL
                    WHERE ca.category_id = IFL.category_id
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
                  ) AS Files

                FROM Category c
                INNER JOIN CategoryAccess ca
                  ON c.id = ca.category_id

                WHERE ca.department_id = @department_id
                  ";

            string query = @"
                          SELECT 
    c.id,
    c.category_name,
    c.group_id,
    COALESCE(ca.department_access, ga.department_access, 0) AS department_access,

    (
        SELECT 
            IFL.file_name AS fileName,
            IFL.upload_date AS fileDate,
            IFL.directory AS fileDirectory,
            IFL.original_name AS originalName
        FROM CategoryFiles AS IFL
        WHERE IFL.category_id = c.id
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS Files

FROM Category c

LEFT JOIN CategoryAccess ca
    ON c.id = ca.category_id
    AND ca.department_id = @department_id

LEFT JOIN GroupAccess ga
    ON ga.group_id = c.group_id
    AND ga.department_id = @department_id

WHERE 
    c.group_id IS NOT NULL
    AND (ca.category_id IS NOT NULL OR ga.group_id IS NOT NULL)
                            ";
            //string query = $@"SELECT 
            // gc.id,
            // gc.group_name,
            // gca.group_id
            //FROM GroupCategory gc
            //INNER JOIN GroupCategoryAssign gca
            //ON gc.id = gca.group_id
            //WHERE gca.department_id =@departmentId AND gca.department_parameter=@department_parameter AND gc.status=@status";

            // var worker = await _workerLoaderCache.GetAsync();
            // Console.WriteLine($"Is db{worker.DepartmentId}      {departmentId}");
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add(new SqlParameter("@department_id", departmentId));
                    // command.Parameters.Add(new SqlParameter("@department_parameter", departmentId));
                    //  command.Parameters.Add(new SqlParameter("@status", 1));
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                GroupId = reader["group_id"] != DBNull.Value
                                        ? Convert.ToInt32(reader["group_id"])
                                        : (int?)null,
                                Name = reader.GetString(reader.GetOrdinal("category_name")),
                                DepartmentAccess = Convert.ToInt32(reader["department_access"]),

                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Files")))
                            {

                                var jsonPath = reader.GetString(reader.GetOrdinal("Files"));
                                category.recordFile = JsonSerializer.Deserialize<RecordFile>(jsonPath);


                            }
                            // Console.WriteLine(category.recordFile.originalName);
                            categories.Add(category);
                        }

                    }
                }

            }
        }






        public async Task LoadCategoryAsync(List<Category> categories, int departmentId)
        {

          //  Console.WriteLine($"Dsdsdsdsdsd{departmentId}");
            string query = $@"SELECT 
             c.id,
             c.category_name,
             c.image,
             c.group_id,
             ca.category_id,                        
             ca.department_access
            FROM Category c
            INNER JOIN CategoryAccess ca
            ON c.id = ca.category_id
            WHERE ca.department_id =@departmentId AND ca.department_access=@departmentId";

            var worker = await _workerLoaderCache.GetAsync();
            //    _queryService.queryActionSet("SELECT id,category_name,group_id,image FROM Category WHERE DepartmentId=@departmentId");

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@departmentId",departmentId));
                   

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                GroupId = reader["group_id"] != DBNull.Value
                                            ? Convert.ToInt32(reader["group_id"])
                                            : (int?)null,
                                Name = reader.GetString(reader.GetOrdinal("category_name")),
                                Image = reader.IsDBNull(reader.GetOrdinal("image"))                           
                                ? null
                                : reader.GetString(reader.GetOrdinal("image")),
                                DepartmentAccess = Convert.ToInt32(reader["department_access"])
                            };
                            categories.Add(category);
                        }

                    }
                }
                await connection.CloseAsync();
            }
        }



        public async Task<Category?> GetCategoryAsync(int categoryId)
        {
            _queryService.queryActionSet("SELECT id,category_name,group_id,image FROM Category WHERE id=@categoryId");

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@categoryId", categoryId));

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                           return new Category 
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                GroupId = Convert.ToInt32(reader["group_id"]),
                                Name = reader.GetString(reader.GetOrdinal("category_name")),
                                Image = reader.IsDBNull(reader.GetOrdinal("image"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("image"))
                            };
                          
                        }

                    }
                }
                
            }
            return new Category() {
            Name = "default"
            
            };
        }

        public async Task<List<Category>> LoadCategoriesAsync(int id)
        {
            List<Category> categories = new List<Category>();


            //  Console.WriteLine($"Dsdsdsdsdsd{departmentId}");
            //string query = $@"SELECT  
            //            c.id,
            //        c.category_name,
            //        c.image,
            //        ca.group_id,
            //        ca.category_id,                        
            //        ca.department_access
            //        FROM Category c
            //        LEFT JOIN CategoryAccess ca
            //            ON c.id = ca.category_id
            //        WHERE c.DepartmentId = @departmentId
            //        AND (
            //            ca.group_id = @group_id 
            //            OR ca.category_id IS NULL  
            //         )" ;
            string query = $@"SELECT  
                            c.id,
                            c.category_name,
                            c.image,
                            c.group_id,
                            ca.category_id,                        
                            ca.department_access
                        FROM Category c
                        OUTER APPLY (
                            SELECT TOP 1 *
                            FROM CategoryAccess ca
                            WHERE ca.category_id = c.id
                        ) ca
                        WHERE c.DepartmentId = @departmentId
                          AND (
                                c.group_id = @group_id
                             OR c.group_id IS NULL
                          );";

            var worker = await _workerLoaderCache.GetAsync();
            //    _queryService.queryActionSet("SELECT id,category_name,group_id,image FROM Category WHERE DepartmentId=@departmentId");

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@departmentId", worker.DepartmentId));
                    command.Parameters.Add(new SqlParameter("@group_id", id));

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["id"]),

                                GroupId = reader["group_id"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["group_id"]),

                                Name = reader.GetString(reader.GetOrdinal("category_name")),

                                Image = reader.IsDBNull(reader.GetOrdinal("image"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("image")),

                                DepartmentAccess = reader["department_access"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["department_access"]),

                                isSelected = reader["group_id"] != DBNull.Value
                            };

                            categories.Add(category);
                        }

                    }
                }

              //  await connection.CloseAsync();
              return categories;
            }
        }


        public async Task<List<CategoryGroup>> LoadGroupsAsync(int id)
        {
            List<CategoryGroup> groups = new List<CategoryGroup>();
           

            //  Console.WriteLine($"Dsdsdsdsdsd{departmentId}");
            //string query = $@"SELECT  
            //            c.id,
            //        c.category_name,
            //        c.image,
            //        ca.group_id,
            //        ca.category_id,                        
            //        ca.department_access
            //        FROM Category c
            //        LEFT JOIN CategoryAccess ca
            //            ON c.id = ca.category_id
            //        WHERE c.DepartmentId = @departmentId
            //        AND (
            //            ca.group_id = @group_id 
            //            OR ca.category_id IS NULL  
            //         )" ;
            string query = $@"SELECT 
                            g.id,
                            g.group_name,
                            ca.id as category_id,
                            cac.department_access
                        FROM GroupCategory g
                        LEFT JOIN Category ca
                            ON ca.group_id = g.id
                            AND ca.id = @category_id
                        LEFT JOIN CategoryAccess cac
                        ON ca.id=cac.category_id
                        INNER JOIN GroupAccess ga
                        ON g.id=ga.group_id
                        WHERE ga.department_id = @department_id";

            var worker = await _workerLoaderCache.GetAsync();
            //    _queryService.queryActionSet("SELECT id,category_name,group_id,image FROM Category WHERE DepartmentId=@departmentId");

            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@department_id", worker.DepartmentId));
                    command.Parameters.Add(new SqlParameter("@category_id", id));

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            CategoryGroup group = new CategoryGroup
                            {
                                Id = Convert.ToInt32(reader["id"]),

                                CategoryId = reader["category_id"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["category_id"]),

                                Name = reader.GetString(reader.GetOrdinal("group_name")),                              

                                DepartmentAccess = reader["department_access"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["department_access"]),

                                isSelected = reader["category_id"] != DBNull.Value
                            };

                            groups.Add(group);
                        }

                    }
                }

                //  await connection.CloseAsync();
                return groups;
            }
        }

        public async Task<CategoryGroup?> GetGroupAsync(int groupId)
        {
            string queryAction = "SELECT id, group_name FROM GroupCategory WHERE id=@groupId";
            _queryService.queryActionSet(queryAction);
            //  Console.WriteLine($"Is db");
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(_queryService.queryActionGet(), connection))
                {
                    command.Parameters.Add(new SqlParameter("@groupId", groupId));
                   
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                          return new CategoryGroup 
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader.GetString(reader.GetOrdinal("group_name"))
                            };
                          
                        }

                    }
                }
                
            }
            return new CategoryGroup() { Name = "default" };
        }


        public async Task LoadNavigationAsync(List<NavigationItem> items,int departmentId)
        {

            //string query = $@"SELECT 
            // nc.id,
            // nc.name,
            // nc.asp_action,
            // nc.asp_controller,           
            // nca.action_id,
            // nca.department_parameter
            //FROM NavigationsConfig nc
            //INNER JOIN NavigationsConfigAssign nca
            // ON nc.id = nca.config_id
            //WHERE nca.department_id = @department_id
            //ORDER BY name ASC";
            string query = $@"SELECT 
                            nc.id,
                            nc.navigation_id,
                            nc.name,
                             nc.asp_action,
                             nc.asp_controller,  
                            nca.department_id as department_access
                             FROM NavigationConfig nc
                             INNER JOIN NavigationActionAccess nca 
                            ON nc.navigation_id = nca.action_id                          
                             WHERE nca.department_id = @department_id ORDER BY name ASC";



        


           // _queryService.queryActionSet("SELECT navigation_id,assign_id,Parameter,name,asp_action,asp_controller from NavigationConfig where department_id=@assignId ORDER BY name ASC");
            await using(SqlConnection connection = _queryService.sqlConnGet())
            {
              await connection.OpenAsync();
                await using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@department_id",departmentId));

                    await using(SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                       while(await reader.ReadAsync())
                        {
                            NavigationItem item = new NavigationItem()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                AssignId = reader.GetInt32(reader.GetOrdinal("navigation_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                AspAction = reader.GetString(reader.GetOrdinal("asp_action")),
                                AspController = reader.GetString(reader.GetOrdinal("asp_controller")),
                                DepartmentAccess = reader.GetInt32(reader.GetOrdinal("department_access"))

                            };

                            items.Add(item);


                        }




                    }


                }


                await connection.CloseAsync() ;
            }



        }
        public async Task LoadNavigationActionAsync(List<ActionNavigationItem> items, int departmentId)
        {
            string query = $@"SELECT 
                            na.name,
                            naa.action_id,
                            naa.department_id
                        FROM NavigationAction na
                        INNER JOIN NavigationActionAccess naa 
                            ON na.id = naa.action_id
                        WHERE na.status = @status
                          AND naa.department_id = @assignId";
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@assignId", departmentId));
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 50) { Value="active"});

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            
                            ActionNavigationItem item = new ActionNavigationItem()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("action_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")) ,
                                DepartmentId = Convert.ToInt32(reader["department_id"])
                            };

                            items.Add(item);


                        }




                    }


                }


                await connection.CloseAsync();
            }












        }


        public async Task<List<ActionNavigationItem>> GetNavigationActionAsync()
        {
            string query = $@"SELECT 
                            na.id,
                            na.name                          
                        FROM NavigationAction na                      
                        WHERE na.status = @status
                         ";
            List<ActionNavigationItem> items = new();
            await using (SqlConnection connection = _queryService.sqlConnGet())
            {
                await connection.OpenAsync();
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                   
                    command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 50) { Value = "active" });

                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            ActionNavigationItem item = new ActionNavigationItem()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name"))                            
                            };

                            items.Add(item);


                        }




                    }


                }


                return items;
            }












        }
    }
}