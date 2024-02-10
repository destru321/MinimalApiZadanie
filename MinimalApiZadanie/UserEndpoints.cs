using DataModels.DTOs;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiZadanie;

public class UserEndpoints
{
    public void RegisterUser(WebApplication app)
    {
        app.MapPost("/user/register", async (UserDTO user, MariaDbContext db) =>
        {
            if (await db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName) != null)
                return Results.StatusCode(StatusCodes.Status409Conflict);

            if (user.UserName == null || user.UserName == string.Empty || user.UserName == " ")
                return Results.StatusCode(StatusCodes.Status400BadRequest);
    
            if (user.Password == null || user.Password == string.Empty || user.Password == " ")
                return Results.StatusCode(StatusCodes.Status400BadRequest);

            string Hash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var newUser = new User { UserName = user.UserName, Hash = Hash };

            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            return Results.StatusCode(StatusCodes.Status201Created);
        });
    }
    
    public void LoginUser(WebApplication app)
    {
        app.MapPost("/user/login", async (UserDTO user, MariaDbContext db, HttpContext context) =>
        {
            var LoginUser = await db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            
            if (LoginUser == null)
                return Results.StatusCode(StatusCodes.Status409Conflict);

            if (user.UserName == null || user.UserName == string.Empty || user.UserName == " ")
                return Results.StatusCode(StatusCodes.Status400BadRequest);
    
            if (user.Password == null || user.Password == string.Empty || user.Password == " ")
                return Results.StatusCode(StatusCodes.Status400BadRequest);

            var Hash = BCrypt.Net.BCrypt.Verify(user.Password, LoginUser.Hash);
            Console.WriteLine(Hash);

            if (!Hash)
                // Return JWT
                return Results.StatusCode(StatusCodes.Status401Unauthorized);

            return Results.StatusCode(StatusCodes.Status200OK);
        });
    }
}