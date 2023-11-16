namespace Eudic.Qwerty.Infrastructure.Services
{
    using Eudic.Qwerty.Core.Entities;
    using Eudic.Qwerty.Infrastructure.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    public class DictService : IDictService
    {
        private readonly IConfigurationRoot config;
        private readonly ILogger<IDictService> logger;
        private readonly SqliteConsoleContext context;

        public DictService(ILoggerFactory loggerFactory, IConfigurationRoot configurationRoot, SqliteConsoleContext sqliteConsoleContext)
        {
            logger = loggerFactory.CreateLogger<DictService>();
            config = configurationRoot;
            context = sqliteConsoleContext;
        }

        public Dict? GetDict(string word)
        {
            var examples = context.StarDict.Where(x => x.Word.Equals(word)).ToList();

            //foreach (var example in examples)
            //{
            //    logger.LogInformation($"Word: {example.Word} Definition: {example.Definition} Translation: {example.Translation}");
            //}

            return examples.FirstOrDefault();
        }
    }
}
