# CanAndGpsCapture
CAN bus & GPS data capture tool

## Introduction
This tool is used to capture CAN bus data and GPS data from a vehicle and GPS receiver.  
The tool is designed to be used with a GPS receiver and a Canable compatible CAN bus interface.  
  - [GPS Receiver](https://content.u-blox.com/sites/default/files/products/documents/u-blox7-V14_ReceiverDescriptionProtocolSpec_%28GPS.G7-SW-12001%29_Public.pdf)
    - Compatible product link: [VK-162 G-Mouse USB GPS Dongle](https://www.amazon.com/Onyehn-Navigation-External-Receiver-Raspberry/dp/B07GJGSZB9/ref=sr_1_4?crid=CA48J4SO2XJF&dib=eyJ2IjoiMSJ9.osETxs61LuDnf_khAtnDadmBSarFxosoNE-iw-RsxohxaE23WvLUCT32kD0GB45asbMagP5pkb9Hwfke3J5pe9AoVTn6TBY5iwxPRTqzF26ZfS04TX58UxW-PYkhzo2vpPevQ6UPIXbY7LBaf4if5Z69G_Vux6rimQFeg2pAazKEOtuCVVT4ziJrUF97YOqchfoU9SmHBlqA1E-1uBnWYJAAyCSap4keR2kB4gnpNqM.YPInRzh2x8dnSptJFAouXElmKAOA7N0BkwYGpCWnZlo&dib_tag=se&keywords=vk-162+gps&qid=1735986965&sprefix=vk-162+gps%2Caps%2C136&sr=8-4)
  - [Canable](https://canable.io/)
    - Compatible product link: [USB to CAN Adapter](https://www.amazon.com/ASHATA-Adapter-Converter-STM32F072-Candlelight/dp/B0BSFRL8D6/ref=sr_1_3?crid=K74J9O0NRDJH&dib=eyJ2IjoiMSJ9.L4CZEcyeYuL6iNLpiZmt5gFBbJ1zyHYsl4ZSfl8buRO0O7hB2uA2sZINnpefD-vTnaPFlZ4GJdyo165fvsuH47qrmsNNm14rVsd41vqn8pmKtsdPKCiDe2_KqPDH31jqgiM9s_1fGSNIX6CSv7ZUM38vtJ0Mc7Whryt6oYxyHMxmhCXac4XdaK3zFSlXmUXHMpghVYYUNXtEs5pxYonSXai-CVheFa6e6QFGdY5rYlg.yAS4sgYmCu2X0IsCEOyHAK9qpqftcQ-vTyxULw7Mtko&dib_tag=se&keywords=usb+to+can+adapter+STM32F072&qid=1735987013&sprefix=usb+to+can+adapter+stm32f072%2Caps%2C140&sr=8-3)
    - Reference application code: [cangaroo](https://github.com/normaldotcom/cangaroo/)  

## How to use

Before you run the **cagcap.exe**, check and update the configuration file `appsettings.json` if needed.  

### Configuration

  - GPS receiver enable: true or false
    - Port: The port name of the CAN bus interface.
    - BaudRate: The baud rate of the CAN bus interface.
  - CAN Bus enable: true or false
    - Port: The port name of the GPS receiver.

## Design

The tool is written in C# and provides the following features:
  - Initialize GPS receiver and CAN bus interfaces.  
  - Read GPS data and CAN bus data from the interfaces.
  - Save the data to an OpenTelemetry log.

The tool has the following architectural layers:
  - Domain type: Defines core types.
  - Business logic: Implements the high-level function.
  - Application: Implements the main application.
  - UI: Implements the user interface.
  - Framework: Implements the device-specific concrete function.
  - Composition root: Creates the actual instances.

### Domain type

  - Device: Represents a GPS receiver or a CAN bus interface.
    - Open & Close
    - Start & Stop receiving data
    - Take a logger to show and/or save the data

### Business logic

The business logic implements the high-level function of the application.  

### Application

The application implements the main application and manages its lifecycle.  
The application takes arguments from the outside to configure the composition root.  
The application uses business logic to implement its primary function for the clients.

### Framework

The framework implements the device-specific concrete function and data parser.  


### Composition root

The composition root creates the actual instances and configures the application.

## OTS items

The tool uses the following OTS items:

  - For CagCap project
     - CommandLineParser
     - LibUsbDotNet
     - Microsoft.Extensions.Configuration
     - Microsoft.Extensions.Configuration.Binder
     - Microsoft.Extensions.Configuration.Json
     - Microsoft.Extensions.DependencyInjection
     - Microsoft.Extensions.Logging.Console
     - Microsoft.Extensions.Options
     - OpenTelemetry
     - System.IO.Ports
     
  - For test projects
    - coverlet.collector
    - Microsoft.NET.Test.Sdk
    - Moq
    - xunit
    - xunit.runner.visualstudio
