using AutoMapper;
using Books;
using Gateway.GraphQL.Types;
using Grpc.Net.Client;

namespace Gateway.GraphQL;

public class Query
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public Query(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public async Task<GetBooksResponse> GetBooks()
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var booksRes = await client.GetBooksAsync(new GetBooksRequest());
        var bookList = booksRes.Books.ToList();
        
        return _mapper.Map<List<BookType>>(bookList);
    }

    public async Task<GetBookByIdResponse> GetBookById(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBookByIdAsync(new GetBookByIdRequest{ Id = id});
    }
}