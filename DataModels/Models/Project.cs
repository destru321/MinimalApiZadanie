using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace DataModels.Models;

public class Project
{
    [Required]
    public Guid Id { set; get; } = Guid.NewGuid();
    [Required]
    public string ProjectName { set; get; }
    [Required]
    public string Description { set; get; }

    [Required] public DateOnly StartDate { set; get; } = DateOnly.FromDateTime(DateTime.Now);

    [Required] public DateOnly EndDate { set; get; } = DateOnly.FromDateTime(DateTime.Now);
    
    public List<User> Users { set; get; }
}