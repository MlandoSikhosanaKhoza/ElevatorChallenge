using ElevatorChallenge.Api.Dependancies;
using ElevatorChallenge.Services.Hubs;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic();
builder.Services.AddSignalR();
builder.Services.AddLogging(configure => configure.AddConsole());

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.WithOrigins("https://localhost:7017").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Make a display a real time position of the elevator
app.MapHub<ElevatorStatusHub>("/elevator/status");
app.MapHub<PassengerStatusHub>("/passenger/status");

app.UseCors(options =>
{
    options.WithOrigins("https://localhost:7017").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
