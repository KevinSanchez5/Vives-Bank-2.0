using Banco_VivesBank.Cliente.Controller;
using Banco_VivesBank.Cliente.Dto;
using Banco_VivesBank.Cliente.Exceptions;
using Banco_VivesBank.Cliente.Models;
using Banco_VivesBank.Cliente.Services;
using Banco_VivesBank.User.Dto;
using Banco_VivesBank.User.Exceptions;
using Banco_VivesBank.Utils.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Test.Cliente.Controller;

[TestFixture]
public class ClienteControllerTest
{
    private Mock<IClienteService> _clienteServiceMock;
    private ClienteController _clienteController;
    private Mock<PaginationLinksUtils> _paginationLinksUtils;

    [SetUp]
    public void SetUp()
    {
        _clienteServiceMock = new Mock<IClienteService>();
        _paginationLinksUtils = new Mock<PaginationLinksUtils>();
        _clienteController = new ClienteController(_clienteServiceMock.Object, _paginationLinksUtils.Object);
    }

    [Test]
    public async Task GetAll()
    {
        var response = new ClienteResponse
        {
            Guid = "guid",
            Nombre = "nombreTest",
            Apellidos =  "apellidosTest",
            Direccion = new Direccion {
                Calle = "calleTest",
                Numero = "numeroTest",
                CodigoPostal = "codigoPostalTest",
                Piso = "pisoTest",
                Letra = "letraTest"
            },
            Email = "emailTest",
            Telefono = "telefonoTest",
            FotoPerfil = "fotoPerfilTest",
            FotoDni = "fotoDniTest",
            UserResponse = new UserResponse 
            {
                Guid = "userGuid",
                Username = "usernameTest",
                Role = "roleTest",
                CreatedAt = "createdAtTest",
                UpdatedAt = "updatedAtTest",
                IsDeleted = false
            },
            CreatedAt = "createdAtTest",
            UpdatedAt = "updatedAtTest",
            IsDeleted = false
        };
        var pageRequest = new PageRequest
        {
            PageNumber = 1,
            PageSize = 20,
            SortBy = "id",
            Direction = "ASC"
        };
        var clientes = new PageResponse<ClienteResponse>
        {
            Content = new List<ClienteResponse> {response},
            PageSize = 1,
            SortBy = "id",
            Direction = "ASC"
        };
        clientes.Content.Add(response);
        _clienteServiceMock.Setup(service => service.GetAllPagedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),pageRequest)).ReturnsAsync(clientes);
        
        _paginationLinksUtils.Setup(utils => utils.CreateLinkHeader(clientes, It.IsAny<Uri>()))
            .Returns("<http://localhost/api/clientes>; rel=\"prev\",<http://localhost/api/clientes>; rel=\"next\"");

        // Configurar el contexto HTTP para la prueba
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Scheme = "http",
                Host = new HostString("localhost"),
                PathBase = new PathString("/api/clientes")
            }
        };
        _clienteController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        
        var result = await _clienteController.GetAllPaged(null, null, null, 1, 20, "id", "ASC");
        
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }

    [Test]
    public async Task GetAll_EmptyList()
    {
        var pageRequest = new PageRequest
        {
            PageNumber = 1,
            PageSize = 20,
            SortBy = "id",
            Direction = "ASC"
        };
        var clientes = new PageResponse<ClienteResponse>
        {
            Content = new List<ClienteResponse> (),
            PageSize = 1,
            SortBy = "id",
            Direction = "ASC"
        };     
        _clienteServiceMock.Setup(service => service.GetAllPagedAsync(null, null,null, pageRequest)).ReturnsAsync(clientes);
        
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Scheme = "http",
                Host = new HostString("localhost"),
                PathBase = new PathString("/api/clientes")
            }
        };
        _clienteController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        
        var baseUri = new Uri("http://localhost/api/clientes");
        _paginationLinksUtils.Setup(utils => utils.CreateLinkHeader(It.IsAny<PageResponse<ClienteResponse>>(), baseUri))
            .Returns("<http://localhost/api/clientes?page=0&size=5>; rel=\"prev\",<http://localhost/api/clientes?page=2&size=5>; rel=\"next\"");

        
        var result = await _clienteController.GetAllPaged(null, null,null, 1, 20, "id", "ASC");

        
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
    }
    
    [Test]
    public async Task GetByGuid()
    {
        var response = new ClienteResponse
        {
            Guid = "guid",
            Nombre = "nombreTest",
            Apellidos =  "apellidosTest",
            Direccion = new Direccion {
                Calle = "calleTest",
                Numero = "numeroTest",
                CodigoPostal = "codigoPostalTest",
                Piso = "pisoTest",
                Letra = "letraTest"
            },
            Email = "emailTest",
            Telefono = "telefonoTest",
            FotoPerfil = "fotoPerfilTest",
            FotoDni = "fotoDniTest",
            UserResponse = new UserResponse
            {
                Guid = "userGuid",
                Username = "usernameTest",
                Role = "roleTest",
                CreatedAt = "createdAtTest",
                UpdatedAt = "updatedAtTest",
                IsDeleted = false
            },
            CreatedAt = "createdAtTest",
            UpdatedAt = "updatedAtTest",
            IsDeleted = false
        };
        
        _clienteServiceMock.Setup(service => service.GetByGuidAsync(It.IsAny<string>()))
            .ReturnsAsync(response);
        var result = await _clienteController.GetByGuid("guid");
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());


        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.TypeOf<ClienteResponse>());


        var clienteResult = okResult.Value as ClienteResponse;
        Assert.That(clienteResult, Is.EqualTo(response));
    }
    
    [Test]
    public async Task GetByGuid_ClienteNotFound()
    {
        _clienteServiceMock.Setup(service => service.GetByGuidAsync(It.IsAny<string>())).ReturnsAsync((ClienteResponse)null);

        var result = await _clienteController.GetByGuid("nonexistent-guid");

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.Value, Is.EqualTo("No se ha encontrado cliente con guid: nonexistent-guid"));
    }

    [Test]
    public async Task Create()
    {
        // Arrange
        var clienteRequest = new ClienteRequest
        {
            Dni = "asdas",
            Nombre = "nombreTest",
            Apellidos = "apellidosTest",
            Calle = "calleTest",
            Numero = "numeroTest",
            CodigoPostal = "codigoPostalTest",
            Piso = "pisoTest",
            Letra = "letraTest",
            Email = "emailTest",
            Telefono = "telefonoTest",
            UserGuid = "userIdTest"
        };

        var clienteResponse = new ClienteResponse
        {
            Guid = "guid",
            Nombre = "nombreTest",
            Apellidos = "apellidosTest",
            Direccion = new Direccion
            {
                Calle = "calleTest",
                Numero = "numeroTest",
                CodigoPostal = "codigoPostalTest",
                Piso = "pisoTest",
                Letra = "letraTest"
            },
            Email = clienteRequest.Email,
            Telefono = clienteRequest.Telefono,
            UserResponse = new UserResponse
            {
                Guid = "userGuid",
                Username = "usernameTest",
                Role = "roleTest",
                CreatedAt = "createdAtTest",
                UpdatedAt = "updatedAtTest",
                IsDeleted = false
            },
            CreatedAt = "createdAtTest",
            UpdatedAt = "updatedAtTest",
            IsDeleted = false
        };

        _clienteServiceMock.Setup(service => service.CreateAsync(It.IsAny<ClienteRequest>()))
            .ReturnsAsync(clienteResponse);

        var result = await _clienteController.Create(clienteRequest);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(clienteResponse));
    }

    [Test]
    public async Task Create_BadRequest()
    {
        _clienteController.ModelState.AddModelError("Nombre", "El campo es obligatorio");
        var clienteRequest = new ClienteRequest();

        var result = await _clienteController.Create(clienteRequest);
        
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Create_ClienteExistsException()
    {
        var clienteRequest = new ClienteRequest
        {
            Nombre = "nombreTest",
            Apellidos = "apellidosTest"
        };

        _clienteServiceMock.Setup(service => service.CreateAsync(It.IsAny<ClienteRequest>()))
            .ThrowsAsync(new ClienteException("Cliente ya existe"));

        var result = await _clienteController.Create(clienteRequest);
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Cliente ya existe"));
    }

    [Test]
    public async Task Create_UserNotFound()
    {
        var clienteRequest = new ClienteRequest
        {
            Nombre = "nombreTest",
            Apellidos = "apellidosTest"
        };

        _clienteServiceMock.Setup(service => service.CreateAsync(It.IsAny<ClienteRequest>()))
            .ThrowsAsync(new UserException("Usuario no encontrado"));

        var result = await _clienteController.Create(clienteRequest);

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo("Usuario no encontrado"));
    }

    [Test]
    public async Task Update()
    {
        var guid = "valid-guid";
        var clienteRequestUpdate = new ClienteRequestUpdate
        {
            Nombre = "NuevoNombre",
            Apellidos = "NuevoApellido",
            Calle = "NuevaCalle",
            Numero = "NuevoNumero",
            CodigoPostal = "NuevoCodigoPostal",
            Piso = "NuevoPiso",
            Letra = "NuevaLetra",
            Email = "nuevoEmail@test.com",
            Telefono = "123456789"
        };

        var clienteResponse = new ClienteResponse
        {
            Guid = guid,
            Nombre = clienteRequestUpdate.Nombre,
            Apellidos = clienteRequestUpdate.Apellidos,
            Direccion =new Direccion
            {
                Calle = "NuevaCalle",
                Numero = "NuevoNumero",
                CodigoPostal = "NuevoCodigoPostal",
                Piso = "NuevoPiso",
                Letra = "NuevaLetra",
            },
            Email = clienteRequestUpdate.Email,
            Telefono = clienteRequestUpdate.Telefono,
            CreatedAt = "2024-01-01",
            UpdatedAt = "2024-01-10",
            IsDeleted = false
        };

        _clienteServiceMock.Setup(service => service.UpdateAsync(guid, clienteRequestUpdate))
            .ReturnsAsync(clienteResponse);

        var result = await _clienteController.UpdateCliente(guid, clienteRequestUpdate);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(clienteResponse));
    }

    [Test]
    public async Task Update_BadRequest()
    {
        var guid = "valid-guid";
        _clienteController.ModelState.AddModelError("Nombre", "El campo es requerido");
        var clienteRequestUpdate = new ClienteRequestUpdate();

        var result = await _clienteController.UpdateCliente(guid, clienteRequestUpdate);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_ClienteNotFound()
    {
        var guid = "nonexistent-guid";
        var clienteRequestUpdate = new ClienteRequestUpdate
        {
            Nombre = "NuevoNombre",
            Apellidos = "NuevoApellido"
        };

        _clienteServiceMock.Setup(service => service.UpdateAsync(guid, clienteRequestUpdate))
            .ReturnsAsync((ClienteResponse)null);

        var result = await _clienteController.UpdateCliente(guid, clienteRequestUpdate);

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo($"No se ha podido actualizar el cliente con guid: {guid}"));
    }

    [Test]
    public async Task UpdateCliente_ClienteExistsException_()
    {
        var guid = "valid-guid";
        var clienteRequestUpdate = new ClienteRequestUpdate
        {
            Nombre = "NuevoNombre",
            Apellidos = "NuevoApellido"
        };

        _clienteServiceMock.Setup(service => service.UpdateAsync(guid, clienteRequestUpdate))
            .ThrowsAsync(new ClienteException("Error al actualizar el cliente"));

        var result = await _clienteController.UpdateCliente(guid, clienteRequestUpdate);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Error al actualizar el cliente"));
    }
    
    [Test]
    public async Task DeleteByGuid()
    {
        var guid = "valid-guid";
        var clienteResponse = new ClienteResponse
        {
            Guid = guid,
            Nombre = "NombreTest",
            Apellidos = "ApellidosTest",
            IsDeleted = true
        };

        _clienteServiceMock.Setup(service => service.DeleteByGuidAsync(guid))
            .ReturnsAsync(clienteResponse);

        var result = await _clienteController.DeleteByGuid(guid);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(clienteResponse));
    }

    [Test]
    public async Task DeleteByGuid_ClienteNotFound()
    {
        var guid = "nonexistent-guid";

        _clienteServiceMock.Setup(service => service.DeleteByGuidAsync(guid))
            .ReturnsAsync((ClienteResponse)null);

        var result = await _clienteController.DeleteByGuid(guid);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo($"No se ha podido borrar el usuario con guid: {guid}"));
    }

    [Test]
    public async Task PatchFotoPerfil()
    {
        var guid = "valid-guid";
        var clienteResponse = new ClienteResponse
        {
            Guid = "guid",
            Nombre = "nombreTest",
            Apellidos =  "apellidosTest",
            Direccion = new Direccion {
                Calle = "calleTest",
                Numero = "numeroTest",
                CodigoPostal = "codigoPostalTest",
                Piso = "pisoTest",
                Letra = "letraTest"
            },
            Email = "emailTest",
            Telefono = "telefonoTest",
            FotoPerfil = "fotoPerfilTest",
            FotoDni = "fotoDniTest",
            UserResponse = new UserResponse
            {
                Guid = "userGuid",
                Username = "usernameTest",
                Role = "roleTest",
                CreatedAt = "createdAtTest",
                UpdatedAt = "updatedAtTest",
                IsDeleted = false
            },
            CreatedAt = "createdAtTest",
            UpdatedAt = "updatedAtTest",
            IsDeleted = false
        };

        var file = CreateMockFile("foto_perfil.jpg", "image/.jpeg");

        _clienteServiceMock.Setup(service => service.UpdateFotoPerfil(guid, It.IsAny<IFormFile>()))
            .ReturnsAsync(clienteResponse);

        var result = await _clienteController.PatchFotoPerfil(guid, file);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(clienteResponse));

    }

    [Test]
    public async Task PatchFotoPerfil_ClienteNotFound()
    {
        // Arrange
        var guid = "non-existent-guid";
        var mockFile = CreateMockFile("foto_perfil.jpg", "image/jpeg");

        _clienteServiceMock
            .Setup(service => service.UpdateFotoPerfil(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync((ClienteResponse)null);

        // Act
        var result = await _clienteController.PatchFotoPerfil(guid, mockFile);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.Value, Is.EqualTo("No se ha podido actualizar la foto de perfil del cliente con guid: non-existent-guid"));
    }
    
    [Test]
    public async Task PatchFotoDni()
    {
        // Arrange
        var guid = "cliente-guid";
        var mockFile = CreateMockFile("foto_dni.jpg", "image/jpeg");
        var expectedResponse = new ClienteResponse { Guid = guid, Nombre = "Juan" };

        _clienteServiceMock
            .Setup(service => service.UpdateFotoDni(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _clienteController.PatchFotoDni(guid, mockFile);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
    }
    
    [Test]
    public async Task PatchFotoDni_ClienteNotFound()
    {
        // Arrange
        var guid = "non-existent-guid";
        var mockFile = CreateMockFile("foto_dni.jpg", "image/jpeg");

        _clienteServiceMock
            .Setup(service => service.UpdateFotoDni(guid, It.IsAny<IFormFile>()))
            .ReturnsAsync((ClienteResponse)null);

        // Act
        var result = await _clienteController.PatchFotoDni(guid, mockFile);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.Value, Is.EqualTo("No se ha podido actualizar la foto del dni del cliente con guid: non-existent-guid"));
    }
    
    
    private IFormFile CreateMockFile(string fileName, string contentType)
    {
        var content = "Fake file content";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var formFile = new Mock<IFormFile>();
        
        formFile.Setup(f => f.FileName).Returns(fileName);
        formFile.Setup(f => f.Length).Returns(stream.Length);
        formFile.Setup(f => f.OpenReadStream()).Returns(stream);
        formFile.Setup(f => f.ContentType).Returns(contentType);
        
        return formFile.Object;
    }
}