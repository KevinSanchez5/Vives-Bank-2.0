﻿using Banco_VivesBank.Frankfurter.Controller;
using Banco_VivesBank.Frankfurter.Model;
using Banco_VivesBank.Frankfurter.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

public class DivisasControllerTest
{
    private Mock<ILogger<DivisasController>> _logger;
    private Mock<IDivisasService> _divisasService;
    private DivisasController _controller;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<DivisasController>>();
        _divisasService = new Mock<IDivisasService>();
        _controller = new DivisasController(_divisasService.Object, _logger.Object);
    }

    [Test]
    public void ObtenerCambio()
    {
        var expectedResponse = new FrankFurterResponse 
        { 
            Amount = 1,
            Base = "EUR",
            Rates = new Dictionary<string, decimal> { { "USD", 1.13m } }
        };
    
        _divisasService
            .Setup(x => x.ObtenerUltimasTasas(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedResponse);

        var result = _controller.GetLatestRates("1", "EUR", "USD");

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public void ObtenerCambios()
    {
        var expectedResponse = new FrankFurterResponse
        {
            Amount = 1,
            Base = "EUR",
            Rates = new Dictionary<string, decimal> { { "USD", 1.13m } }
        };

        _divisasService
            .Setup(x => x.ObtenerUltimasTasas("EUR", null, "1"))
            .Returns(expectedResponse);

        var result = _controller.GetLatestRates();

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        _divisasService.Verify(x => x.ObtenerUltimasTasas("EUR", null, "1"), Times.Once);
    }
}