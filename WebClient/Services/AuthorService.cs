using AutoMapper;
using LibraryGQL;
using MudBlazor.Extensions;
using StrawberryShake;
using WebClient.DTO;
using WebClient.DTO.Authors;
using WebClient.Services.Interfaces;

namespace WebClient.Services;

public class AuthorService : IGenericService<AuthorDto>
{
    private readonly LibraryClient _client;
    private readonly IMapper _mapper;

    public AuthorService(LibraryClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }
    public async Task<(PagedResult<AuthorDto>, IReadOnlyList<IClientError>, bool IsSuccess)> GetAll<K, M>(K pagingInput, M filterInput)
    {
        var input = pagingInput.As<PagingInput>();
        var filters = filterInput.As<AuthorFilterInput>();
        var res = await _client.GetAuthors.ExecuteAsync(input, filters);
        var data = res.Data.Authors.Data;
        
    
        
        var authorList = _mapper.Map<IReadOnlyList<IGetAuthors_Authors_Data>, List<AuthorDto>>(data);

        var pagedResult = new PagedResult<AuthorDto>
            { Data = authorList, PageInfo = new PageInfo { Total = res.Data.Authors.PageInfo.Total } };

        return (pagedResult, res.Errors, res.IsSuccessResult());
    }

    public Task<AuthorDto> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<(AuthorDto, IReadOnlyList<IClientError>, bool IsSuccess)> Create<K>(K createDto)
    {
        throw new NotImplementedException();
    }

    public Task<(AuthorDto, IReadOnlyList<IClientError>, bool IsSuccess)> Update<K>(int id, K updateDto)
    {
        throw new NotImplementedException();
    }

    public Task<(bool, IReadOnlyList<IClientError>)> Delete(int id)
    {
        throw new NotImplementedException();
    }
}