using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBLOG.CORE.Service;
using CoreBLOG.MODEL.Context;
using CoreBLOG.SERVICE.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBLOG.UI
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
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddControllersWithViews();
            services.AddMvc(x => x.EnableEndpointRouting = false); //Mvc route kullanabilmek için yapýldý
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                option =>
                {
                    option.LoginPath = "/Account/Login";
                });
            services.AddScoped(typeof(ICoreService<>), typeof(BaseService<>));
            //typeof kullanmamýzýn sebebi; bu tip ne için kullanýlýyorsa BaseService'te de o tip için kullanýlsýn.
            services.AddDbContext<BlogContext>(option =>
            {
                option.UseSqlServer("server=.; database=BlogProjeDB; uid=sa; pwd=123");
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                //linq sorgularý ile gönderilen sorgularý sadece okuyup iþlemini bitiriyor, iþlemi bitince bu sorguyu elinde tutmuyor.
            });
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


            });

        }
    }
}
