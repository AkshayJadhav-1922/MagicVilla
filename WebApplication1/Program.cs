//using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//To use serilog we have to install two packages - serilog and file
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.text", rollingInterval: RollingInterval.Hour).
//    CreateLogger();

//builder.WebHost.UseSerilog();

builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

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
