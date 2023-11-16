namespace Eudic.Qwerty.Infrastructure.Services
{
    using Eudic.Qwerty.Core.Entities;
    using Eudic.Qwerty.Infrastructure.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class ExampleService : IExampleService
    {
        private readonly IConfigurationRoot config;
        private readonly ILogger<IExampleService> logger;
        private readonly SqliteConsoleContext context;

        public ExampleService(ILoggerFactory loggerFactory, IConfigurationRoot configurationRoot, SqliteConsoleContext sqliteConsoleContext)
        {
            logger = loggerFactory.CreateLogger<ExampleService>();
            config = configurationRoot;
            context = sqliteConsoleContext;
        }

        public void GetExamples()
        {
            logger.LogInformation($"All examples from database: {config["ConnectionStrings:DefaultConnection"]}");

            var examples = context.Examples
                .OrderBy(e => e.Name)
                .ToList();

            foreach (var example in examples)
            {
                logger.LogInformation($"Name: {example.Name}");
            }
        }

        public void AddExample(string name)
        {
            logger.LogInformation($"Adding example: {name}");

            var example = new Example()
            {
                Name = name
            };

            context.Examples.Add(example);
            context.SaveChanges();
        }
    }
}