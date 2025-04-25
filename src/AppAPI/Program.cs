using Controllers;
using Controllers.FileControllers;
using Controllers.Parsers;
using Devices;

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

DeviceManager deviceManager = Factory.CreateDeviceManager(new TxtFileController("..\\input.txt"), new StringParser());

app.MapGet("/api/devices", () => deviceManager.AllDevices);
app.MapGet("/api/devices/{id}", (int id) => deviceManager.GetDeviceData(id));

app.MapPost("/api/devices", (string newDevice) =>
{
    if (deviceManager.TryGetDeviceFromText(newDevice, out Device createdDevice))
    {
        deviceManager.AddDevice(createdDevice);
        return Results.Created($"/api/devices/{createdDevice.Id}", newDevice);
    }
    return Results.BadRequest("Given specification is not good");
});
app.MapDelete("/api/devices/{id}", (int id) => deviceManager.TryRemoveDevice(id));

app.MapPut("/api/devices/{id}", (int id, string newDevice) =>
{
    if (deviceManager.TryGetDeviceFromText(newDevice, out Device createdDevice))
    {
        deviceManager.EditDeviceData(id, createdDevice);
        return Results.Accepted();
    }
    return Results.BadRequest("Given specification is not good");
});


app.Run();
