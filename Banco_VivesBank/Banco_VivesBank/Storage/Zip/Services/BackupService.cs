﻿using System.IO.Compression;
using Banco_VivesBank.Cliente.Dto;
using Banco_VivesBank.Cliente.Services;
using Banco_VivesBank.Producto.Base.Dto;
using Banco_VivesBank.Producto.Base.Services;
using Banco_VivesBank.Producto.Cuenta.Dto;
using Banco_VivesBank.Producto.Cuenta.Services;
using Banco_VivesBank.Producto.Tarjeta.Dto;
using Banco_VivesBank.Producto.Tarjeta.Services;
using Banco_VivesBank.Storage.Backup.Exceptions;
using Banco_VivesBank.Storage.Json.Service;
using Banco_VivesBank.User.Dto;
using Banco_VivesBank.User.Service;
using Banco_VivesBank.Utils.Generators;
using Banco_VivesBank.Utils.Pagination;

public class BackupService : IBackupService
{
    private readonly ILogger<BackupService> _logger;
    
    //servicios
    private readonly IUserService _userService;
    private readonly IClienteService _clienteService;
    private readonly IBaseService _baseService;
    private readonly ICuentaService _cuentaService;
    private readonly ITarjetaService _tarjetaService;
    //private readonly IMovimientoService _movimientoService;

    
    //storageJson
    private readonly IStorageJson _storageJson;
    
    public BackupService(ILogger<BackupService> logger, 
        IUserService userService, 
        IClienteService clienteService, 
        IBaseService baseService, 
        ICuentaService cuentaService, 
        ITarjetaService tarjetaService,
        //IMovimientoService movimientoService
        IStorageJson storageJson
        )
    {
        _logger = logger;
        
        //inicializamos los servicios
        _userService = userService;
        _clienteService = clienteService;
        _baseService = baseService;
        _cuentaService = cuentaService;
        _tarjetaService = tarjetaService;
        //_movimientoService = movimientoService;
        
        //inicializamos el storageJson
        _storageJson = storageJson;
    }

    public async Task ExportToZip(string sourceDirectory, string zipFilePath)
    {
        _logger.LogInformation("Iniciando la Exportación de datos a ZIP...");

        try
        {
            // Obtén los datos de forma asíncrona
            var users = await _userService.GetAllAsync();
            var clientes = await _clienteService.GetAllAsync();
            var bases = await _baseService.GetAllAsync();
            var cuentas = await _cuentaService.GetAll(
                saldoMax: null, 
                saldoMin: null, 
                tipoCuenta: null, 
                pageRequest: new PageRequest { PageNumber = 1, PageSize = 10 }
            );
            var tarjetas = await _tarjetaService.GetAllAsync();

            // Exporta los datos a archivos JSON
            _storageJson.ExportJson(new FileInfo(Path.Combine(sourceDirectory, "usuarios.json")), users.ToList());
            _storageJson.ExportJson(new FileInfo(Path.Combine(sourceDirectory, "clientes.json")), clientes.ToList());
            _storageJson.ExportJson(new FileInfo(Path.Combine(sourceDirectory, "bases.json")), bases.ToList());
            _storageJson.ExportJson(new FileInfo(Path.Combine(sourceDirectory, "cuentas.json")), cuentas.Content.ToList());
            _storageJson.ExportJson(new FileInfo(Path.Combine(sourceDirectory, "tarjetas.json")), tarjetas);

            // Crea el archivo ZIP
            using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                zipArchive.CreateEntryFromFile(Path.Combine(sourceDirectory, "usuarios.json"), "usuarios.json");
                zipArchive.CreateEntryFromFile(Path.Combine(sourceDirectory, "clientes.json"), "clientes.json");
                zipArchive.CreateEntryFromFile(Path.Combine(sourceDirectory, "bases.json"), "bases.json");
                zipArchive.CreateEntryFromFile(Path.Combine(sourceDirectory, "cuentas.json"), "cuentas.json");
                zipArchive.CreateEntryFromFile(Path.Combine(sourceDirectory, "tarjetas.json"), "tarjetas.json");
            }

            _logger.LogInformation("Exportación de datos a ZIP finalizada.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al exportar datos a ZIP");
            throw new ExportFromZipException("Ocurrió un error al intentar exportar datos al archivo ZIP.", e);
        }
    }
    
    public async Task ImportFromZip(string zipFilePath, string destinationDirectory)
    {
        _logger.LogInformation("Iniciando la importación de datos desde ZIP...");

        try
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            ZipFile.ExtractToDirectory(zipFilePath, destinationDirectory, overwriteFiles: true);
            _logger.LogInformation($"Archivos descomprimidos en {destinationDirectory}");

            var usuariosFile = new FileInfo(Path.Combine(destinationDirectory, "usuarios.json"));
            var clientesFile = new FileInfo(Path.Combine(destinationDirectory, "clientes.json"));
            var basesFile = new FileInfo(Path.Combine(destinationDirectory, "bases.json"));
            var cuentasFile = new FileInfo(Path.Combine(destinationDirectory, "cuentas.json"));
            var tarjetasFile = new FileInfo(Path.Combine(destinationDirectory, "tarjetas.json"));

            if (usuariosFile.Exists)
            {
                var usuarios = _storageJson.ImportJson<UserRequest>(usuariosFile);
                //hacemos un bucle for ya que no hay un saveAll
                foreach (var user in usuarios)
                {
                    await _userService.CreateAsync(user);
                }
            }

            if (clientesFile.Exists)
            {
                var clientes = _storageJson.ImportJson<ClienteRequest>(clientesFile);
                foreach (var cliente in clientes)
                {
                    await _clienteService.CreateAsync(cliente);
                }
            }

            if (basesFile.Exists)
            {
                var bases = _storageJson.ImportJson<BaseRequest>(basesFile);
                foreach (var baseEntity in bases)
                {
                    await _baseService.CreateAsync(baseEntity);
                }
            }
            
            if (cuentasFile.Exists)
            {
                var cuentas = _storageJson.ImportJson<CuentaRequest>(cuentasFile);
                foreach (var cuentaDto in cuentas)
                {
                    await _cuentaService.save(GuidGenerator.GenerarId(),cuentaDto);
                }
            }
            
            if (tarjetasFile.Exists)
            {   
                var tarjetas = _storageJson.ImportJson<TarjetaRequestDto>(tarjetasFile);
                foreach (var tarjetaDto in tarjetas)
                {
                    await _tarjetaService.CreateAsync(tarjetaDto);
                }
            }

            _logger.LogInformation("Importación de datos desde ZIP finalizada.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la importación de datos desde ZIP");
            throw new ImportFromZipException("Ocurrió un error durante la importación de datos desde el archivo ZIP.", ex);
        }
    }
}