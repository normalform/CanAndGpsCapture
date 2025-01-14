// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap
{
    using CagCap.Application;
    using CagCap.DomainObject;
    using CagCap.DomainObject.Device;
    using CagCap.Frameworks.Device.Adapter;
    using CagCap.Frameworks.Device.Canable;
    using CagCap.Frameworks.Device.UbloxGps;
    using CagCap.Frameworks.Processor.GpsData;
    using CagCap.UI;

    using CommandLine;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    static class Program
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

            var userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var commonUserAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var userConfigFilePath = Path.Combine(userAppData, "CagCap", "CagCapSettings.json");
            var commonUserConfigFilePath = Path.Combine(commonUserAppData, "CagCap", "CagCapSettings.json");
            var defaultConfigFilePath = Path.Combine(AppContext.BaseDirectory, "CagCapSettings.json");

            // Determine which configuration file to use
            var configFilePath = defaultConfigFilePath;
            if (File.Exists(userConfigFilePath))
            {
                configFilePath = userConfigFilePath;
            }
            else
            {
                if (File.Exists(commonUserConfigFilePath))
                {
                    configFilePath = commonUserConfigFilePath;
                }
            }

            // Set up configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
                .Build();

            // Set up dependency injection
            var serviceCollection = ConfigureServices(configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // TODO It's temporary for testing the low-level devices. Remove this when the high-level design is ready
            _ = serviceProvider.GetRequiredService<IGpsReceiver>();
            _ = serviceProvider.GetRequiredService<ICanableDevice>();

            // create application
            var cagCapApp = serviceProvider.GetRequiredService<ICagCapApp>();
            cagCapApp.Start();
        }

        private static ServiceCollection ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddOpenTelemetry();
            });

            services.Configure<CanBusConfig>(configuration.GetSection("CanBus").Bind);
            services.Configure<GpsReceiverConfig>(configuration.GetSection("GpsReceiver").Bind);
            services.AddSingleton<ICagCapApp, CagCapApp>();
            services.AddSingleton<IUsbUtils, UsbUtils>();
            services.AddSingleton<IUsbAccess, UsbAccess>();
            services.AddSingleton<IGpsDataProcessor, GpsDataProcessor>();
            services.AddSingleton<IUserInterface, ConsoleUserInterface>();
            services.AddSingleton<IGpsReceiver, GpsReceiver>();
            services.AddSingleton<IGpsReceiverDevice, GpsReceiverDeviceAdapter>();

            services.AddSingleton<ICanableDevice>(provider =>
            {
                var usbUtils = provider.GetRequiredService<IUsbUtils>();
                var guids = UsbAccess.GetDeviceGuid(usbUtils);
                var logger = provider.GetRequiredService<ILogger<CanableDevice>>();
                if (guids.Length != 0)
                {
                    if (guids.Length > 1)
                    {
                        logger.LogInformation("Multiple CANable devices found. Using the first one.");
                    }
                    else
                    {
                        var config = provider.GetRequiredService<IOptions<CanBusConfig>>().Value;
                        var usbAccess = provider.GetRequiredService<IUsbAccess>();
                        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                        return new CanableDevice(usbAccess, config, loggerFactory);
                    }
                }
                else
                {
                    logger.LogWarning("No CANable device found.");
                }

                return new NullCanableDevice();
            });

            services.AddSingleton<IUbloxGpsReceiverDevice>(
                provider =>
                {
                    var config = provider.GetRequiredService<IOptions<GpsReceiverConfig>>().Value;
                    if (config.Enable)
                    {
                        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                        return new UbloxGpsReceiverDevice(config.Port, config.BaudRate, loggerFactory);
                    }

                    return new NullUbloxGpsReceiverDevice();
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
}
