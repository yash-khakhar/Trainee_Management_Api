using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TraineeManagement.api.Data;
using TraineeManagement.api.repository;
using TraineeManagement.api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument();
builder.Services.AddValidation();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
            {
                policy.WithOrigins("http://localhost:5220")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddScoped<ITraineeService, TraineeServices>();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("TraineeManagementDb"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
