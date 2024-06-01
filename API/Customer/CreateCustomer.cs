using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;

namespace Prodcuct.Function
{
    public class CreateCustomer(ILogger<CreateCustomer> logger, DemoDbContext demoDbContext)
    {
        private readonly ILogger<CreateCustomer> _logger = logger;
        private readonly DemoDbContext _demoDbContext = demoDbContext;
        
        /// <summary>
        /// 編輯Customer 資料
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="customer">編輯後的資源</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        [Function("CreateCustomer")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateCustomer")] HttpRequestData req,
            [FromBody]Customer newCustomer, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateCustomer API");
            var response = req.CreateResponse();
            var exist = _demoDbContext.Customers.FirstOrDefault(c => c.CustomerID == newCustomer.CustomerID);
            // 先判斷存不存在 存在即報錯， 因為是新增， 不應該有值。
            if(exist is not null) {
                response.StatusCode = HttpStatusCode.Conflict;
                return response;
            }
            
            newCustomer.CustomerID = null;
            newCustomer.rowguid = null;
            newCustomer.ModifiedDate = null;

            // 確認更新
            try{
                await _demoDbContext.Customers.AddAsync(newCustomer);
                await _demoDbContext.SaveChangesAsync(cancellationToken);
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(newCustomer, cancellationToken);
            }catch(Exception e){
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync($"Edit Error:{e.Message}");
            }
            return response;
        }
    }
}
