using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Beergam.Models;
using Microsoft.EntityFrameworkCore;
using Beergam.Services.User;
namespace Beergam.Services.Marketplace;

public enum MarketplaceType
{
    Meli,
    Shopee
}

[PrimaryKey(nameof(ShopId))]
public class MarketplaceModel : BaseModel
{
    [StringLength(100)]
    public required string ShopId { get; set; }
    [StringLength(255)]
    public required string Name { get; set; }
    [StringLength(255)]
    public required string AccessToken { get; set; }
    [StringLength(255)]
    public required string RefreshToken { get; set; }
    public required MarketplaceType Type { get; set; }
    
    [StringLength(9)]
    public required string UserPin { get; set; }
    
    [ForeignKey(nameof(UserPin))]
    public required UserModel User { get; set; }
    
}