using FileExplorer.Hubs;
using FileExplorer.Models;
using FileExplorer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading;

namespace FileExplorer
{
    public class Startup
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<AppSettings>>().Value);

            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<IBackgroundWorker, BackgroundWorker>();

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("http://localhost:4445")
                       .AllowCredentials();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<FileSystemHub>("/file-system");
            });

            app.UseMvc();
            var worker = app.ApplicationServices.GetService<IBackgroundWorker>();
            var service = app.ApplicationServices.GetService<IFileSystemService>();
            worker.Add("read", async () => await service.ReadFileSystem());
        }

        private void OnShutdown()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
