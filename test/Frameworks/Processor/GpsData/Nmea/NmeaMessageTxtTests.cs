// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageTxtTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            //01,01,02,HW  UBX-G70xx   00070000 
            var txtData = new[]
            {
                "01",
                "01",
                "02",
                "HW  UBX-G70xx   00070000 "
            };

            // Act
            var txtMessage = new NmeaMessageTxt(txtData, loggerMock);

            // Assert
            Assert.Equal(1, txtMessage.NumberOfMessages);
            Assert.Equal(1, txtMessage.MessageNumber);
            Assert.Equal(NmeaTextMessageType.Notice, txtMessage.TextMessageType);
            Assert.Equal("HW  UBX-G70xx   00070000 ", txtMessage.Message);
        }

        [Theory]
        [InlineData("00", NmeaTextMessageType.Error)]
        [InlineData("01", NmeaTextMessageType.Warning)]
        [InlineData("02", NmeaTextMessageType.Notice)]
        [InlineData("03", NmeaTextMessageType.Error)]   // Threat unsupport message type to error
        [InlineData("04", NmeaTextMessageType.Error)]   // Threat unsupport message type to error
        [InlineData("05", NmeaTextMessageType.Error)]   // Threat unsupport message type to error
        [InlineData("06", NmeaTextMessageType.Error)]   // Threat unsupport message type to error
        [InlineData("07", NmeaTextMessageType.User)]
        public void ParseNmeaTextMessageType(string textMessageType, NmeaTextMessageType expected)
        {
            // Arrange & Act
            var actual = NmeaMessageTxt.ParseNmeaTextMessageType(textMessageType);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var txtData = new[]
            {
                "01",
                "01",
                "02",
                "HW  UBX-G70xx   00070000 "
            };

            // Act
            var txtMessage = new NmeaMessageTxt(txtData, loggerMock);

            // Assert
            Assert.Equal("TXT: NumberOfMessages: 1, MessageNumber: 1, TextMessageType: Notice, Message: HW  UBX-G70xx   00070000 ", txtMessage.ToString());
        }
    }
}
