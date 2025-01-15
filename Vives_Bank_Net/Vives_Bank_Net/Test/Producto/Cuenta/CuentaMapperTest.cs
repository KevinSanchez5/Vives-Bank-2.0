﻿using NUnit.Framework;
using Vives_Bank_Net.Rest.Producto.Cuenta.Database;
using Vives_Bank_Net.Rest.Producto.Cuenta.Mappers;
using Vives_Banks_Net.Utils.Generators;

namespace Vives_Bank_Net.Test.Producto.Cuenta;

[TestFixture]
public class CuentaMapperTest
{
    [Test]
    public void ToCuentaEntity_MapsCorrectly()
    {
        // Arrange
        var cuenta = new Rest.Producto.Cuenta.Cuenta()
        {
            Guid = GuidGenerator.GenerarId(),
            Iban = IbanGenerator.GenerateIban(),
            Saldo = 1000,
            TarjetaId = 1,
            ClienteId = 2,
            ProductoId = 3,
            IsDeleted = false
        };

        // Act
        var result = cuenta.ToCuentaEntity();

        // Assert
        Assert.That(result, Is.EqualTo(cuenta));
        Assert.That(result.Guid, Is.EqualTo(cuenta.Guid));
        Assert.That(result.Iban, Is.EqualTo(cuenta.Iban));

    }

    [Test]
    public void ToCuentaResponse_MapsCorrectly()
    {
        // Arrange
        var cuentaEntity = new CuentaEntity
        {
            Guid = GuidGenerator.GenerarId(),
            Iban = IbanGenerator.GenerateIban(),
            Saldo = 1500,
            TarjetaId = 4,
            ClienteId = 5,
            ProductoId = 6,
            IsDeleted = true
        };

        // Act
        var result = cuentaEntity.ToCuentaResponse();

        // Assert
        Assert.That(result, Is.EqualTo(cuentaEntity));
        Assert.That(result.Guid, Is.EqualTo(cuentaEntity.Guid));
        Assert.That(result.Iban, Is.EqualTo(cuentaEntity.Iban));
    }
}