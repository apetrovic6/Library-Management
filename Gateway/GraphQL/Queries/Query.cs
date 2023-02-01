using AutoMapper;
using Books;
using Gateway.GraphQL.Inputs;
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

    public async Task<List<BookType>> GetBooks(PagingInput paging)
    {
        var pagingInfo = new PageInfo { Page = paging.Page, PageSize = paging.PageSize };
        var bookRequest = new GetBooksRequest { Pageinfo = pagingInfo };
        
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        var booksRes = await client.GetBooksAsync(bookRequest);
        var bookList = booksRes.Data.ToList();
        
        return _mapper.Map<List<BookType>>(bookList);
    }

    public async Task<GetBookByIdResponse> GetBookById(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration["BooksService"]);
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBookByIdAsync(new GetBookByIdRequest{ Id = id});
    }
}