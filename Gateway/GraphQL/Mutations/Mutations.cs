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

    public async Task<UpdateBookResponse> UpdateBook(int id, string title, int stock)
    {
        bookInput.Id = id;
        
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var request = new UpdateBookRequest { Id = id, Title = title, Stock = stock};
        return await client.UpdateBookAsync(request);
    }
    
    public async Task<DeleteBookResponse> DeleteBook(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var request = new DeleteBookRequest { Id = id };
        return client.DeleteBook(request);
    }
}