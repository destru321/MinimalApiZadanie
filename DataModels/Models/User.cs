using System.ComponentModel.DataAnnotations;

namespace DataModels.Models;

public class User
{
    [Required]
    public Guid Id { set; get; } = Guid.NewGuid();
    [Required]
    public string UserName { set; get; }
    [Required]
    public string Hash { set; get; }

    [Required] public string Role { set; get; } = "User";
    
    public List<Project> Projects { set; get; }
}