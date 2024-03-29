using Delights.Modules.Hello;
using Delights.Modules.Hello.Server;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Test.Modulights.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModules(builder =>
            {
                builder.UseRazorComponentClientModules().AddServerSideBlazorUI<TestBlazorUIProvider>().AddModule<Wasm.TestModule>()
                    .AddHelloModule((o, _) => o.GraphQLEndpoint = "https://localhost:44378/graphql");
                builder.UseGraphQLServerModules()
                    .AddHelloServerModule();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAspNetServerModuleMiddlewares();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAspNetServerModuleEndpoints();
                endpoints.MapGraphQLServerModuleEndpoints();
            });
        }
    }
}
