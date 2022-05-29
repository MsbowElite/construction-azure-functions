using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppCRUD
{
    public partial class ConstructionFunctions
    {
        [Function("ConstructionFunctionsGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = _baseRoute+"/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation($"{nameof(ConstructionFunctions)} function processed a request.");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var construction =
                await _constructionDataStore.GetByIdAsync(id);

            if (construction == null) return new NotFoundResult();

            return new OkObjectResult(construction);
        }
    }
}
