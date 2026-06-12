using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.Middleware;
using TraineeManagement.api.Repository.Mentor;
using TraineeManagement.api.Repository.Password;
using TraineeManagement.api.Repository.Task;
using TraineeManagement.api.Repository.TaskAssignment;
using TraineeManagement.api.Repository.Trainee;
using TraineeManagement.api.Repository.User;
using TraineeManagement.api.Services;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddScoped<ITraineeService, TraineeServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));


var app = builder.Build();

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