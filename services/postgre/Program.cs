using Microsoft.Extensions.Configuration;
using Npgsql;
using postgre.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SchemeCreator>();
builder.Services.AddSingleton<DbConnectionProvider>();
builder.Services.AddSingleton<DataFiller>();
builder.Services.AddSingleton<DataHelper>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
