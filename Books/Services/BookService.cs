using Books.Data;
using Books.Models;
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

    public override async Task<CreateBookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
        var newBook = new Book() { Name = request.Name, Stock = request.Stock };

        await _context.Books.AddAsync(newBook);

        await _context.SaveChangesAsync();
        
        CreateBookResponse response = new () {Book = new BookModel{ Id = newBook.Id, Name = newBook.Name, Stock = newBook.Stock }};
        
        return response;
    }
}