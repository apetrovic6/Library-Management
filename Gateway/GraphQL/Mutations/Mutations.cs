using Authors;
using AutoMapper;
using Books;
using Gateway.GraphQL.Inputs;
using Grpc.Net.Client;

namespace Gateway.GraphQL.Mutations;

public class Mutations
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public Mutations(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<CreateBookResponse> CreateBook(CreateBookInput bookInput)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var request = _mapper.Map<CreateBookRequest>(bookInput);
        return await client.CreateBookAsync(request);
    }

    public async Task<UpdateBookResponse> UpdateBook(int id, UpdateBookInput bookInput)
    {
        bookInput.Id = id;
        
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var request = _mapper.Map<UpdateBookRequest>(bookInput);
        return await client.UpdateBookAsync(request);
    }
    
    public async Task<DeleteBookResponse> DeleteBook(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var request = new DeleteBookRequest { Id = id };
        return client.DeleteBook(request);
    }

    public async Task<CreateAuthorResponse> CreateAuthor(CreateAuthorInput authorInput)
    {
        var channel = GrpcChannel.ForAddress(_configuration["AuthorService"]);
        var client = new Author.AuthorClient(channel);
        var request = new CreateAuthorRequest { Name = authorInput.Name };

        return await client.CreateAuthorAsync(request);
    }

    public async Task<UpdateAuthorResponse> UpdateAuthor(int id, UpdateAuthorInput authorInput)
    {
        authorInput.Id = id;
        
        var channel = GrpcChannel.ForAddress(_configuration["AuthorService"]);
        var client = new Author.AuthorClient(channel);
        var request = new UpdateAuthorRequest { Id = authorInput.Id, Name = authorInput.Name};

        return await client.UpdateAuthorAsync(request);
    }

    public async Task<DeleteAuthorResponse> DeleteAuthor(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["AuthorService"]);
        var client = new Author.AuthorClient(channel);
        var request = new DeleteAuthorRequest { Id = id };

        return await client.DeleteAuthorAsync(request);
    }
}