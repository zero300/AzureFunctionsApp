using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Prodcuct.Function.Model;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using System.Text.Json;

namespace Prodcuct.Function
{
    public class CustomerUpdate
    {
        private readonly ILogger<CustomerUpdate> _logger;

        public CustomerUpdate(ILogger<CustomerUpdate> logger)
        {
            _logger = logger;
        }

        [Function("CustomerUpdate")]
        public async static Task<CustomerOutputType> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, 
            FunctionContext executionContext
        )
        {
            var _logger = executionContext.GetLogger("CustomerUpdate");
            
            // 判斷傳值
            CustomerModel? customer = await JsonSerializer.DeserializeAsync<CustomerModel>(req.Body);
            if(customer is null){
                _logger.LogError("There is no CustomerInsertModel ");  
                throw new ArgumentException($"Missing JSON object. {nameof(CustomerModel)}");
            }

            // 當customer 為空的時候是 Insert 
            // 反之 Update  
            var message = "Welcome Azure";
            if(customer.CustomerID is null) {
                _logger.LogInformation("Customer Ready To Insert ");
                message = "Insert Test ";
            }
            else {
                _logger.LogInformation("Customer Ready To Update ");
                message = "Update Test ";
            }

            customer.rowguid ??= Guid.NewGuid();
            customer.ModifiedDate ??= DateTime.Now;

            // 回傳確認
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(message);


            return new CustomerOutputType(){
                CustomerModel = customer, 
                Response = response
            };
        }
    }
}
