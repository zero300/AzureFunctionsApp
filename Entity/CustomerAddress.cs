namespace Prodcuct.Entities;
public class CustomerAddress{
    public int CustomerID { get; set; }
    public int AddressID { get; set; }
    public string? AddressType { get; set; }
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }

    // FOREIGN KEY 反向
    public Customer Customer { get; set; }
    public Address Address { get; set; }
}