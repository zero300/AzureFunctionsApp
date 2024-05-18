// using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.EntityFrameworkCore;

namespace Prodcuct.Function.Model{
    public class CustomerModel{
        public int? CustomerID {get; set;}
        public required bool NameStyle { get; set; }
        public string? Title { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public string? Suffix { get; set; }
        public string? CompanyName { get; set; }
        public string? SalesPerson { get; set; }
        public string? EmailAddress { get; set; }
        public string? Phone { get; set; }
        public required string PasswordHash { get; set; }
        
        public required string PasswordSalt { get; set; }
        public Guid? rowguid { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class CustomerOutputType{
        [SqlOutput("SalesLT.Customer", connectionStringSetting: "SqlConnectionString")]
        public required CustomerModel CustomerModel { get; set; }
        public required HttpResponseData Response { get; set; }
    }

}