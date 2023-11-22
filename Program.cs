using Microsoft.EntityFrameworkCore;
using anime_climax_api.Database;
using anime_climax_api.Models;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Data;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text.Json;
// using GraphQL.SystemTextJson;
// using GraphQL.Server.Transports.AspNetCore.SystemTextJson;
// using GraphQL.Server;
var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var AnimeClimaxOnly = "AnimeClimaxOnly";

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql("Host=publisher-11849.7tt.cockroachlabs.cloud;Port=26257;Database=anime_climax;Username=pichan;Password=05N6AQXVg18ayJ22FnbFnQ;SslMode=VerifyFull"
));



builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    options.AddPolicy(name: AnimeClimaxOnly,
        policy =>
        {
            policy.WithOrigins(
                "https://anime-climax.vercel.app/",
                "https://www.anime-climax.vercel.app/",
                // "http://localhost:3000"
            ).AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddGraphQLServer().RegisterDbContext<DataContext>().AddQueryType<Query>().AddFiltering().AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpLogging();
app.UseAuthorization();

app.MapGraphQL();
//      app.UsePlayground(new PlaygroundOptions
//   {
//       QueryPath = "/graphql",
//       Path = "/playground"
//   });

await app.RunAsync();
