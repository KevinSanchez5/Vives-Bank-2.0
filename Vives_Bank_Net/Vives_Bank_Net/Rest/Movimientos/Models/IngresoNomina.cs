﻿using System.Text.Json.Serialization;

namespace Vives_Bank_Net.Rest.Movimientos.Models;

public abstract class IngresoNomina
{
    [JsonPropertyName("ibanOrigen")]
    public required string IbanOrigen { get; set; }
    
    [JsonPropertyName("ibanDestino")]
    public required string IbanDestino { get; set; }
    
    [JsonPropertyName("cantidad")]
    public required decimal Cantidad { get; set; }
    
    [JsonPropertyName("nombreEmpresa")]
    public required string NombreEmpresa { get; set; }
    
    [JsonPropertyName("cifEmpresa")]
    public required string CifEmpresa { get; set; }
}