using AutoMapper;
using Books.Models;
using BooksGQL;
using MudBlazor.Extensions;
using StrawberryShake;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Services;

public class BookService : IGenericService<Book>
{
    private readonly BooksClient _client;
    private readonly IMapper _mapper;

    public BookService(BooksClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }
    public async Task<(PagedResult<Book>, IReadOnlyList<IClientError>, bool IsSuccess)> GetAll<PagingInput>(PagingInput pagingInput)
    {
        var input = pagingInput.As<BooksGQL.PagingInput>();
        
        var res = await _client.GetBooks.ExecuteAsync(input);
        var a = res.Data.Books.Data;
        var bookList = _mapper.Map<IReadOnlyList<IGetBooks_Books_Data>, List<Book>>(a);
        var pagedResult = new PagedResult<Book>
            { Data = bookList, PageInfo = new PageInfo { Total = res.Data.Books.PageInfo.Total } };

        return (pagedResult, res.Errors, res.IsSuccessResult());
    }

    public async Task<Book> GetById(int id)
    {
        var res = await _client.GetBookById.ExecuteAsync(id);
        return _mapper.Map<Book>(res.Data.BookById.Book);
    }
    

    public async Task<(Book, IReadOnlyList<IClientError>, bool IsSuccess)> Create<K>(K createDto)
    {
        var createBookInput = _mapper.Map<CreateBookInput>(createDto);
        var res = await _client.CreateBook.ExecuteAsync(createBookInput);
        var createdBook = _mapper.Map<Book>(res.Data.CreateBook.Book);

        return (createdBook, res.Errors, res.IsSuccessResult());
    }

    public async Task<(Book, IReadOnlyList<IClientError>, bool IsSuccess)> Update<K>(int id, K updateDto)
    {
        try
        {
            var updateInput = _mapper.Map<UpdateBookInput>(updateDto);

            var res = await _client.UpdateBook.ExecuteAsync(id, updateInput);
            var updatedBook = _mapper.Map<IUpdateBook_UpdateBook_Book, Book>(res.Data.UpdateBook.Book);
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