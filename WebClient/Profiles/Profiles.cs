using AutoMapper;
using LibraryGQL;
using StrawberryShake;
using WebClient.DTO;
using WebClient.DTO.Authors;

namespace WebClient.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<CreateBookDto, CreateBookInput>();
        CreateMap<UpdateBookDto, UpdateBookInput>();
        CreateMap<UpdateBookInput, UpdateBookDto>();
        CreateMap<BookDto, UpdateBookDto>()
            .ForMember(dest => dest.Description, opt => opt.NullSubstitute(""));
        CreateMap<IGetBooks_Books, BookDto>();
        CreateMap<IGetBookById_BookById_Book, BookDto>();
        CreateMap<ICreateBook_CreateBook_Book, BookDto>();
        CreateMap<IUpdateBook_UpdateBook_Book, BookDto>();
        CreateMap<IGetBooks_Books_Data, BookDto>();
        CreateMap<IGetAuthorByName_AuthorByName_Authors, AuthorDto>();
        CreateMap<IGetAuthors_Authors_Data, AuthorDto>();
    }
}