using System.Text.Json;
using DataModels.DTOs;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MinimalApiZadanie;

public class ProjectEndpoints
{
    public void CreateProject(WebApplication app)
    {
        app.MapPost("/project", async ([FromBody]JsonElement data, MariaDbContext db) =>
        {
            string roleName = data.GetProperty("RoleName").GetString();
            var project = data.GetProperty("Project").Deserialize<ProjectDTO>();
            
            if (roleName == "Admin")
            {
                if (await db.Projects.FirstOrDefaultAsync(x => x.ProjectName == project.ProjectName) != null)
                    return Results.StatusCode(StatusCodes.Status409Conflict);

                if (project.ProjectName == null || project.ProjectName == string.Empty || project.ProjectName == " ")
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
    
                if (project.Description == null || project.Description == string.Empty || project.Description == " ")
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
            
                var newProject = new Project() { ProjectName = project.ProjectName, Description = project.Description };

                await db.Projects.AddAsync(newProject);
                await db.SaveChangesAsync();

                return Results.StatusCode(StatusCodes.Status201Created);
            }

            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        });
    }

    public void GetProjects(WebApplication app)
    {
        app.MapGet("/project", async (MariaDbContext db) => await db.Projects.ToListAsync());
    }

    public void DeleteProject(WebApplication app)
    {
        app.MapDelete("/project/{id}", async ([FromBody]JsonElement data, Guid id, MariaDbContext db) =>
        {
            string roleName = data.GetProperty("RoleName").GetString();
            
            if (roleName == "Admin")
            {
                var deleteProject = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);

                if (deleteProject == null)
                    return Results.StatusCode(StatusCodes.Status404NotFound);

                db.Projects.Remove(deleteProject);
                await db.SaveChangesAsync();

                return Results.StatusCode(StatusCodes.Status200OK);
            }

            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        });
    }

    public void UpdateProject(WebApplication app)
    {
        app.MapPut("/project/{id}", async ([FromBody]JsonElement data,Guid id, MariaDbContext db) =>
        {
            string roleName = data.GetProperty("RoleName").GetString();
            var project = data.GetProperty("Project").Deserialize<ProjectDTO>();

            if (roleName == "Admin")
            {
                var updateProject = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);
            
                if (project.ProjectName == null || project.ProjectName == string.Empty || project.ProjectName == " ")
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
    
                if (project.Description == null || project.Description == string.Empty || project.Description == " ")
                    return Results.StatusCode(StatusCodes.Status400BadRequest);

                updateProject.ProjectName = project.ProjectName;
                updateProject.Description = project.Description;

                await db.SaveChangesAsync();

                return Results.StatusCode(StatusCodes.Status200OK);
            }

            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        });
    }

    public void AssignUserToProject(WebApplication app)
    {
        app.MapPost("/project/assign", async ([FromBody] JsonElement data, MariaDbContext db) =>
        {
            string roleName = data.GetProperty("RoleName").GetString();
            var projectId = Guid.Parse(data.GetProperty("ProjectId").GetString());
            var userId = Guid.Parse(data.GetProperty("UserId").GetString());

            if (roleName == "Admin")
            {
                if (projectId == null || userId == null)
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
                
                List<User> users = new List<User> { await db.Users.FirstOrDefaultAsync(x => x.Id == userId) };

                var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

                if (project == null)
                    return Results.StatusCode(StatusCodes.Status404NotFound);
                
                project.Users = users;

                await db.SaveChangesAsync();

                return Results.StatusCode(StatusCodes.Status201Created);
            }

            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        }).RequireAuthorization("Admin");
    }
}