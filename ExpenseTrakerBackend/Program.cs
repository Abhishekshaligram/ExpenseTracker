using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PracticeCrud;
using PracticeCrud.Common.Config;
using PracticeCrud.Common.Helper;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Common.Settings;
using PracticeCrud.Common.SignalR.Notification;
using Quartz;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddQuartz(q =>
//{
//    q.SchedulerName = "BudgetNotificationScheduler";
//    q.UseMicrosoftDependencyInjectionJobFactory();

//    q.AddJob<LowBudgetNotificationJob>(options => options.WithIdentity("LowBudgetNotificationJob"));
//    q.AddTrigger(options =>
//    {
//        options.ForJob("LowBudgetNotificationJob")
//            .WithIdentity("LowBudgetNotificationTrigger")
//            .StartNow() // Start immediately
//            .WithSimpleSchedule(x => x
//                .WithIntervalInMinutes(30) // Run every 30 minutes
//                .RepeatForever());
//    });
//});

//builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

// Add services to the container.
builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(
     options => { 
         options.AddPolicy("AddCorePolicy", builder => {
         builder.WithOrigins("http://localhost:4300").AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
     });

    });
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Expense Traker API",
        Description = "Expense Traker .NET Core Web API"
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICSVService, CSVService>();
builder.Services.AddScoped<IBaseRepository,BaseRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IJwtAuthenticationService, JwtAuthenticationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

RegisterService.RegisterServices(builder.Services);
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AddCorePolicy");
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();
