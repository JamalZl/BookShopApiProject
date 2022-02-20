using BookShopApi.Apps.AdminApi.DTOs;
using BookShopApi.Apps.AdminApi.Profiles;
using BookShopApi.Data.DAL;
using BookShopApi.Data.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AuthorPostDto>());
            services.AddDbContext<BookShopDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddAutoMapper(option =>
            {
                option.AddProfile(new MapProfile());

            });
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<BookShopDbContext>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = Configuration.GetSection("JWT:issuer").Value,
                    ValidAudience = Configuration.GetSection("JWT:audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:secret").Value))
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("admin_v1", new OpenApiInfo
                {
                    Title = "Book Shop API",
                    Version = "admin_v1",
                    Description = "An API to perform Employee operations",
                    Contact = new OpenApiContact
                    {
                        Name = "Camal Zeynalli  ",
                        Email = "zeynalli5503.7@gmail.com"
                    },
                });
                c.SwaggerDoc("user_v1", new OpenApiInfo
                {
                    Title = "Book Shop API",
                    Version = "user_v1",
                    Description = "An API to perform Employee operations",
                    Contact = new OpenApiContact
                    {
                        Name = "Camal Zeynalli  ",
                        Email = "zeynalli5503.7@gmail.com"
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
                });
            });
           
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/api/swagger/user_v1/swagger.json", "User API  V1");
                x.SwaggerEndpoint("/api/swagger/admin_v1/swagger.json", "Admin API V1");
                x.RoutePrefix = "api/swagger";
            });
        }
    }
}
