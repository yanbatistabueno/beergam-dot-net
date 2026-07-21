using System.ComponentModel.DataAnnotations;
using Beergam.Models;
using Microsoft.EntityFrameworkCore;
using Beergam.Services.Marketplace;
namespace Beergam.Services.User;

public enum UserRole
{
    Master,
    Colab,
}

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Pin), IsUnique = true)]
[PrimaryKey(nameof(Pin))]
public class UserModel : BaseModel
{
    [StringLength(255)]
    public required string Name { get; set; }
    
    [StringLength(9)]
    public required string Pin { get; set; }
    
    [StringLength(9)]
    public string? MasterPin { get; set; }
    [StringLength(255)]
    public required string Password { get; set; }
    public required bool IsActive { get; set; }
    public required UserRole Role { get; set; }
    [StringLength(255)]
    public required string Email { get; set; }

    public ICollection<MarketplaceModel> Marketplaces { get; set; } = new List<MarketplaceModel>();
}