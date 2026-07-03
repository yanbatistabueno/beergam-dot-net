using System.ComponentModel.DataAnnotations;

namespace Beergam.Models;

public enum UserRole
{
    Master,
    Colab,
}

public class User : BaseModel
{
    [StringLength(255)]
    public required string Name { get; set; }
    [StringLength(255)]
    public required string Pin { get; set; }
    [StringLength(255)]
    public string? MasterPin { get; set; }
    [StringLength(255)]
    public required string Password { get; set; }
    public required bool IsActive { get; set; }
    public required UserRole Role { get; set; }
    [StringLength(255)]
    public required string Email { get; set; }
}