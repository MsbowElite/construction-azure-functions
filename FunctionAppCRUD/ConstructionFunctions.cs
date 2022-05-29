using FunctionAppCRUD.Data;
using Microsoft.Extensions.Logging;

namespace FunctionAppCRUD
{
    public partial class ConstructionFunctions
    {
        private readonly IConstructionDataStore _constructionDataStore;
        private readonly ILogger _logger;
        private const string _baseRoute = "constructions";

        public ConstructionFunctions(IConstructionDataStore constructionDataStore, ILoggerFactory loggerFactory)
        {
            _constructionDataStore = constructionDataStore;
            _logger = loggerFactory.CreateLogger<ConstructionFunctions>();
        }
    }
}
