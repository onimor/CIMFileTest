using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace CIM.Backend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // if don't set default value is: 30 MB
            });
            //services.AddGrpc(options =>
            //{
            //    options.MaxReceiveMessageSize = 150 * 1024 * 1024; // 150 MB
            //    options.MaxSendMessageSize = 155 * 1024 * 1024; // 155 MB
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBlazorFrameworkFiles();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb();

                endpoints.MapPost("/Filesave", async context =>
                {
                    var read = await context.Request.ReadFormAsync();
                    foreach (var item in read.Files)
                    {
                        var name = item.FileName;
                        Console.WriteLine(name);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();
                        var path = @$"Resources\FileTest\{name}";

                        await using FileStream fs = new(path, FileMode.Create);
                        await item.CopyToAsync(fs);
                        Console.WriteLine(path);
                    }
                    await context.Response.WriteAsync("ok");
                });

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
