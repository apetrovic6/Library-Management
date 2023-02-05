using Books.Models;
using StrawberryShake;

namespace WebClient.Services.Interfaces;

public interface IGenericService<T> 
{
    Task<(PagedResult<T>, IReadOnlyList<IClientError>, bool IsSuccess)> GetAll<K, M>(K pagingInput, M filterInput);
    Task<T> GetById(int id);
    Task<(T, IReadOnlyList<IClientError>, bool IsSuccess)> Create<K>(K createDto);
    Task<(T, IReadOnlyList<IClientError>, bool IsSuccess)> Update<K>(int id,  K updateDto);
    Task<(bool,IReadOnlyList<IClientError>)> Delete(int id);
}