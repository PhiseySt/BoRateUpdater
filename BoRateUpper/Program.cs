using BoRateUpper;
using BoRateUpper.Models;
using BoRateUpper.Services;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var paramValues = config.GetRequiredSection("Settings").Get<Settings>();
int.TryParse(paramValues.DelayMinSec, out var delayMinSec);
int.TryParse(paramValues.DelayMaxSec, out var delayMaxSec);

Console.Write("Введите количество накруток: ");
int.TryParse(Console.ReadLine(), out var countRateUp);

var fileData = new RequestFileService();
var rateUpper = new RateUpperService();
for (var i = 1; i<= countRateUp; i++ )
{
    rateUpper.RateUp(fileData.Referer, fileData.Cookies, fileData.Body);
    var currentDelay = rateUpper.GenerateDelay(delayMinSec, delayMaxSec);
    Thread.Sleep(currentDelay*1000);
}

Console.WriteLine("Программа окончила работу.");
Console.ReadLine();