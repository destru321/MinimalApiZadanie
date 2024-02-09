using System.ComponentModel.DataAnnotations;
using DataModels.Models;

namespace DataModels.DTOs;

public class UserDTO
{
    [Required]
    public string UserName { set; get; }
    [Required]
    public string Password { set; get; }
}