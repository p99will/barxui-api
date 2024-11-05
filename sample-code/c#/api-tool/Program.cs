﻿using api_tool;
using api_tool.Models;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("barxui API Tool Sample");

// configuration
var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .Build();

// configuration  
var services = new ServiceCollection()
  .Configure<APIOptions>(configuration.GetSection("APIOptions"))
  .AddSingleton<App>()
  .AddLogging(configure => configure.AddSimpleConsole(options =>
    {
      options.IncludeScopes = false;
      options.SingleLine = true;
      options.TimestampFormat = "hh:mm:ss ";
    }))
  .BuildServiceProvider();

// create the app
var app = services.GetRequiredService<App>();

// parse command line and run the app
var parser = new Parser(with =>
{
  with.CaseInsensitiveEnumValues = true;
});

var parsedArgs = parser.ParseArguments<PrintOptions, ListOptions>(args);
await parsedArgs.WithParsedAsync<ListOptions>(async options => await app.ListAction(options));
await parsedArgs.WithParsedAsync<PrintOptions>(async options => await app.PrintAction(options));
parsedArgs.WithNotParsed(errors => Console.WriteLine("Invalid arguments"));

