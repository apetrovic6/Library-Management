using Authors;
using AutoMapper;
using Books;
using Gateway.GraphQL.Inputs;
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

    public async Task<GetBooksResponse> GetBooks(PagingInput paging, BookFilterInput bookFilterInput)
    {
           
        var bookFilter = new BookFilters() { AuthorName = bookFilterInput.AuthorName };
        var pagingInfo = new PageInfo { Page = paging.Page, PageSize = paging.PageSize };
        
        var bookRequest = new GetBooksRequest { PageInfo = pagingInfo, Filters = bookFilter};

        var channel = GrpcChannel.ForAddress(_configuration.GetConnectionString("BooksService"));
        var client = new Books.Books.BooksClient(channel);

        return await client.GetBooksAsync(bookRequest);
    }

    public async Task<GetBookByIdResponse> GetBookById(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetConnectionString("BooksService"));
        var client = new Books.Books.BooksClient(channel);
        return await client.GetBookByIdAsync(new GetBookByIdRequest { Id = id });
    }

    public async Task<GetAuthorsResponse> GetAuthors(PagingInput paging, AuthorFilterInput filterInput)
    {
        var filters = new AuthorFilters { AuthorName = filterInput.AuthorName };
        var pagingInfo = new AuthorPageInfo() { Page = paging.Page, PageSize = paging.PageSize };
        var authorsRequest = new GetAuthorsRequest { PageInfo = pagingInfo , Filters = filters};

        var channel = GrpcChannel.ForAddress(_configuration.GetConnectionString("AuthorService"));
        var client = new Author.AuthorClient(channel);
        return await client.GetAuthorsAsync(authorsRequest);
    }

    public async Task<GetAuthorByIdResponse> GetAuthorById(int id)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetConnectionString("AuthorService"));
        var client = new Author.AuthorClient(channel);
        return await client.GetAuthorByIdAsync(new GetAuthorByIdRequest { Id = id });
    }

    public async Task<GetAuthorByNameResponse> GetAuthorByName(string name)
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetConnectionString("AuthorService"));
        var client = new Author.AuthorClient(channel);
        return await client.GetAuthorByNameAsync(new GetAuthorByNameRequest { Name = name });
    }
}