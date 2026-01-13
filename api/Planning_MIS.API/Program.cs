using Domain.Common;
using Domain.Interfaces;
using Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Planning_MIS.API.Authorization;
using Planning_MIS.API.DocumentAPI;
using Planning_MIS.API.Services;
using System.Reflection;
using System.Text;




var builder = WebApplication.CreateBuilder(args);



// -----------------------------------------------------------
// 🔹 Core services
// -----------------------------------------------------------

builder.Services.AddControllers(); //AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// ⭐ Add CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // nextjs
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// -----------------------------------------------------------
// 🔹 Dependency Injection 
// -----------------------------------------------------------


//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<DocumentAPIHelper>();

builder.Services.AddScoped<AppConfig>();
//builder.Services.AddScoped<SessionData>();

builder.Services.AddDependencies();
//DI.Initialize(services);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();


// Authorization policies  [ePermission]
builder.Services.AddAuthorization(options =>
{
    foreach (var value in Enum.GetValues(typeof(ePermission)))
    {
        int id = (int)value;
        string name = Enum.GetName(typeof(ePermission), value)!;
        options.AddPolicy(name, policy =>
            policy.Requirements.Add(new PermissionRequirement(id)));
    }
});







// -----------------------------------------------------------
// 🔹 Authentication ( JWT / IdentityServer)  &  Swagger 
// -----------------------------------------------------------

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwt();

//Authorization
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationHandler>();


// -----------------------------------------------------------
// 🔹 Build and configure middleware pipeline
// -----------------------------------------------------------

var app = builder.Build();

app.UseStaticFiles();
app.UseDefaultFiles();


//swaggerUI
app.UseSwaggerWithUI(app.Environment);

app.UseHttpsRedirection();
app.UseCors("AllowNextJS");

app.UseRouting();

app.UseSession();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapGet("/", () => "Planning_MIS.API is running!");


app.Run();
