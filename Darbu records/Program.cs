using Darbu_records.Formos;
using DevExpress.Data.Browsing.Design;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Darbu_records.Models;
using System.Security.Claims;
using Darbu_records.SearchManagement;
using Darbu_records.Singleton;
using Microsoft.AspNetCore.Http.Features;
using Darbu_records.Formos;
using Darbu_records.DI;
using Darbu_records.Interfaces.NoteService;
using Darbu_records.IncidentManagement;
using Darbu_records.DAL;
using Microsoft.Extensions.Options;
using Darbu_records.Interfaces;
using Darbu_records.UserManagement;
using Darbu_records.AuthenticationManagement;
using Darbu_records.Query.User;
using Darbu_records.CommentManagement;
using Darbu_records.SharedeskManagement;
using Darbu_records.InstructionManagement;

var builder = WebApplication.CreateBuilder(args);






//sesijos sukurimas
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);// laikas kiek galioja slapukas
    options.Cookie.Name = "Note_program";
    options.Cookie.HttpOnly = true; //pasiekiamas tik per http
    options.Cookie.IsEssential = true; //cia nurodom kad sitas slapukas butinas
    

});

//cia konfiginasi formos ikelimo mb dydis
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500000000;



});


//paemmam appsetings duomenis
//var configFile = builder.Configuration;


//var navBarConfig = configFile.GetSection(key: "Navbar").Get<AppInfo>();
//if(navBarConfig != null)
//{
//builder.Services.AddSingleton(navBarConfig);
//}
//else
//{
//    builder.Services.AddSingleton<AppInfo>();
//}



//builder.Services.AddIncidentServices();


//cia konfiguruojasi max failo dydis serveryje ir siaip kestrel serveris
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 500000000; // 500 MB

    serverOptions.Listen(System.Net.IPAddress.Parse("0.0.0.0"), 443, listenOption =>
    {
        listenOption.UseHttps("C:\\win-acme\\certs\\companyhub.lt.pfx", "Company1986");
    });

});





builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "Pagrindinis";
        //tai reiskia kad slapukai bus siunciami tik per https
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.IsEssential = true;
        options.SlidingExpiration = true;
    });



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
//sukuriamas singletonas
//builder.Services.AddSingleton<IrasuKiekis>();

builder.Services.AddScoped<WorkerLoader>();
builder.Services.AddScoped<IFilesPath,FilesPath>();
builder.Services.AddScoped<NavigationLoader>();
builder.Services.AddTransient<DepartmentBoardLoader>();
builder.Services.AddTransient<ShareDeskLoader>();
builder.Services.AddScoped<DepartmentLoader>();
builder.Services.AddTransient<ShareDeskRepository>();
builder.Services.AddScoped<WorkerLoader>();
builder.Services.AddScoped<CommentLoadService>();
builder.Services.AddSingleton<IConnectionRepository,DbSettings>();
builder.Services.AddTransient<AppInfo>();
builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddKeyedScoped<IQueryService, UserRegistrationQueryService>("UserRegistrationQueryService");
builder.Services.AddScoped<WorkerSessionService>();
builder.Services.AddScoped<IWorker, Worker>();
//ServiceRegistration.IncidentServices(builder);
//ServiceRegistration.IncidentQueryServices(builder);
ServiceRegistration.NoteServices(builder);
ServiceRegistration.NoteQueryServices(builder);
ServiceRegistration.ConfigurationQueryServices(builder);
ServiceRegistration.ConfigurationServices(builder);
ServiceRegistration.WorkerQueryServices(builder);
ServiceRegistration.InstructionQueryServices(builder);
ServiceRegistration.InstructionServices(builder);
ServiceRegistration.CommentServices(builder);
ServiceRegistration.TopicService(builder);
ServiceRegistration.FilePathServices(builder);
ServiceRegistration.ShareDesk(builder);
ServiceRegistration.FileService(builder);
ServiceRegistration.DepartmentServices(builder);
ServiceRegistration.GroupServices(builder);
ServiceRegistration.CategoryServices(builder);
builder.Services.AddTransient<FileCommunicationService>();
builder.Services.AddTransient<FilePathService>();



//builder.Services.AddSingleton(appSetting);

var app = builder.Build();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

 var ip = context.Connection.RemoteIpAddress?.ToString();
    var allowedIps = new HashSet<string>
    {
        "127.0.0.1",      // localhost
        "::1",            // IPv6 localhost
        "188.69.7.146",    // pvz tavo IP (pasikeisk į savo)
        "5.20.100.133"
    };

    if (!allowedIps.Contains(ip))
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Access denied");
        return;
    }

    await next();

});

//LOADERIS
//using(var scope = app.Services.CreateScope())
//{
//   var loader =  scope.ServiceProvider.GetRequiredService<NavigationLoader>();
//   var appInfo =  scope.ServiceProvider.GetRequiredService<AppInfo>();
//    await loader.LoadGroups(appInfo.groups);
//   await loader.LoadCategory(appInfo.categories);




//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();//autentikacija turi buti auksciau autorizacijos
app.UseAuthorization();




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
