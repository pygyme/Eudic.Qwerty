using Eudic.Qwerty.Console;
using Eudic.Qwerty.Infrastructure.Data;
using Eudic.Qwerty.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Configuration
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Build();

// Database
var optionsBuilder = new DbContextOptionsBuilder<SqliteConsoleContext>()
    .UseSqlite(configuration.GetConnectionString("DefaultConnection"));
var context = new SqliteConsoleContext(optionsBuilder.Options);
context.Database.EnsureCreated();

// Services
var services = new ServiceCollection()
    .AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
    })
    .AddSingleton(configuration)
    .AddSingleton(optionsBuilder.Options)
    .AddSingleton<IExampleService, ExampleService>()
    .AddSingleton<IDictService, DictService>()
    .AddDbContextPool<SqliteConsoleContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")))
    .BuildServiceProvider();

var logger = (services.GetService<ILoggerFactory>() ?? throw new InvalidOperationException())
    .CreateLogger<Program>();

logger.LogInformation($"Starting application at: {DateTime.Now}");

// Example Service
//var service = services.GetService<IExampleService>();
//service?.AddExample("Test A");
//service?.AddExample("Test B");
//service?.AddExample("Test C");
//service?.GetExamples();

var dictService = services.GetService<IDictService>();
var txtFile = configuration.GetValue<string>("InputTextFile");
var jsonFile = configuration.GetValue<string>("OutputJsonFile");
var converter = new QwertyLearnerConverter(dictService, txtFile);
// converter.Convert();


var filter = new CocaFilter(dictService, txtFile);
filter.FilterVerbToText();

await Task.Delay(1000);