using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreSecurity.WebAPI
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
            services.AddCors(options =>
            {
                //options.AddDefaultPolicy(builder =>
                //{
                //    builder.AllowAnyOrigin(); // istek nereden gelirse gelsin izin ver.
                //    builder.AllowAnyHeader(); // headerda ne gelirse gelsin izin ver.
                //    builder.AllowAnyMethod(); // hangi metod (GET,POST,PUT,DELETE vb) gelirse gelsin izin ver.
                //});
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.WithOrigins("https://localhost:44349").AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("AllowSomeMethod", builder =>
                {
                    builder.WithMethods("GET");
                });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreSecurity.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreSecurity.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //CorsMiddleware
            //app.UseCors("AllowAll"); //belli bir policy kullanma.
            app.UseCors();
            //CorsMiddleware

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
