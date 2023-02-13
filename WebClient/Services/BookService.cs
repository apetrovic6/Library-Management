using AutoMapper;
using LibraryGQL;
using MudBlazor.Extensions;
using StrawberryShake;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Services;

public class BookService : IGenericService<BookDto>
{
    private readonly LibraryClient _client;
    private readonly IMapper _mapper;

    public BookService(LibraryClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }
    public async Task<(PagedResult<BookDto>, IReadOnlyList<IClientError>, bool IsSuccess)> GetAll<PagingInput, Filterinput>(PagingInput pagingInput,Filterinput filterInput)
    {
        var input = pagingInput.As<LibraryGQL.PagingInput>();
        var filter = filterInput.As<BookFilterInput>();
        var res = await _client.GetBooks.ExecuteAsync(input, filter);
        var data = res.Data?.Books.Data;
        var bookList = _mapper.Map<IReadOnlyList<IGetBooks_Books_Data>, List<BookDto>>(data);
        var pagedResult = new PagedResult<BookDto>
            { Data = bookList, PageInfo = new PageInfo { Total = res.Data?.Books?.PageInfo?.Total } };

        return (pagedResult, res.Errors, res.IsSuccessResult());
    }

    public async Task<BookDto> GetById(int id)
    {
        var res = await _client.GetBookById.ExecuteAsync(id);
        return _mapper.Map<BookDto>(res.Data.BookById.Book);
    }
    

    public async Task<(BookDto, IReadOnlyList<IClientError>, bool IsSuccess)> Create<K>(K createDto)
    {
        var createBookInput = _mapper.Map<CreateBookInput>(createDto);
        var res = await _client.CreateBook.ExecuteAsync(createBookInput);
        var createdBook = _mapper.Map<BookDto>(res.Data.CreateBook.Book);

        return (createdBook, res.Errors, res.IsSuccessResult());
    }

    public async Task<(BookDto, IReadOnlyList<IClientError>, bool IsSuccess)> Update<K>(int id, K updateDto)
    {
        try
        {
            var updateInput = _mapper.Map<UpdateBookInput>(updateDto);

            var res = await _client.UpdateBook.ExecuteAsync(id, updateInput);
            var updatedBook = _mapper.Map<IUpdateBook_UpdateBook_Book, BookDto>(res.Data.UpdateBook.Book);
            var a = res.Data.UpdateBook.Book;
            return (updatedBook, res.Errors, res.IsSuccessResult());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<(bool,IReadOnlyList<IClientError>)> Delete(int id)
    {
        var res = await _client.DeleteBook.ExecuteAsync(id);
        return (res.Data.DeleteBook.Deleted, res.Errors);
    }
}