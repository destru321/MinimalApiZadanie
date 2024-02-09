using System.ComponentModel.DataAnnotations;
using DataModels.Models;

namespace DataModels.DTOs;

public class ProjectDTO
{
    [Required]
    public string ProjectName { set; get; }
    [Required] 
    public string Description { set; get; }
}