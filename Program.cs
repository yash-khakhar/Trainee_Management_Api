using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Serilog;
using Serilog.Formatting.Json;
using System.Text;
using System.Text.Json.Serialization;
using TraineeManagement.api.Data;
using TraineeManagement.api.Helper;
using TraineeManagement.api.HttpClientFactory;
using TraineeManagement.api.Middleware;
using TraineeManagement.api.Redis.Repository;
using TraineeManagement.api.Redis.Service;
using TraineeManagement.api.Repository.FileStorage;
using TraineeManagement.api.Repository.Mentor;
using TraineeManagement.api.Repository.Password;
using TraineeManagement.api.Repository.ProcessingJob;
using TraineeManagement.api.Repository.RabbitMQ;
using TraineeManagement.api.Repository.Review;
using TraineeManagement.api.Repository.Submission;
using TraineeManagement.api.Repository.Task;
using TraineeManagement.api.Repository.TaskAssignment;
using TraineeManagement.api.Repository.Trainee;
using TraineeManagement.api.Repository.User;
using TraineeManagement.api.Services;

var builder = WebApplication.CreateBuilder(args);

var year = DateTime.Now.ToString("yyyy");
var month = DateTime.Now.ToString("MM");
var day = DateTime.Now.ToString("dd");
var logPath = $"Logs/Year-{year}/Month-{month}/Day-{day}/log-.txt";

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(
        formatter: new JsonFormatter(),
        path: logPath,
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();

//builder.Services.AddProblemDetails();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument();
builder.Services.AddValidation();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorageSettings")
);


builder.Services.AddScoped<ITraineeService, TraineeServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IRedisCacheRepo, RedisCacheService>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();

builder.Services.AddTransient<SubmissionFileValidator>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "Dev:";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<CorrelationIdHandler>();

builder.Services.AddHttpClient<DummyTraineeService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5062/");
    client.Timeout = TimeSpan.FromSeconds(15); // Total overall lifecycle timeout guard
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "ConsumerAPI-Client");
})
.AddHttpMessageHandler<CorrelationIdHandler>()
// Adds standard 5-layered pipeline: Request Timeout, Retry, Circuit Breaker, Attempt Timeout, Rate Limiter
.AddStandardResilienceHandler(options =>
{
    // 1. Configure Retries (Only triggers for transient HTTP status codes like 5xx or 408)
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(2);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true; // Prevents overwhelming downstream service

    // 2. Configure Circuit Breaker (Trips if too many failures occur consecutively)
    options.CircuitBreaker.FailureRatio = 0.5; // Trip if 50% of requests fail in a block
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 8; // At least 8 requests must pass through before calculating ratio
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15); // How long circuit stays open

    // 3. Finite Attempt Timeout (Limits how long an individual retry attempt can take)
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(4);
});


var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<HttpStatusCodeHandler>();

//app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(myAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();