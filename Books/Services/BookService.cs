using Books.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Books.Services;

public class BookService : Books.BooksBase
{
    private readonly BooksDbContext _context;

    public BookService(BooksDbContext context)
    {
        _context = context;
    }

    public override async Task<GetBooksResponse> GetBooks(GetBooksRequest request, ServerCallContext context)
    {
        var books = await _context.Books.ToListAsync();
    
        var response = new GetBooksResponse();
    
        foreach (var book in books)
        {
            response.Books.Add(new BookModel() { Id = book.Id, Name = book.Name, Stock = book.Stock});
        }
    
        return response;
    }
}