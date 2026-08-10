using Microsoft.EntityFrameworkCore;
using market_watch.Data;
using Microsoft.Data.SqlClient;

// why we add this line????
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<MarketWatchTrainingContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

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





//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

//try
//{
//    using SqlConnection connection = new SqlConnection(connectionString);

//    connection.Open();

//    Console.WriteLine("Database connected successfully!");
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Connection failed!");
//    Console.WriteLine(ex.Message);
//}
