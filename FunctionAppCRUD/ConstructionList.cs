using FunctionAppCRUD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppCRUD
{
    public class ConstructionList
    {
        private readonly IConstructionDataStore _constructionDataStore;
        private readonly ILogger _logger;
        private const string _baseRoute = "constructions";

        public ConstructionList(
            IConstructionDataStore constructionDataStore, ILoggerFactory loggerFactory)
        {
            _constructionDataStore = constructionDataStore;
            _logger = loggerFactory.CreateLogger<ConstructionList>();
        }

        [Function(nameof(ConstructionList))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = _baseRoute)] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(ConstructionList)} function processed a request.");

            return new OkObjectResult(await _constructionDataStore.ListAsync());
        }
    }
}
