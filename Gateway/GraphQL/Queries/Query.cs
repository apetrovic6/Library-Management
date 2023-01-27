using Books;
using Grpc.Net.Client;

namespace Gateway.GraphQL;

public class Query
{
    private readonly IConfiguration _configuration;

    public Query(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<GetBooksResponse> GetBooks()
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBooksAsync(new GetBooksRequest());
    }
}