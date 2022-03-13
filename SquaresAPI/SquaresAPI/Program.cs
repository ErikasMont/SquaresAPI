using SquaresAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SquaresAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPointsService, PointsService>();
builder.Services.AddScoped<ISquaresService, SquaresService>();

var connectionString = builder.Configuration.GetConnectionString("ApiDb");
builder.Services.AddDbContext<ApiDbContext>(option => option.UseSqlServer(connectionString));


var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>()
    .CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetService<ApiDbContext>();

    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
