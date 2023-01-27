using AutoMapper;
using Books.Data;
using Books.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Books.Services;

public class BookService : Books.BooksBase
{
    private readonly BooksDbContext _context;
    private readonly IMapper _mapper;

    public BookService(BooksDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<GetBooksResponse> GetBooks(GetBooksRequest request, ServerCallContext context)
    {
        var books = await _context.Books.ToListAsync();
        return _mapper.Map<GetBooksResponse>(books);
    }

    public override async Task<CreateBookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
        var newBook = _mapper.Map<Book>(request);

        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateBookResponse>(newBook);
    }

    public async override Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
    {
        var book = await _context.Books.Where(book => book.Id == request.Id).FirstOrDefaultAsync();
        
        _context.Entry(book).CurrentValues.SetValues(request);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<UpdateBookResponse>(book);
    }

    public override async Task<DeleteBookResponse> DeleteBook(DeleteBookRequest request, ServerCallContext context)
    {
        var bookToDelete = await _context.Books.Where(b => b.Id == request.Id).FirstOrDefaultAsync();
        
        if (bookToDelete == null) return  new DeleteBookResponse { Deleted = false, Message = "Not Found"};
        
        _context.Books.Remove(bookToDelete);
        var deleted = (await _context.SaveChangesAsync()) > 0;
         
         return new DeleteBookResponse { Deleted = deleted, Message = $"Deleted Book: {bookToDelete.Name}"};
    }
}