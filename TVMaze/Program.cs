using Polly;
using TVMaze.Data;
using TVMaze.Data.Models;
using TVMaze.Data.Seeding;
using TVMaze.Data.Services;
using TVMaze.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("TVMazeDatabase");

// Add SQL Server data store
builder.Services.AddDbContext<TVMazeDbContext>(options =>
{
    options.UseSqlServer(connStr);
});

// Configure http client for scraping
builder.Services.AddHttpClient("TVMaze", options =>
{
    options.BaseAddress = new Uri("https://api.tvmaze.com");
})
//Configure retry policy for scraping
.AddPolicyHandler(
    Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
    .WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(4),
        TimeSpan.FromSeconds(8)
    },
    (result, timeSpan, retryCount, context) =>
    {
        Console.WriteLine($"Retry attempt {retryCount} for https://api.tvmaze.com. Waiting for {timeSpan.TotalSeconds} seconds.");
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});

builder.Services.AddScoped<DbContext, TVMazeDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddTransient<IShowsService, ShowsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

// Configure DB data
await SeedDb(app);

app.Run();

static async Task SeedDb(WebApplication app)
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var TVMazeDbContext = serviceScope.ServiceProvider.GetRequiredService<TVMazeDbContext>();

        TVMazeDbContext.Database.Migrate();

        var ShowsRepo = serviceScope.ServiceProvider.GetRequiredService<IRepository<Show>>();
        var PersonsRepo = serviceScope.ServiceProvider.GetRequiredService<IRepository<Person>>();
        var TVMazeClientFactory = serviceScope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        // Change last parameter to adjust number of records scraped
        await DataSeeder.Seed(PersonsRepo, ShowsRepo, TVMazeClientFactory, 1000);
    }
}