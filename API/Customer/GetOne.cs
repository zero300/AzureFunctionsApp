using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;

namespace Prodcuct.Function
{
    public class GetOne(ILogger<GetOne> logger, DemoDbContext demoDbContext)
    {
        private readonly ILogger<GetOne> _logger = logger;
        private readonly DemoDbContext _demoDbContext = demoDbContext;
        [Function("GetOne")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCustomers/{rowguid}")] HttpRequestData req,
            string rowguid)
        {
            // 測試用CUSTOMERID : "7"
            // 測試用ROWGUID : "03e9273e-b193-448e-9823-fe0c44aeed78"
            _logger.LogInformation("GetOne API");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync( await _demoDbContext.Customers.SingleAsync(customer => customer.rowguid == Guid.Parse(rowguid) ));

            return response;
        }
    }
}
