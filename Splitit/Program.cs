using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Splitit.EntityFM;
using Splitit.Middleware;
using Splitit.Providers;
using Splitit.Services;
using Splitit.Services.interfaces;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string strProvider = Environment.GetEnvironmentVariable("provider") ?? "";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x=>x.EnableAnnotations());

builder.Services.AddDbContext<ActorsDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IActorService, ActorService>();

builder.Services.Configure<Dictionary<string, string>>(builder.Configuration.GetSection("Urls"));
builder.Services.AddHttpClient<IProvider>();
AddProvider(ref builder, strProvider);
builder.Services.AddScoped<IDBInitializerService, DBInitializerService>();
builder.Services.AddControllers();
builder.Services.AddScoped<ExceptionMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// initializing DB
using (var scope = app.Services.CreateAsyncScope())
{
    IDBInitializerService dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializerService>();
    await dbInitializer.InitializeDataBase();
}


app.Run();


void AddProvider(ref WebApplicationBuilder builder, string strProvider)
{
    ProviderTypes pt;

    if (Enum.TryParse(strProvider, out pt))
    {
        switch (pt)
        {
            case ProviderTypes.IMDB:

                builder.Services.AddScoped<IProvider, IMDBProvider>();
                break;
            case ProviderTypes.RottenTomatoes:
                builder.Services.AddScoped<IProvider, RottenTomatoesProvider>();
                break;
            default:
                builder.Services.AddScoped<IProvider, IMDBProvider>();
                break;
 
        }
        
    }
    else
    {
        builder.Services.AddScoped<IProvider, IMDBProvider>();
    }
}