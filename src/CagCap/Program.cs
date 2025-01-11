// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

using CagCap;
using CagCap.Application;
using CagCap.Frameworks.Device.Canable;
using CagCap.Frameworks.Device.UbloxGps;
using CagCap.UI;

using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(RunApplication);
    }

    private static void RunApplication(Options opts)
    {
        if (opts.Help)
        {
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
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        var gpsConfig = serviceProvider.GetRequiredService<IOptions<GpsReceiverConfig>>().Value;
        if (gpsConfig.Enable)
        {
            var gpsReceiverUblox = serviceProvider.GetRequiredService<IUbloxGpsReceiverDevice>();
        }

        var canBusConfig = serviceProvider.GetRequiredService<IOptions<CanBusConfig>>().Value;
        if (canBusConfig.Enable)
        {
            var usbUtils = serviceProvider.GetRequiredService<IUsbUtils>();
            var guids = UsbAccess.GetDeviceGuid(usbUtils);
            if (guids.Length != 0)
            {
                if (guids.Length > 1)
                {
                    logger.LogInformation("Multiple CANable devices found. Using the first one.");
                }

                var canBus = serviceProvider.GetRequiredService<ICanableDevice>();
                var message = new CanMessage(CanId.StandardCanId(0x114), [0x01, 0x02, 0x03, 0x04]);
                canBus.SendMessage(message);
            }
            else
            {
                logger.LogWarning("No CANable device found.");
            }
        }

        var cagCapApp = serviceProvider.GetRequiredService<CagCapApp>();
        cagCapApp.Start();
    }

    private static ServiceCollection ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        services.Configure<CanBusConfig>(configuration.GetSection("CanBus").Bind);
        services.Configure<GpsReceiverConfig>(configuration.GetSection("GpsReceiver").Bind);

        services.AddSingleton<CagCapApp>();

        services.AddSingleton<IUsbUtils, UsbUtils>();
        services.AddSingleton<IUsbAccess, UsbAccess>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<CanBusConfig>>().Value;
            var logger = provider.GetRequiredService<ILogger<UsbAccess>>();
            return new UsbAccess(logger);
        });

        services.AddSingleton<ICanableDevice, CanableDevice>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<CanBusConfig>>().Value;
            var usbAccess = provider.GetRequiredService<IUsbAccess>();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            return new CanableDevice(usbAccess, config, loggerFactory);
        });

        services.AddSingleton<IUbloxGpsReceiverDevice>(
            provider =>
            {
                var config = provider.GetRequiredService<IOptions<GpsReceiverConfig>>().Value;
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                return new UbloxGpsReceiverDevice(config.Port, config.BaudRate, loggerFactory);
            });

        services.AddSingleton<IUserInterface, ConsoleUserInterface>();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            loggingBuilder.AddOpenTelemetry();
        });

        return services;
    }
}

class Options
{
    [ExcludeFromCodeCoverage]
    [Option('h', "help", Required = false, HelpText = "Show help information.")]
    public bool Help { get; set; }
}
