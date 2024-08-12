using Microsoft.EntityFrameworkCore;
using Company_X.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;

namespace Company_X
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
            });

            //authorize
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminOnly", policy =>
                    policy.Requirements.Add(new SuperAdminRequirement()));
            });

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        In = ParameterLocation.Header,
            //        Description = "Enter 'Bearer' followed by a space and the JWT token."
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] { }
            //        }
            //    });
            //});

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            //configuration database wiht dbcontext
            builder.Services.AddDbContext<APIContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("dbConn")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
