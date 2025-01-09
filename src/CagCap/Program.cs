// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

using CagCap;
using CagCap.Application;
using CagCap.UI;

using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunApplication);
    }

    private static void RunApplication(Options opts)
    {
        if (opts.Help)
        {
            // The help text will be automatically displayed by the parser
            return;
        }

        // Set up configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Set up dependency injection
        var serviceCollection = ConfigureServices(configuration);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve and start the CagCapApp
        var cagCapApp = serviceProvider.GetRequiredService<CagCapApp>();
        cagCapApp.Start();
    }

    private static ServiceCollection ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        services.Configure<CanBusConfig>(configuration.GetSection("CanBus").Bind);
        services.Configure<GpsReceiverConfig>(configuration.GetSection("GpsReceiver").Bind);

        services.AddSingleton<IUserInterface, ConsoleUserInterface>();
        services.AddSingleton<CagCapApp>();

        return services;
    }
}

class Options
{
    [Option('h', "help", Required = false, HelpText = "Show help information.")]
    public bool Help { get; set; }
}
