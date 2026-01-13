using DataAccess;
using DataAccess.Repo;
using DataAccess.UnitOfWork;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using System.Text;

namespace Planning_MIS.API.Services
{
    public static class ServiceExtensions
    {

        // -----------------------------------------------------------
        // 🔹 Dependencies
        // -----------------------------------------------------------
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<DatabaseContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISetupRepository, SetupRepository>();

            //services.AddScoped<ISessionRepository, SessionRepository>();
            //services.AddScoped<IDashboardRepository, DashboardRepository>();
            //services.AddScoped<ICommonRepository, CommonRepository>();
            //services.AddScoped<ICommitteeRepository, CommitteeRepository>();
            //services.AddScoped<IMasterRepository, MasterRepository>();
            //services.AddScoped<IProjectRepository, ProjectRepository>();

            return services;


        }





        // -----------------------------------------------------------
        // 🔹 Authentication (optional: cookie + JWT / IdentityServer)
        // -----------------------------------------------------------

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthenticated User" });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            return services;
        }




        // -----------------------------------------------------------
        // 🔹 Swagger Setup
        // -----------------------------------------------------------

        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MIS Planning API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }


        // -----------------------------------------------------------
        // 🔹 Swagger Middleware
        // -----------------------------------------------------------

        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MIS Planning API v1");
                    c.RoutePrefix = string.Empty;
                    c.InjectStylesheet("/swagger/custom.css");

                });


            return app;
        }
    }
}
