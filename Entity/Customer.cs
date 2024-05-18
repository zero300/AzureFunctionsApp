using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Prodcuct.Entities;
public class Customer{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? CustomerID {get; set;}
    [Required]
    public bool NameStyle { get; set; }
    public string? Title { get; set; }
    [Required]
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required]
    public required string LastName { get; set; }
    public string? Suffix { get; set; }
    public string? CompanyName { get; set; }
    public string? SalesPerson { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    [Required]
    public required string PasswordSalt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Guid? rowguid { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? ModifiedDate { get; set; }
}