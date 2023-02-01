using AutoMapper;
using Books.Data;
using Books.Models;
using Grpc.Core;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Books.Services;

public class BookService : Books.BooksBase
{
    private readonly IDbContextFactory<BooksDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public BookService(IDbContextFactory<BooksDbContext> context, IMapper mapper)
    {
        _contextFactory = context;
        _mapper = mapper;
    }

    public override async Task<GetBooksResponse> GetBooks(GetBooksRequest request, ServerCallContext context)
    {
        var pageInfo = new Paging(request.Pageinfo.Page, request.Pageinfo.PageSize);
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var books = await dbContext.Books
            .Skip(pageInfo.Skip)
            .Take(pageInfo.PageSize)
            .ToListAsync();

        var totalCount = await dbContext.Books.CountAsync();

        var info = new PageInfo { Total = totalCount };
        
        var result = new PagedResult<Book>(books, info);
        await dbContext.DisposeAsync();
        return _mapper.Map<GetBooksResponse>(result);
    }

    public override async Task<GetBookByIdResponse> GetBookById(GetBookByIdRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var book = await dbContext.Books.Where(b => b.Id == request.Id).FirstOrDefaultAsync();

        await dbContext.DisposeAsync();
        return _mapper.Map<GetBookByIdResponse>(book);
    }

    public override async Task<CreateBookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(); 
        var newBook = _mapper.Map<Book>(request);
    
        await dbContext.Books.AddAsync(newBook);
        await dbContext.SaveChangesAsync();

        await dbContext.DisposeAsync();
        return _mapper.Map<CreateBookResponse>(newBook);
    }
    
    public override async Task<UpdateBookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var book = await dbContext.Books.Where(book => book.Id == request.Id).FirstOrDefaultAsync();
        
        dbContext.Entry(book).CurrentValues.SetValues(request);
        await dbContext.SaveChangesAsync();

        await dbContext.DisposeAsync();
        return _mapper.Map<UpdateBookResponse>(book);
    }
    
    public override async Task<DeleteBookResponse> DeleteBook(DeleteBookRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var bookToDelete = await dbContext.Books.Where(b => b.Id == request.Id).FirstOrDefaultAsync();
        
        if (bookToDelete == null) return  new DeleteBookResponse { Deleted = false, Message = "Not Found"};
        
        if (bookToDelete == null) throw new GraphQLException(new Error("Book not found"));
        
        dbContext.Books.Remove(bookToDelete);
        var deleted = (await dbContext.SaveChangesAsync()) > 0;
         
        await dbContext.DisposeAsync();
         return new DeleteBookResponse { Deleted = deleted, Message = $"Deleted Book: {bookToDelete.Title}"};
    }
}