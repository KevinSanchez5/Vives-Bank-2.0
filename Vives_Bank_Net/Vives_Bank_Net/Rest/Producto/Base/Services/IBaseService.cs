﻿using DefaultNamespace;
using Vives_Bank_Net.Rest.Producto.Base.Models;

namespace Vives_Bank_Net.Rest.Producto.Base.Services;

public interface IBaseService
{
    public Task<List<BaseModel>> GetAllAsync();
    public Task<BaseResponseDto> GetByGuidAsync(string id);
    public Task<BaseResponseDto> CreateAsync(BaseRequestDto baseRequest);
    public Task<BaseResponseDto> UpdateAsync(string id, BaseUpdateDto baseUpdate);
    public Task<BaseResponseDto> DeleteAsync(string id);
}