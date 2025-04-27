using Controllers;
using Controllers.FileControllers;
using Controllers.Parsers;
using Devices;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

DeviceManager deviceManager;
//Device Manager based on txt file
//deviceManager = Factory.CreateDeviceManager(new TxtFileController("..\\input.txt"), new StringParser());
//Device Manager based on mssql database
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null)
{
    Console.WriteLine("DIDN'T SPECIFI THE DEFAULT CONNECTION");
    return;
}
DBFileController dbController = new DBFileController(connectionString);
deviceManager = Factory.CreateDeviceManager(dbController, new StringParser(), dbController);

app.MapGet("/api/devices", () => 
{
    return deviceManager.GetDeviceData();
});

app.MapGet("/api/devices/{id}", (string id) => 
{
    string? deviceData = deviceManager.GetDeviceData(id);
    if (deviceData != null)
        return deviceData;
    return "Didn't found object with given id";
});

app.MapPost("/api/devices", (string newDevice) =>
{
    if (deviceManager.TryGetDeviceFromText(newDevice, out Device createdDevice))
    {
        deviceManager.AddDevice(createdDevice, true);
        return Results.Created($"/api/devices/{createdDevice.Id}", newDevice);
    }
    return Results.BadRequest("Given specification is not good");
});
app.MapDelete("/api/devices/{id}", (string id) => deviceManager.TryRemoveDevice(id));

app.MapPut("/api/devices/{id}", (string id, string newDevice) =>
{
    if (deviceManager.TryGetDeviceFromText(newDevice, out Device createdDevice))
    {
        deviceManager.EditDeviceData(id, createdDevice);
        return Results.Accepted();
    }
    return Results.BadRequest("Given specification is not good");
});

app.Run();