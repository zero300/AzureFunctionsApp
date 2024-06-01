using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;
using API.Utils;

namespace Prodcuct.Function
{
    public class GetCustomerDetail(ILogger<GetCustomerDetail> logger, DemoDbContext demoDbContext)
    {
        private readonly ILogger<GetCustomerDetail> _logger = logger;
        private readonly DemoDbContext _demoDbContext = demoDbContext;
        [Function("GetCustomerDetail")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCustomerDetail")] HttpRequestData req
            ,CancellationToken cancellationToken
            )
        {
            _logger.LogInformation("GetCustomerDetail API");
            
            // 測試  直接讀資料
            // string json = await File.ReadAllTextAsync(@"C:\LearnData\雲端學習\AzureFunctionsApp\test.json", cancellationToken);
            // Customer? customer = JsonSerializer.Deserialize<Customer>(json);
            // _logger.LogInformation(customer.CompanyName + customer.FirstName);

            var CustomerDetail = _demoDbContext.CustomerAddress
                .Include( ca => ca.Customer)
                .Include(ca => ca.Address)
                .OrderBySingle("CustomerID", true);

            var limitDetail = SQLUtils.ApplyLimit(CustomerDetail, Convert.ToInt32(req.Query.Get("limit")));

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(limitDetail, cancellationToken);

            return response;
        }
    }
}
