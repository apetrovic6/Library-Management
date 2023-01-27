using AutoMapper;
using Books;
using Grpc.Net.Client;

namespace Gateway.GraphQL;

public class Query
{
    private readonly IConfiguration _configuration;

    public Query(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
    }
    
    public async Task<GetBooksResponse> GetBooks()
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBooksAsync(new GetBooksRequest());
    }

    public async Task<GetBookByIdResponse> GetBookById(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBookByIdAsync(new GetBookByIdRequest{ Id = id});
    }
}