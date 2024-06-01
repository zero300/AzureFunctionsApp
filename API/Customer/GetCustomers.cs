using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;
using API.Utils;

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
            Customer = Customer.OrderBySingle( req.Query.Get("orderCol"), req.Query.Get("orderDesc") is not null );
            // 數量控制
            var limitDetail = SQLUtils.ApplyLimit(Customer, Convert.ToInt32(req.Query.Get("limit")));

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(limitDetail, cancellationToken);

            return response;
        }
    }
}
