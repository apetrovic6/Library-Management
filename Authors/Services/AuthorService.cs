using Authors.Data;
using Authors.Models;
using AutoMapper;
using Grpc.Core;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Authors.Services;

public class AuthorService : Author.AuthorBase
{
    private readonly IDbContextFactory<AuthorsDbContext> _contextFactory;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;

    public AuthorService(IDbContextFactory<AuthorsDbContext> _contextFactory, IMapper mapper,
        IMessageBusClient messageBusClient)
    {
        this._contextFactory = _contextFactory;
        _mapper = mapper;
        _messageBusClient = messageBusClient;
    }
    public override async Task<GetAuthorsResponse> GetAuthors(GetAuthorsRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var authors = await dbContext.Authors.ToListAsync();
    
        await dbContext.DisposeAsync();
        return _mapper.Map<GetAuthorsResponse>(authors);
    }

    public override async Task<GetAuthorByIdResponse> GetAuthorById(GetAuthorByIdRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var author = await dbContext.Authors.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
    
        await dbContext.DisposeAsync();
        return _mapper.Map<GetAuthorByIdResponse>(author);
    }

    public override async Task<CreateAuthorResponse> CreateAuthor(CreateAuthorRequest request, ServerCallContext context)
    {
        await using var dbContext = await  _contextFactory.CreateDbContextAsync();
        var author = _mapper.Map<AuthorEntity>(request);

        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        await dbContext.DisposeAsync();
        return _mapper.Map<CreateAuthorResponse>(author);
    }

    public override async Task<UpdateAuthorResponse> UpdateAuthor(UpdateAuthorRequest request, ServerCallContext context)
    {
        await using var dbContext = await  _contextFactory.CreateDbContextAsync();

        var author = await dbContext.Authors.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        
        if (author == null) throw new GraphQLException("Author not found");
        
        dbContext.Entry(author).CurrentValues.SetValues(request);
        await dbContext.SaveChangesAsync();

        try
        {
            var published = new AuthorPublishedDto() { Name = request.Name, Id = request.Id, Event = "Author_Updated" };
            _messageBusClient.Publish(published);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not publish: {e.Message}");
        }

        await dbContext.DisposeAsync();
        return _mapper.Map<UpdateAuthorResponse>(author);
    }

    public override async Task<DeleteAuthorResponse> DeleteAuthor(DeleteAuthorRequest request, ServerCallContext context)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var authorToDelete = await dbContext.Authors.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        
        if (authorToDelete == null) return  new DeleteAuthorResponse { Deleted = false, Message = "Not Found"};
        
        if (authorToDelete == null) throw new GraphQLException(new Error("Author not found"));
        
        dbContext.Authors.Remove(authorToDelete);
        var deleted = (await dbContext.SaveChangesAsync()) > 0;
         
        await dbContext.DisposeAsync();
        return new DeleteAuthorResponse { Deleted = deleted, Message = $"Deleted Author: {authorToDelete.Name}"};
    }
}