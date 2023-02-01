﻿using StrawberryShake;
using WebClient.DTO;

namespace WebClient.Services.Interfaces;

public interface IGenericService<T> 
{
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<(T, IReadOnlyList<IClientError>, bool IsSuccess)> Create<K>(K createDto);
    Task<T> Update<K>(int id,  K updateDto);
    Task<T> Delete(int id);
}