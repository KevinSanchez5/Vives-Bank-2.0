﻿using Banco_VivesBank.Cliente.Exceptions;
using Banco_VivesBank.Cliente.Services;
using Banco_VivesBank.Movimientos.Database;
using Banco_VivesBank.Movimientos.Dto;
using Banco_VivesBank.Movimientos.Exceptions;
using Banco_VivesBank.Movimientos.Models;
using Banco_VivesBank.Utils.Validators;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Banco_VivesBank.Movimientos.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMongoCollection<Movimiento> _movimientoCollection;
    private readonly ILogger<MovimientoService> _logger;
    private readonly IClienteService _clienteService;
    
    public MovimientoService(IOptions<MovimientosMongoConfig> movimientosDatabaseSettings, ILogger<MovimientoService> logger, IClienteService clienteService)
    {
        _logger = logger;
        _clienteService = clienteService;
        var mongoClient = new MongoClient(movimientosDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(movimientosDatabaseSettings.Value.DatabaseName);
        _movimientoCollection = mongoDatabase.GetCollection<Movimiento>(movimientosDatabaseSettings.Value.CategoriasCollectionName);
    }
    
    public async Task<IEnumerable<Movimiento>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todos los movimientos en la base de datos");
        return await _movimientoCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Movimiento?> GetByIdAsync(string id)
    {
        _logger.LogInformation($"Buscando movimiento con id: {id}");
        
        if (!ObjectId.TryParse(id, out var objectId))
        {
            _logger.LogWarning($"Id con formáto inválido, debe ser un ObjectId: {id}");
            return null;
        }
        
        _logger.LogInformation("Buscando movimiento en base de datos");
        var movimiento = await _movimientoCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        
        if (movimiento != null)
        {
            _logger.LogInformation("Movimiento encontrado en base de datos.");
            return movimiento;
        }

        _logger.LogInformation($"Movimiento no encontrado con id: {id}");
        return null;
    }

    public async Task<Movimiento?> GetByGuidAsync(string guid)
    {
        _logger.LogInformation($"Buscando movimiento con guid: {guid}");
        
        _logger.LogInformation("Buscando movimiento en base de datos");
        var movimiento = await _movimientoCollection.Find(m => m.Guid == guid).FirstOrDefaultAsync();
        
        if (movimiento != null)
        {
            _logger.LogInformation("Movimiento encontrado en base de datos.");
            return movimiento;
        }
        
        _logger.LogInformation($"Movimiento no encontrado con guid: {guid}");
        return null;
    }

    public async Task<Movimiento?> GetByClinteGuidAsync(string clienteGuid)
    {
        _logger.LogInformation($"Buscando movimiento por guid del cliente: {clienteGuid}");
        
        _logger.LogInformation("Buscando movimiento en base de datos");
        var movimiento = await _movimientoCollection.Find(m => m.Cliente.Guid == clienteGuid).FirstOrDefaultAsync();
        
        if (movimiento != null)
        {
            _logger.LogInformation("Movimiento encontrado en base de datos.");
            return movimiento;
        }
        
        _logger.LogInformation($"Movimiento no encontrado por guid del cliente: {clienteGuid}");
        return null;
    }

    public async Task<Movimiento> CreateAsync(MovimientoRequest movimientoRequest)
    {
        _logger.LogInformation("Guardando movimiento");
        var clienteExistente = await _clienteService.GetClienteModelByGuid(movimientoRequest.ClienteGuid);
        if (clienteExistente == null)
        {
            _logger.LogInformation("El cliente asignado al movimiento no existe o no se ha encontrado");
            throw new ClienteNotFoundException("El cliente asignado al movimiento no existe o no se ha encontrado");
        }

        Movimiento nuevoMovimiento = new Movimiento
        {
            Cliente = clienteExistente,
            Domiciliacion = movimientoRequest.Domiciliacion,
            IngresoNomina = movimientoRequest.IngresoNomina,
            PagoConTarjeta = movimientoRequest.PagoConTarjeta,
            Transferencia = movimientoRequest.Transferencia
        };
        
        await _movimientoCollection.InsertOneAsync(nuevoMovimiento);
        _logger.LogInformation("Movimiento guardado con éxito");

        return nuevoMovimiento;
    }

    public async Task<Movimiento> CreateDomiciliacionAsync(Domiciliacion domiciliacion)
    {
        _logger.LogInformation("Creando domiciliación");
        
        _logger.LogInformation("Validando iban de origen");
        if (!IbanValidator.ValidarIban.ValidateIban(domiciliacion.IbanOrigen))
        {
            _logger.LogInformation($"El iban de origen: {domiciliacion.IbanOrigen} no es válido");
            throw new NotValidIbanException($"El iban de origen: {domiciliacion.IbanOrigen} no es válido");
        }
        
        _logger.LogInformation("Validando iban de destino");
        if (!IbanValidator.ValidarIban.ValidateIban(domiciliacion.IbanDestino))
        {
            _logger.LogInformation($"El iban de destino: {domiciliacion.IbanDestino} no es válido");
            throw new NotValidIbanException($"El iban de destino: {domiciliacion.IbanDestino} no es válido");
        }

        // TODO Validar que el cliente existe (cliente autenticado)
        
        _logger.LogInformation("Validando cuenta existente");
        throw new NotImplementedException();
    }

    public async Task<Movimiento> CreateIngresoNominaAsync(IngresoNomina ingresoNomina)
    {
        throw new NotImplementedException();
    }

    public async Task<Movimiento> CreatePagoConTarjetaAsync(PagoConTarjeta pagoConTarjeta)
    {
        throw new NotImplementedException();
    }

    public async Task<Movimiento> CreateTransferenciaAsync(Transferencia transferencia)
    {
        throw new NotImplementedException();
    }

    public async Task<Movimiento> RevocarTransferencia(string movimientoGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<Movimiento?> UpdateAsync(string id, Movimiento movimiento)
    {
        throw new NotImplementedException();
    }

    public async Task<Movimiento?> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}