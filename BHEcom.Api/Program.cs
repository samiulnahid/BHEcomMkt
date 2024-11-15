using BHEcom.Api;
using Serilog;
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: "App_Data/Log/log-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(config => {
    config.AddConsole();
    config.AddDebug();
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //  app.UseDeveloperExceptionPage();
    // app.UseStaticFiles(); // Ensure static files are served
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //    c.RoutePrefix = string.Empty; // To serve Swagger UI at application's root (http://localhost:<port>/)
    //    c.InjectJavascript("/js/swagger-custom.js");
    //});

}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowSpecificOrigin");
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

startup.Configure(app, app.Environment);

app.MapControllers();

app.Run();


