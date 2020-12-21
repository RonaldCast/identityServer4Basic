using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer4.AccessTokenValidation;

namespace ProofAPI
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
            services.AddMvcCore()
                .AddAuthorization();
            
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(op =>
                {
                    op.Authority = "http://localhost:5000";
                    op.RequireHttpsMetadata = false;
                    op.ApiName = "my-api";
                });

            //para que no acepte todos los tokens
            services.AddAuthorization(op =>
            {
                op.AddPolicy("ApiScope", policy =>
                {
                    
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "read");
                    policy.RequireClaim("scope", "write");
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
         
            
            app.UseHttpsRedirection();

            app.UseRouting();

          
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });
        }
    }
}