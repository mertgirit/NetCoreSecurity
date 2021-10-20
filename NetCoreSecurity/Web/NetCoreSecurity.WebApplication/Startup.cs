using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreSecurity.WebApplication.Models;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSecurity.WebApplication.Middleware;

namespace NetCoreSecurity.WebApplication
{
    using AppSettings;
    using NetCoreSecurity.WebApplication.Filters;

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
            services.Configure<IPList>(Configuration.GetSection("IPList"));
            services.AddControllersWithViews();
            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //DataProtection servislerinin containera eklenmesini sa�lar.
            services.AddDataProtection();

            //Controller/Action Bazl� IP Kontrolu
            services.AddScoped<CheckWhiteListAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Uygulama Bazl� IP Kontrolu
            //app.UseMiddleware<IPSecurityMiddleware>();
            //Uygulama Bazl� IP Kontrolu

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}