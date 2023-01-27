using AutoMapper;
using Books;
using Gateway.GraphQL.Types;

namespace Gateway.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<GetBooksResponse, BookType>();
        CreateMap<DeleteBookResponse, BookType>();
    }
}