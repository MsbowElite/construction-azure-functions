
using FunctionAppCRUD.Data.Entities;
using FunctionAppCRUD.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionAppCRUD
{
    public partial class ConstructionFunctions
    {
        [Function("ConstructionFunctionsAdd")]
        public async Task<IActionResult> ConstructionFunctionsAdd(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = _baseRoute)] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(ConstructionFunctions)} processed a request.");

            var construction =
                await req.Body.DeserializeAsync<Construction>();

            if (construction is null)
                return new BadRequestResult();

            await _constructionDataStore.AddAsync(construction);
            return new CreatedResult($"/{_baseRoute}/{construction.Id}", construction);
        }
    }
}
