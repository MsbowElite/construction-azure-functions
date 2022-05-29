using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppCRUD
{
    public partial class ConstructionFunctions
    {
        [Function("ConstructionFunctionsList")]
        public async Task<IActionResult> ConstructionFunctionsList(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = _baseRoute)] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(ConstructionFunctions)} function processed a request.");

            return new OkObjectResult(await _constructionDataStore.ListAsync());
        }
    }
}
