using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.ServiceProvider;

namespace Web
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
            // Setup config
            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");
            var rebusConfig = new RebusConfiguration(
                Configuration["Rebus:Transport"],
                Configuration["Rebus:Subscription"],
                Configuration["Rebus:DefaultQueue"]);
            
            // Configure and register Rebus
            services.AddRebus(configure => configure
                .Logging(l => l.Console())
                .Transport(t => t.UseSqlServer(defaultConnection, rebusConfig.Transport, rebusConfig.Transport))
                .Subscriptions(s => s.StoreInSqlServer(defaultConnection, rebusConfig.Subscription)));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRebus();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
