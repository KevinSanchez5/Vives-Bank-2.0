﻿using Banco_VivesBank.Cliente.Exceptions;
using Banco_VivesBank.Movimientos.Controller;
using Banco_VivesBank.Movimientos.Dto;
using Banco_VivesBank.Movimientos.Exceptions;
using Banco_VivesBank.Movimientos.Services;
using Banco_VivesBank.Movimientos.Services.Movimientos;
using Banco_VivesBank.Producto.Cuenta.Exceptions;
using Banco_VivesBank.Producto.Tarjeta.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.Movimientos.Controller;

[TestFixture]
public class MovimientoControllerTests
{
    private Mock<IMovimientoService> _mockMovimientoService;
    private MovimientoController _controller;

    [SetUp]
    public void Setup()
    {
        _mockMovimientoService = new Mock<IMovimientoService>();
        _controller = new MovimientoController(_mockMovimientoService.Object);
    }
    
    [Test]
    public async Task GetAll()
    {
     
        var mockResponseList = new List<MovimientoResponse>
        {
            new MovimientoResponse { Guid = "1", ClienteGuid = "cliente1", CreatedAt = "01/01/2025" },
            new MovimientoResponse { Guid = "2", ClienteGuid = "cliente2", CreatedAt = "02/01/2025" }
        };

        _mockMovimientoService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(mockResponseList);
        
        var result = await _controller.GetAll();

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var responseList = okResult.Value as List<MovimientoResponse>;
        Assert.That(responseList, Is.Not.Null);
        Assert.That(responseList.Count, Is.EqualTo(mockResponseList.Count));
        
        foreach (var response in responseList)
        {
            Assert.That(response.CreatedAt, Is.Not.Null);
        }
    }

    [Test]
    public async Task GetAll_NoExisteMovimiento()
    {
      
        var mockResponseList = new List<MovimientoResponse>();

        _mockMovimientoService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(mockResponseList);
        
        var result = await _controller.GetAll();
        
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var responseList = okResult.Value as List<MovimientoResponse>;
        Assert.That(responseList, Is.Not.Null);
        Assert.That(responseList.Count, Is.EqualTo(0)); // Esperamos que la lista esté vacía
    }
    
    [Test]
    public async Task GetById()
    {
        var guid = "1";
        var mockMovimiento = new MovimientoResponse 
        { 
            Guid = guid, 
            ClienteGuid = "cliente1", 
            CreatedAt = "01/01/2025" 
        };

        _mockMovimientoService.Setup(service => service.GetByGuidAsync(guid))
            .ReturnsAsync(mockMovimiento);
        
        var result = await _controller.GetById(guid);
        
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    
        var responseValue = okResult.Value as MovimientoResponse;
        Assert.That(responseValue, Is.Not.Null);
    
        Assert.That(responseValue.Guid, Is.EqualTo(mockMovimiento.Guid));
        Assert.That(responseValue.ClienteGuid, Is.EqualTo(mockMovimiento.ClienteGuid));
        Assert.That(responseValue.CreatedAt, Is.EqualTo(mockMovimiento.CreatedAt));
        
        Assert.That(responseValue.Domiciliacion, Is.Null);
        Assert.That(responseValue.IngresoNomina, Is.Null);
        Assert.That(responseValue.PagoConTarjeta, Is.Null);
        Assert.That(responseValue.Transferencia, Is.Null);
    }
    
    [Test]
    public async Task GetById_NotFound()
    {
        var guid = "non-existent-guid";
        _mockMovimientoService.Setup(service => service.GetByGuidAsync(guid))
            .ReturnsAsync((MovimientoResponse)null);
        
        var result = await _controller.GetById(guid);
        
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo($"No se ha encontrado el movimiento con guid: {guid}"));
    }
    
    [Test]
    public async Task GetByClienteGuid()
    {
        var clienteGuid = "cliente1";
        var mockResponseList = new List<MovimientoResponse>
        {
            new MovimientoResponse { Guid = "1", ClienteGuid = clienteGuid, CreatedAt = "01/01/2025" },
            new MovimientoResponse { Guid = "2", ClienteGuid = clienteGuid, CreatedAt = "02/01/2025" }
        };

        _mockMovimientoService.Setup(service => service.GetByClienteGuidAsync(clienteGuid))
            .ReturnsAsync(mockResponseList);
        
        var result = await _controller.GetByClienteGuid(clienteGuid);
        
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    
        var responseList = okResult.Value as List<MovimientoResponse>;
        Assert.That(responseList, Is.Not.Null);
        Assert.That(responseList.Count, Is.EqualTo(mockResponseList.Count));
        
        foreach (var response in responseList)
        {
            Assert.That(response.ClienteGuid, Is.EqualTo(clienteGuid));
        }
    }
    
    [Test]
    public async Task GetByClienteGuid_ListaVacia()
    {
        var clienteGuid = "cliente1";
        var mockResponseList = new List<MovimientoResponse>();

        _mockMovimientoService.Setup(service => service.GetByClienteGuidAsync(clienteGuid))
            .ReturnsAsync(mockResponseList);
        var result = await _controller.GetByClienteGuid(clienteGuid);
        var okResult = result.Result as OkObjectResult;
        
        Assert.That(okResult, Is.Not.Null);

        var responseList = okResult.Value as List<MovimientoResponse>;
        Assert.That(responseList, Is.Not.Null);
        Assert.That(responseList.Count, Is.EqualTo(0));
    }
    
    
    [Test]
    public async Task CreateIngresoNomina()
    {
        var ingresoNominaRequest = new IngresoNominaRequest
        {
            NombreEmpresa = "Empresa S.A.",
            CifEmpresa = "B12345678",
            IbanEmpresa = "ES1234567890",
            IbanCliente = "ES0987654321",
            Importe = 1500.0
        };

        var mockResponse = new IngresoNominaResponse
        {
            NombreEmpresa = ingresoNominaRequest.NombreEmpresa,
            CifEmpresa = ingresoNominaRequest.CifEmpresa,
            IbanEmpresa = ingresoNominaRequest.IbanEmpresa,
            IbanCliente = ingresoNominaRequest.IbanCliente,
            Importe = ingresoNominaRequest.Importe
        };

        _mockMovimientoService.Setup(service => service.CreateIngresoNominaAsync(ingresoNominaRequest))
            .ReturnsAsync(mockResponse);
        
        var result = await _controller.CreateIngresoNomina(ingresoNominaRequest);
        var okResult = result.Result as OkObjectResult;
        
        Assert.That(okResult, Is.Not.Null);

        var responseValue = okResult.Value as IngresoNominaResponse;
        Assert.That(responseValue, Is.Not.Null);
        Assert.That(responseValue.NombreEmpresa, Is.EqualTo(mockResponse.NombreEmpresa));
        Assert.That(responseValue.CifEmpresa, Is.EqualTo(mockResponse.CifEmpresa));
        Assert.That(responseValue.IbanEmpresa, Is.EqualTo(mockResponse.IbanEmpresa));
        Assert.That(responseValue.IbanCliente, Is.EqualTo(mockResponse.IbanCliente));
        Assert.That(responseValue.Importe, Is.EqualTo(mockResponse.Importe));
    }
    
    [Test]
    public async Task CreateIngresoNomina_CuentaException()
    {
        var ingresoNominaRequest = new IngresoNominaRequest
        {
            NombreEmpresa = "Empresa S.A.",
            CifEmpresa = "B12345678",
            IbanEmpresa = "ES1234567890",
            IbanCliente = "ES0987654321",
            Importe = 1500.0
        };

        var exceptionMessage = "Cuenta no encontrada";

        _mockMovimientoService.Setup(service => service.CreateIngresoNominaAsync(ingresoNominaRequest))
            .ThrowsAsync(new CuentaException(exceptionMessage));

        var result = await _controller.CreateIngresoNomina(ingresoNominaRequest);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreateIngresoNomina_MovimientoException()
    {
        var ingresoNominaRequest = new IngresoNominaRequest
        {
            NombreEmpresa = "Empresa S.A.",
            CifEmpresa = "B12345678",
            IbanEmpresa = "ES1234567890",
            IbanCliente = "ES0987654321",
            Importe = 1500.0
        };

        var exceptionMessage = "Error en el movimiento";

        _mockMovimientoService.Setup(service => service.CreateIngresoNominaAsync(ingresoNominaRequest))
            .ThrowsAsync(new MovimientoException(exceptionMessage));
        
        var result = await _controller.CreateIngresoNomina(ingresoNominaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreateIngresoNomina_Invalido()
    {
       
        var ingresoNominaRequest = new IngresoNominaRequest
        {
            NombreEmpresa = "", 
            CifEmpresa = "", 
            IbanEmpresa = "ES123", 
            IbanCliente = "", 
            Importe = -1500.0 
        };

        _controller.ModelState.AddModelError("NombreEmpresa", "El nombre de la empresa es obligatorio");
        _controller.ModelState.AddModelError("CifEmpresa", "El CIF de la empresa es obligatorio");
        _controller.ModelState.AddModelError("IbanEmpresa", "El IBAN de la empresa no es válido");
        _controller.ModelState.AddModelError("IbanCliente", "El IBAN del cliente es obligatorio");
        _controller.ModelState.AddModelError("Importe", "El importe debe ser positivo");
        
        var result = await _controller.CreateIngresoNomina(ingresoNominaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.TypeOf<SerializableError>());
    }
    
    [Test]
    public async Task CreatePagoConTarjeta()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "Tienda S.A.",
            Importe = 100.0,
            NumeroTarjeta = "4111111111111111"
        };

        var mockResponse = new PagoConTarjetaResponse
        {
            NombreComercio = pagoConTarjetaRequest.NombreComercio,
            Importe = pagoConTarjetaRequest.Importe,
            NumeroTarjeta = pagoConTarjetaRequest.NumeroTarjeta
        };

        _mockMovimientoService.Setup(service => service.CreatePagoConTarjetaAsync(pagoConTarjetaRequest))
            .ReturnsAsync(mockResponse);
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var okResult = result.Result as OkObjectResult;
        
        Assert.That(okResult, Is.Not.Null);

        var responseValue = okResult.Value as PagoConTarjetaResponse;
        Assert.That(responseValue, Is.Not.Null);
        Assert.That(responseValue.NombreComercio, Is.EqualTo(mockResponse.NombreComercio));
        Assert.That(responseValue.Importe, Is.EqualTo(mockResponse.Importe));
        Assert.That(responseValue.NumeroTarjeta, Is.EqualTo(mockResponse.NumeroTarjeta));
    }
    
    [Test]
    public async Task CreatePagoConTarjeta_TarjetaException()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "Tienda S.A.",
            Importe = 100.0,
            NumeroTarjeta = "4111111111111111"
        };

        var exceptionMessage = "Tarjeta no válida";

        _mockMovimientoService.Setup(service => service.CreatePagoConTarjetaAsync(pagoConTarjetaRequest))
            .ThrowsAsync(new TarjetaException(exceptionMessage));
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreatePagoConTarjeta_SaldoCuentaInsuficientException()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "Tienda S.A.",
            Importe = 100.0,
            NumeroTarjeta = "4111111111111111"
        };

        var exceptionMessage = "Saldo insuficiente";

        _mockMovimientoService.Setup(service => service.CreatePagoConTarjetaAsync(pagoConTarjetaRequest))
            .ThrowsAsync(new SaldoCuentaInsuficientException(exceptionMessage));
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreatePagoConTarjeta_CuentaException()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "Tienda S.A.",
            Importe = 100.0,
            NumeroTarjeta = "4111111111111111"
        };

        var exceptionMessage = "Cuenta no encontrada";

        _mockMovimientoService.Setup(service => service.CreatePagoConTarjetaAsync(pagoConTarjetaRequest))
            .ThrowsAsync(new CuentaException(exceptionMessage));
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreatePagoConTarjeta_MovimientoException()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "Tienda S.A.",
            Importe = 100.0,
            NumeroTarjeta = "4111111111111111"
        };

        var exceptionMessage = "Error en el movimiento";

        _mockMovimientoService.Setup(service => service.CreatePagoConTarjetaAsync(pagoConTarjetaRequest))
            .ThrowsAsync(new MovimientoException(exceptionMessage));
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    
    [Test]
    public async Task CreatePagoConTarjeta_Invalido()
    {
        var pagoConTarjetaRequest = new PagoConTarjetaRequest
        {
            NombreComercio = "", 
            Importe = -100.0,
            NumeroTarjeta = "123" 
        };

        _controller.ModelState.AddModelError("NombreComercio", "El nombre del comercio es obligatorio");
        _controller.ModelState.AddModelError("Importe", "El importe debe ser un número positivo");
        _controller.ModelState.AddModelError("NumeroTarjeta", "El número de tarjeta no es válido");
        
        var result = await _controller.CreatePagoConTarjeta(pagoConTarjetaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.TypeOf<SerializableError>());
    }
    
    
    [Test]
    public async Task CreateTransferencia()
    {
        var transferenciaRequest = new TransferenciaRequest
        {
            IbanOrigen = "ES9121000418450200051332",
            IbanDestino = "ES4721000418450200051333",
            NombreBeneficiario = "Juan Pérez",
            Importe = 100.0
        };

        var mockResponse = new TransferenciaResponse
        {
            IbanOrigen = transferenciaRequest.IbanOrigen,
            IbanDestino = transferenciaRequest.IbanDestino,
            NombreBeneficiario = transferenciaRequest.NombreBeneficiario,
            Importe = transferenciaRequest.Importe,
            Revocada = false
        };

        _mockMovimientoService.Setup(service => service.CreateTransferenciaAsync(transferenciaRequest))
            .ReturnsAsync(mockResponse);
        
        var result = await _controller.CreateTransferencia(transferenciaRequest);
        var okResult = result.Result as OkObjectResult;
        
        Assert.That(okResult, Is.Not.Null);

        var responseValue = okResult.Value as TransferenciaResponse;
        Assert.That(responseValue, Is.Not.Null);
        Assert.That(responseValue.IbanOrigen, Is.EqualTo(mockResponse.IbanOrigen));
        Assert.That(responseValue.IbanDestino, Is.EqualTo(mockResponse.IbanDestino));
        Assert.That(responseValue.NombreBeneficiario, Is.EqualTo(mockResponse.NombreBeneficiario));
        Assert.That(responseValue.Importe, Is.EqualTo(mockResponse.Importe));
        Assert.That(responseValue.Revocada, Is.False);
    }
    
    [Test]
    public async Task CreateTransferencia_SaldoCuentaInsuficientException()
    {
       
        var transferenciaRequest = new TransferenciaRequest
        {
            IbanOrigen = "ES9121000418450200051332",
            IbanDestino = "ES4721000418450200051333",
            NombreBeneficiario = "Juan Pérez",
            Importe = 1000.0
        };

        var exceptionMessage = "Saldo insuficiente";

        _mockMovimientoService.Setup(service => service.CreateTransferenciaAsync(transferenciaRequest))
            .ThrowsAsync(new SaldoCuentaInsuficientException(exceptionMessage));
        
        var result = await _controller.CreateTransferencia(transferenciaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreateTransferencia_CuentaException()
    {
        var transferenciaRequest = new TransferenciaRequest
        {
            IbanOrigen = "ES9121000418450200051332",
            IbanDestino = "ES4721000418450200051333",
            NombreBeneficiario = "Juan Pérez",
            Importe = 100.0
        };

        var exceptionMessage = "Cuenta no encontrada";

        _mockMovimientoService.Setup(service => service.CreateTransferenciaAsync(transferenciaRequest))
            .ThrowsAsync(new CuentaException(exceptionMessage));
        
        var result = await _controller.CreateTransferencia(transferenciaRequest);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task CreateTransferencia_MovimientoException()
    {
      
        var transferenciaRequest = new TransferenciaRequest
        {
            IbanOrigen = "ES9121000418450200051332",
            IbanDestino = "ES4721000418450200051333",
            NombreBeneficiario = "Juan Pérez",
            Importe = 100.0
        };

        var exceptionMessage = "Error al procesar la transferencia";

        _mockMovimientoService.Setup(service => service.CreateTransferenciaAsync(transferenciaRequest))
            .ThrowsAsync(new MovimientoException(exceptionMessage));
        
        var result = await _controller.CreateTransferencia(transferenciaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    
    [Test]
    public async Task CreateTransferencia_Invalido()
    {
     
        var transferenciaRequest = new TransferenciaRequest
        {
            IbanOrigen = "", 
            IbanDestino = "",
            NombreBeneficiario = "Juan Pérez",
            Importe = -100.0 
        };

        _controller.ModelState.AddModelError("IbanOrigen", "El IBAN de origen es obligatorio");
        _controller.ModelState.AddModelError("IbanDestino", "El IBAN de destino es obligatorio");
        _controller.ModelState.AddModelError("Importe", "El importe debe ser un número positivo");
        
        var result = await _controller.CreateTransferencia(transferenciaRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.TypeOf<SerializableError>());
    }
    [Test]
    public async Task RevocarTransferencia()
    {
        var movimientoGuid = "movimiento1";

        var mockResponse = new TransferenciaResponse
        {
            IbanOrigen = "ES9121000418450200051332",
            IbanDestino = "ES4721000418450200051333",
            NombreBeneficiario = "Juan Pérez",
            Importe = 100.0,
            Revocada = true
        };

        _mockMovimientoService.Setup(service => service.RevocarTransferenciaAsync(movimientoGuid))
            .ReturnsAsync(mockResponse);
        
        var result = await _controller.RevocarTransferencia(movimientoGuid);
        var okResult = result.Result as OkObjectResult;
        
        Assert.That(okResult, Is.Not.Null);

        var responseValue = okResult.Value as TransferenciaResponse;
        Assert.That(responseValue, Is.Not.Null);
        Assert.That(responseValue.IbanOrigen, Is.EqualTo(mockResponse.IbanOrigen));
        Assert.That(responseValue.IbanDestino, Is.EqualTo(mockResponse.IbanDestino));
        Assert.That(responseValue.NombreBeneficiario, Is.EqualTo(mockResponse.NombreBeneficiario));
        Assert.That(responseValue.Importe, Is.EqualTo(mockResponse.Importe));
        Assert.That(responseValue.Revocada, Is.True);
    }
    
    [Test]
    public async Task RevocarTransferencia_MovimientoNotFoundException()
    {
        var movimientoGuid = "movimiento1";
        var exceptionMessage = "Movimiento no encontrado";

        _mockMovimientoService.Setup(service => service.RevocarTransferenciaAsync(movimientoGuid))
            .ThrowsAsync(new MovimientoNotFoundException(exceptionMessage));
        
        var result = await _controller.RevocarTransferencia(movimientoGuid);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task RevocarTransferencia_MovimientoException()
    {
     
        var movimientoGuid = "movimiento1";
        var exceptionMessage = "Error al revocar el movimiento";

        _mockMovimientoService.Setup(service => service.RevocarTransferenciaAsync(movimientoGuid))
            .ThrowsAsync(new MovimientoException(exceptionMessage));
        
        var result = await _controller.RevocarTransferencia(movimientoGuid);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task RevocarTransferencia_SaldoCuentaInsuficientException()
    {
        var movimientoGuid = "movimiento1";
        var exceptionMessage = "Saldo insuficiente para revocar la transferencia";

        _mockMovimientoService.Setup(service => service.RevocarTransferenciaAsync(movimientoGuid))
            .ThrowsAsync(new SaldoCuentaInsuficientException(exceptionMessage));
        
        var result = await _controller.RevocarTransferencia(movimientoGuid);
        var badRequestResult = result.Result as BadRequestObjectResult;
        
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
    }
    
    [Test]
    public async Task RevocarTransferencia_CuentaException()
    {
        var movimientoGuid = "movimiento1";
        var exceptionMessage = "Cuenta no encontrada";

        _mockMovimientoService.Setup(service => service.RevocarTransferenciaAsync(movimientoGuid))
            .ThrowsAsync(new CuentaException(exceptionMessage));
        
        var result = await _controller.RevocarTransferencia(movimientoGuid);
        var notFoundResult = result.Result as NotFoundObjectResult;
        
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.Value, Is.EqualTo(exceptionMessage));
    }
    
}