using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Prodcuct.Entities;

public class CustomerDetailViewModel{
    public int? CustomerID {get; set;}
    public int? AddressID {get; set;}
    public bool NameStyle { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Suffix { get; set; }
    public string? CompanyName { get; set; }
    public string? SalesPerson { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? AddressType { get; set; }
    // public required string PasswordHash { get; set; }
    // public required string PasswordSalt { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? CountryRegion { get; set; }
    public string? PostalCode { get; set; }

    
    public CustomerDetailViewModel(Customer customer, CustomerAddress cusAdd, Address address){
        CustomerID = customer.CustomerID;
        NameStyle = customer.NameStyle;
        Title = customer.Title;
        FirstName = customer.FirstName;
        MiddleName = customer.MiddleName;
        LastName = customer.LastName;
        Suffix = customer.Suffix;
        CompanyName = customer.CompanyName;
        SalesPerson = customer.SalesPerson;
        EmailAddress = customer.EmailAddress;
        Phone = customer.Phone;

        AddressType = cusAdd.AddressType;
        
        AddressLine1 = address.AddressLine1;
        AddressLine2 = address.AddressLine2;
        City = address.City;
        StateProvince = address.StateProvince;
        CountryRegion = address.CountryRegion;
        PostalCode = address.PostalCode;
    }
}