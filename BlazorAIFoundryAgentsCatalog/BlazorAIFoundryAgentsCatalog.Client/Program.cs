using BlazorAIFoundryAgentsCatalog.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAgentCatalogService, AgentCatalogService>();
builder.Services.AddScoped<IAgentChatService, AgentChatService>();

await builder.Build().RunAsync();
