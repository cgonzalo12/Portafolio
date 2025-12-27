using StackExchange.Redis;
using System.Reflection;
using Application.DTOs;
using Application.Interfaces;
using Application.Service;
using Infraestructure.Command;
using Infraestructure.Data;
using Infraestructure.Query;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.GetExecutingAssembly())
    .AddControllersAsServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CV Portfolio API",
        Version = "v1",
        Description = "API REST para gestionar CV Portfolio con Redis"
    });
});

// Redis Configuration
try
{
    var config = new ConfigurationOptions
    {
        EndPoints = { "redis-10086.c57.us-east-1-4.ec2.cloud.redislabs.com:10086" },
        User = "default",
        Password = "SdMsBfK8VbukBWurmNWcIQSxusrNtCkD",
        Ssl = false, // ⚠️ CAMBIO: Sin SSL para este endpoint
        AbortOnConnectFail = false,
        ConnectTimeout = 15000,
        SyncTimeout = 15000,
        AsyncTimeout = 15000,
        ConnectRetry = 3,
        KeepAlive = 60,
        AllowAdmin = false,
        DefaultDatabase = 0
    };

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("🔄 Conectando a Redis Cloud (Sin SSL)...");

        try
        {
            var connection = ConnectionMultiplexer.Connect(config);

            connection.ConnectionFailed += (sender, args) =>
            {
                logger.LogError($"❌ Connection Failed: {args.Exception?.Message ?? "Unknown"}");
            };

            connection.ConnectionRestored += (sender, args) =>
            {
                logger.LogInformation($"✅ Connection Restored: {args.EndPoint}");
            };

            // Esperar conexión
            logger.LogInformation("⏳ Esperando conexión...");
            var timeout = TimeSpan.FromSeconds(5);
            var start = DateTime.UtcNow;

            while (!connection.IsConnected && DateTime.UtcNow - start < timeout)
            {
                System.Threading.Thread.Sleep(200);
            }

            if (connection.IsConnected)
            {
                logger.LogInformation("✅ ¡Redis conectado exitosamente!");

                try
                {
                    var db = connection.GetDatabase();
                    var testKey = "test:connection";
                    var testValue = $"OK-{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";

                    db.StringSet(testKey, testValue);
                    var result = db.StringGet(testKey);

                    logger.LogInformation($"✅ Test READ/WRITE exitoso: {result}");
                }
                catch (Exception testEx)
                {
                    logger.LogError($"❌ Test READ/WRITE falló: {testEx.Message}");
                }
            }
            else
            {
                logger.LogError("❌ Redis NO pudo conectar");
            }

            return connection;
        }
        catch (Exception ex)
        {
            logger.LogError($"❌ Error: {ex.Message}");
            throw;
        }
    });

    builder.Services.AddScoped<IRedisContext, RedisContext>();

    Console.WriteLine("✅ Configuración de Redis completada");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error configurando Redis: {ex.Message}");
}

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500", "http://localhost:3000", "https://portafolio-front-end-eight.vercel.app")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register Command Handlers
builder.Services.AddScoped<ICommandHandler<UpdatePersonalInfoRequest, UpdatePersonalInfoResponse>, UpdatePersonalInfoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateExperienceRequest, CreateExperienceResponse>, CreateExperienceCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateExperienceRequest, UpdateExperienceResponse>, UpdateExperienceCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteExperienceRequest, DeleteExperienceResponse>, DeleteExperienceCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreatePersonalInfoRequest, UpdatePersonalInfoResponse>,CreatePersonalInfoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateSkillRequest, CreateSkillResponse>,CreateSkillCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateEducationRequest, CreateEducationResponse>,CreateEducationCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateProjectRequest, CreateProjectResponse>,CreateProjectCommandHandler>();


// Register Query Handlers
builder.Services.AddScoped<IQueryHandler<GetPersonalInfoRequest, GetPersonalInfoResponse>, GetPersonalInfoQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllExperiencesRequest, GetAllExperiencesResponse>, GetAllExperiencesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetExperienceByIdRequest, GetExperienceByIdResponse>, GetExperienceByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllSkillsRequest, GetAllSkillsResponse>, GetAllSkillsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetSkillsByCategoryRequest, GetAllSkillsResponse>,GetSkillsByCategoryQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllEducationRequest, GetAllEducationResponse>,GetAllEducationQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllProjectsRequest, GetAllProjectsResponse>,GetAllProjectsQueryHandler>();

// Register Services
builder.Services.AddScoped<IPersonalInfoService, PersonalInfoService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();


var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CV Portfolio API v1");
    c.RoutePrefix = "swagger";
});

//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseRouting();
app.UseAuthorization();

// Test endpoint
app.MapGet("/", () => new {
    message = "CV Portfolio API is running",
    version = "1.0",
    endpoints = new[] {
        "/api/personalinfo",
        "/api/experience",
        "/api/skill",
        "/api/project",
        "/api/education"
    }
});

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}