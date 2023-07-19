using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Reflection;
using System.Text.Json.Serialization;
using TransactionsAPI.Database;
using TransactionsAPI.Database.Repositories;
using TransactionsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//Automapper definition
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase)
        );
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext registration
builder.Services.AddDbContext<TransDbContext>(opt => {

    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<TransDbContext>().Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();




string CreateConnectionString(IConfiguration configuration) {
    var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
    var pass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "transactions";
    var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";

    var connBuilder = new NpgsqlConnectionStringBuilder {
        Host = host,
        Port = int.Parse(port),
        Username = username,
        Database = databaseName,
        Password = pass,
        Pooling = true
    };

    return connBuilder.ConnectionString;
}