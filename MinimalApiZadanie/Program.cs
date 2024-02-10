using Microsoft.EntityFrameworkCore;
using MinimalApiZadanie;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MariaDbContext>(options =>
{
    var ConnString = builder.Configuration.GetConnectionString("MariaDbConnectionString");
    options.UseMySql(ConnString, ServerVersion.AutoDetect(ConnString));
});

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

var UserEndpoints = new UserEndpoints();
var ProjectEndpoints = new ProjectEndpoints();

UserEndpoints.RegisterUser(app);
UserEndpoints.LoginUser(app);

ProjectEndpoints.CreateProject(app);
ProjectEndpoints.GetProjects(app);
ProjectEndpoints.DeleteProject(app);
ProjectEndpoints.UpdateProject(app);
ProjectEndpoints.AssignUserToProject(app);

app.Run();