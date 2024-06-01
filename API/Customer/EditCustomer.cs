using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prodcuct.Entities;

namespace Prodcuct.Function
{
    public class EditCustomer(ILogger<EditCustomer> logger, DemoDbContext demoDbContext)
    {
        private readonly ILogger<EditCustomer> _logger = logger;
        private readonly DemoDbContext _demoDbContext = demoDbContext;
        
        /// <summary>
        /// 編輯Customer 資料
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="customer">編輯後的資源</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        [Function("EditCustomer")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "EditCustomer")] HttpRequestData req,
            [FromBody]Customer newCustomer, CancellationToken cancellationToken)
        {
            _logger.LogInformation("EditCustomer API");
            var response = req.CreateResponse();
            var exist = _demoDbContext.Customers.FirstOrDefault(c => c.CustomerID == newCustomer.CustomerID);
            // 先判斷存不存在 不存在即報錯。
            if(exist is null) response.StatusCode = HttpStatusCode.NotFound;
            else {
                newCustomer.CustomerID = exist.CustomerID;
                newCustomer.rowguid = exist.rowguid;
                _demoDbContext.Entry(exist).CurrentValues.SetValues(newCustomer);
            }
            
            // 確認更新
            try{
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
