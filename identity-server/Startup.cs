using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Identity_server.Data;
using Identity_server.Data.DomainModel;
using Identity_server.Data.SettingIdentity;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using FluentValidation.AspNetCore;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Identity_server
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()                
                .AddFluentValidation(fv => 
                    { fv.RegisterValidatorsFromAssemblyContaining<Startup>();});


            var connectionString = Configuration.GetConnectionString("IdentityServerDatabase");
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            


            var dataAssemblyName = typeof(AppDbContext).Assembly.GetName().Name;
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString,
                x => x.MigrationsAssembly(dataAssemblyName)));

            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1d);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //certificate cert_rsa512
           var rsaCertificate = new X509Certificate2(
                Path.Combine(_environment.ContentRootPath, "cert_rsa512.pfx"), "1234");
            
            var builder = services.AddIdentityServer()

                .AddAspNetIdentity<User>()
                .AddConfigurationStore(op =>
                {
                    op.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));
                })
                .AddOperationalStore(op =>
                    op.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql =>
                        sql.MigrationsAssembly(migrationAssembly))
                ).AddSigningCredential(rsaCertificate);

            builder.Services.AddTransient<IProfileService, ProfileService>();
            
            services.AddLocalApiAuthentication();

            services.AddOpenApiDocument(document =>
            {
                document.Title = "IdentityServer";
                document.Description = "API identityserver";
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
 
                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
            
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DatabaseInitializer.PopulateIdentityServer(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}