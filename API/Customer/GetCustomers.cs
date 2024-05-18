using System.Net;
using System.Threading;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;

namespace Prodcuct.Function
{
    public class GetCustomers(ILogger<GetCustomers> logger, DemoDbContext demoDbContext)
    {
        private readonly ILogger<GetCustomers> _logger = logger;
        private readonly DemoDbContext _demoDbContext = demoDbContext;
        [Function("GetCustomers")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCustomers")] HttpRequestData req
            ,CancellationToken cancellationToken
            )
        {
            _logger.LogInformation("GetCustomers API");
            IQueryable<Customer> Customer = _demoDbContext.Customers;
            // 排序控制 限1
            Customer = ApplyOrdering(Customer, req.Query.Get("orderCol"), req.Query.Get("orderDesc"));
            // 數量控制
            if(Int32.TryParse(req.Query.Get("limit"), out int limit )) Customer = Customer.Take(limit);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(Customer, cancellationToken);

            return response;
        }

        private IQueryable<Customer> ApplyOrdering(IQueryable<Customer> customers, string? orderCol = null , string? orderDesc = null)
        {
            return orderCol switch
            {
                "name" or "Name" => orderDesc?.ToUpper() == "DESC" ? 
                    customers.OrderByDescending(customer => customer.FirstName) : 
                    customers.OrderBy(customer => customer.FirstName) ,
                _ => customers.OrderBy(customer => customer.CustomerID),
            };
        }
    }
}
