using CIM.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MudBlazor.Services;
using BlazorDownloadFile;

namespace CIM.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazorDownloadFile(ServiceLifetime.Scoped);
            builder.Services.AddMudServices();
            builder.Services.AddSingleton(services =>
            {
                
                  var baseUrl = services.GetRequiredService<NavigationManager>().BaseUri;
                ChannelBase channel = GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
                {
                    HttpHandler =  new GrpcWebHandler(new HttpClientHandler())
                });
                return new Greeter.GreeterClient(channel);
            });
            await builder.Build().RunAsync();
        }
    }
}
