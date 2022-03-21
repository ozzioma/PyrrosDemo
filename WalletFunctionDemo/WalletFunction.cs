using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WalletDomain;

namespace WalletFunctionDemo
{
    public class WalletFunction
    {
        private readonly ILogger<WalletFunction> logger;
        private readonly WalletDbContext dbContext;
        public WalletFunction(ILogger<WalletFunction> _log, WalletDbContext _dbcontext)
        {
            logger = _log;
            dbContext = _dbcontext;
        }

        [FunctionName("PostDebit")]
        [OpenApiOperation(operationId: "PostDebit")]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(WalletDTO), Description = "Debit transaction body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> PostDebit(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "debit")] HttpRequest req)
        {

            try
            {

                logger.LogInformation("C# HTTP trigger function processed a request.");

                string responseMessage = $"Successfully debited account";

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                WalletDTO debitRequest = JsonConvert.DeserializeObject<WalletDTO>(requestBody);

                var walletAccount = dbContext.Wallets.FirstOrDefault(x => x.Account == debitRequest.Account);

                if (walletAccount == null)
                {
                    responseMessage = $"Invalid account details: {debitRequest.Account}";
                    return new NotFoundObjectResult(responseMessage);

                }

                if (walletAccount.Amount < debitRequest.Amount)
                {
                    responseMessage = $"Insufficient balance error";
                    return new BadRequestObjectResult(responseMessage);
                }

                var debitTranscation = new Wallet
                {
                    Account = debitRequest.Account,
                    Amount = debitRequest.Amount,
                    Direction = debitRequest.Direction
                };

                dbContext.Wallets.Add(debitTranscation);


                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing PostDebit");

                return new BadRequestObjectResult("Error executing PostDebit");
            }

        }
    }
}

